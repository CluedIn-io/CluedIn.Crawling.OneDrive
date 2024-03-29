using System;
using System.Collections.Generic;
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
    public class OneDriveClient
    {
        private readonly ILogger log;

        private readonly OneDriveCrawlJobData onedriveCrawlJobData;

        public readonly GraphServiceClient graphClient;

        public OneDriveClient(ILogger log, OneDriveCrawlJobData onedriveCrawlJobData, IRestClient client)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.onedriveCrawlJobData = onedriveCrawlJobData ?? throw new ArgumentNullException(nameof(onedriveCrawlJobData));

            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                   .Create(onedriveCrawlJobData.ClientID)
                   .WithAuthority($"https://login.microsoftonline.com/{onedriveCrawlJobData.Tenant}/v2.0")
                   .WithClientSecret(onedriveCrawlJobData.ClientSecret)
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

        public IEnumerable<DriveItem> GetDriveItems(Drive drive)
        {
            IDriveItemChildrenCollectionPage items = null;
            try
            {
                items = graphClient.Drives[drive.Id].Root.Children.Request().GetAsync().Result;
            }
            catch
            {
                log.Error("Could not get items for Drive " + drive.Name);
            }

            if (items != null)
                foreach (var item in items)
                    yield return item;
        }

        public IEnumerable<IDriveItemChildrenCollectionPage> GetDriveItems(string id = null)
        {
            IDriveItemChildrenCollectionRequestBuilder driveItemChildrenCollectionRequest;
            IDriveItemChildrenCollectionPage data;
            if (id != null)
                driveItemChildrenCollectionRequest = graphClient.Drive.Items[id].Children;
            else
                driveItemChildrenCollectionRequest = graphClient.Drive.Root.Children;
            log.Info(driveItemChildrenCollectionRequest.RequestUrl);
            data = driveItemChildrenCollectionRequest.Request()
                //.OrderBy("CreatedDateTime")
                // .OrderBy("LastModifiedDateTime")
                //.Filter($"(CreatedDateTime ge '{onedriveCrawlJobData.LastCrawlFinishTime:yyyy-MM-ddThh:mm:ssZ}' or LastModifiedDateTime ge '{onedriveCrawlJobData.LastCrawlFinishTime:yyyy-MM-ddThh:mm:ssZ})'")
                .Top(100)
                .GetAsync().Result;
            foreach (var item in data)
            {
                if (item.Folder != null)
                    foreach (var page in GetDriveItems(item.Id))
                        yield return page;
            }

            var pageIterator = PageIterator<DriveItem>
    .CreatePageIterator(graphClient, data, (m) =>
    {
        return true;
    });
            pageIterator.IterateAsync();

            yield return data;
        }

        private async Task<T> GetAsync<T>(HttpMethod httpMethod, string url, HttpContent httpContent = null)
        {
            var request = new HttpRequestMessage(httpMethod, url);

            if (httpContent != null)
            {
                request.Content = httpContent;
            }

            var response = await ActionExtensions.ExecuteWithRetryAsync(async () => { return await graphClient.HttpProvider.SendAsync(request); });

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var diagnosticMessage = $"Request to {graphClient.BaseUrl}{url} failed, response {response.Content} ({response.StatusCode})";
                log.Error(() => diagnosticMessage);
                throw new InvalidOperationException($"Communication to jsonplaceholder unavailable. {diagnosticMessage}");
            }

            var data = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

            return data;
        }

        public async Task<T> PostAsync<T>(string url, HttpContent httpContent) =>
             await GetAsync<T>(HttpMethod.Post, url, httpContent);

        public async Task<T> GetAsync<T>(string url) =>
            await GetAsync<T>(HttpMethod.Get, url);

        public async Task<T> DeleteAsync<T>(string url, HttpContent httpContent = null) =>
            await GetAsync<T>(HttpMethod.Delete, url, httpContent);

        public async Task<T> PutAsync<T>(string url, HttpContent httpContent) =>
             await GetAsync<T>(HttpMethod.Put, url, httpContent);

        public AccountInformation GetAccountInformation()
        {
            var response = ActionExtensions.ExecuteWithRetry(() => { return graphClient.Users.Request().GetAsync().Result; });
            if (response == null)
                return new AccountInformation(string.Empty, string.Empty);
            else
                return new AccountInformation(onedriveCrawlJobData.ClientID, onedriveCrawlJobData.ClientID);
        }
    }
}
