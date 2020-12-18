using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CluedIn.Core;
using CluedIn.Core.Logging;
using CluedIn.Core.Providers;
using CluedIn.Crawling.OneDrive.Core;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using RestSharp;

namespace CluedIn.Crawling.OneDrive.Infrastructure
{
    public partial class OneDriveClient
    {
        private readonly ILogger log;

        private readonly OneDriveCrawlJobData onedriveCrawlJobData;

        public readonly GraphServiceClient graphClient;

        public OneDriveClient(ILogger log, OneDriveCrawlJobData onedriveCrawlJobData, IRestClient client)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.onedriveCrawlJobData = onedriveCrawlJobData ?? throw new ArgumentNullException(nameof(onedriveCrawlJobData));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            if (onedriveCrawlJobData.UseProxy)
                WebRequest.DefaultWebProxy = new System.Net.WebProxy(Environment.GetEnvironmentVariable("PROXY_URL", EnvironmentVariableTarget.Machine), true);

            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                   .Create(onedriveCrawlJobData.ClientID)
                   .WithAuthority($"https://login.microsoftonline.com/{onedriveCrawlJobData.Tenant}/v2.0")
                   .WithClientSecret(Environment.GetEnvironmentVariable(onedriveCrawlJobData.ClientSecret, EnvironmentVariableTarget.Machine))
                   .Build();

            graphClient =
              new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
              {
                  var authResult = await confidentialClientApplication
                        .AcquireTokenForClient(new string[] { "https://graph.microsoft.com/.default" })
                        .ExecuteAsync();

                  requestMessage.Headers.Authorization =
                     new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
              })
              );
        }

        public IEnumerable<User> GetUsers()
        {
            foreach (var user in graphClient.Users.Request().GetAsync().Result)
                yield return user;
        }

        public IEnumerable<Drive> GetDrives(User user)
        {
            IUserDrivesCollectionPage drives = null;
            try
            {
                drives = graphClient.Users[user.Id].Drives.Request().GetAsync().Result;
            }
            catch
            {
                log.Warn("Could not get Drives for User " + user.DisplayName);
            }
            if (drives != null)
            {
                log.Info("Got Drives for User" + user.DisplayName);
                foreach (var drive in drives)
                    yield return drive;
            }
        }

        public IEnumerable<DriveItem> GetDriveItems(Drive drive, string folder = null)
        {
            int pageSize = 100;
            IDriveItemChildrenCollectionPage page = null;
            try
            {
                IDriveItemChildrenCollectionRequestBuilder builder = null;
                if (folder != null)
                    builder = graphClient.Drives[drive.Id].Items[folder].Children;
                else
                    builder = graphClient.Drives[drive.Id].Root.Children;
                page = ActionExtensions.ExecuteWithRetry(() => { return builder.Request().Top(pageSize).GetAsync().Result; });
                //TODO - not supported by default .OrderBy("CreatedDateTime")
            }
            catch (Exception ex)
            {
                log.Error(() => ex.Message, ex);
            }
            if (page != null)
            {
                foreach (var item in page)
                {
                    if (item.Folder != null)
                        foreach (var folderItem in GetDriveItems(drive, item.Id))
                            yield return folderItem;
                    yield return item;
                }
                while (page.NextPageRequest != null)
                {
                    try
                    {
                        page = page.NextPageRequest.GetAsync().Result;
                    }
                    catch (Exception ex)
                    {
                        log.Error(() => ex.Message, ex);
                    }
                    if (page != null)
                        foreach (var item in page)
                            yield return item;
                }
            }
        }

        public AccountInformation GetAccountInformation()
        {
            var response = ActionExtensions.ExecuteWithRetry(() => { return graphClient.Users.Request().GetAsync().Result; });
            if (response == null)
                return new AccountInformation(string.Empty, string.Empty);
            else
                return new AccountInformation(onedriveCrawlJobData.ClientID, onedriveCrawlJobData.ClientID);
        }

        public DriveItem ReplaceFile(string userId, Drive drive, DriveItem item, Stream stream)
        {
            var otherStream = new MemoryStream();
            stream.CopyTo(otherStream);

            DriveItem result = null;

            try
            {
                result = graphClient.Users[userId].Drive.Items[item.Id].Content.Request().PutAsync<DriveItem>(stream).Result;
            }
            catch (Exception ex)
            {
                log.Error(() => $"Could not replace file in OneDrive. {ex.Message}", ex);

                result = graphClient.Drives[drive.Id].Items[item.Id].Content.Request().PutAsync<DriveItem>(otherStream).Result;
            }

            return result;
        }

        public UploadResult<DriveItem> ReplaceLargeFile(string userId, Drive drive, DriveItem item, Stream stream)
        {
            UploadResult<DriveItem> uploadResult = null;
            using (var fileStream = stream)
            {
                var uploadProps = new DriveItemUploadableProperties
                {
                    ODataType = null,
                    AdditionalData = new Dictionary<string, object>
                    {
                        { "@microsoft.graph.conflictBehavior", "replace" }
                    }
                };

                UploadSession uploadSession = null;

                try
                {
                    uploadSession = graphClient
                    .Users[userId]
                    .Drive
                    .Items[item.Id]
                    .CreateUploadSession(uploadProps)
                    .Request()
                    .PostAsync().Result;
                }
                catch (Exception ex)
                {
                    log.Error(() => $"Could not create uploadSession. {ex.Message}", ex);

                    uploadSession = graphClient
                    .Drives[drive.Id]
                    .Items[item.Id]
                    .CreateUploadSession(uploadProps)
                    .Request()
                    .PostAsync().Result;
                }
                

                int maxSliceSize = 320 * 1024;
                var fileUploadTask =
                    new LargeFileUploadTask<DriveItem>(uploadSession, fileStream, maxSliceSize);

                IProgress<long> progress = new Progress<long>(slice =>
                {
                    log.Info($"Uploaded {slice} bytes of {fileStream.Length} bytes");
                });

                try
                {
                    uploadResult = fileUploadTask.UploadAsync(progress).Result;

                    if (uploadResult.UploadSucceeded)
                    {
                        log.Info($"Upload complete, item ID: {uploadResult.ItemResponse.Id}");
                    }
                    else
                    {
                        log.Error("Upload failed");
                    }
                }
                catch (ServiceException ex)
                {
                    log.Error($"Error uploading: {ex}");
                }
            }
            return uploadResult;
        }
    }
}
