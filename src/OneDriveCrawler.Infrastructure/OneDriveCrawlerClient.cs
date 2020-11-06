using System;
using System.Net;
using System.Threading.Tasks;
using CluedIn.Core.Logging;
using CluedIn.Core.Providers;
using CluedIn.Crawling.OneDriveCrawler.Core;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.Graph;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using CluedIn.Crawling.OneDriveCrawler.Infrastructure.Helpers;
using System.Security;
using CluedIn.Crawling.OneDriveCrawler.Core.Models;


namespace CluedIn.Crawling.OneDriveCrawler.Infrastructure
{
    // TODO - This class should act as a client to retrieve the data to be crawled.
    // It should provide the appropriate methods to get the data
    // according to the type of data source (e.g. for AD, GetUsers, GetRoles, etc.)
    // It can receive a IRestClient as a dependency to talk to a RestAPI endpoint.
    // This class should not contain crawling logic (i.e. in which order things are retrieved)
    public class OneDriveCrawlerClient
    {
        private readonly GraphServiceClient client;

        public OneDriveCrawlerClient(OneDriveCrawlerCrawlJobData onedrivecrawlerCrawlJobData) 
        {
            if (onedrivecrawlerCrawlJobData == null)
            {
                throw new ArgumentNullException(nameof(onedrivecrawlerCrawlJobData));
            }


            //this.log = log ?? throw new ArgumentNullException(nameof(log));

            // this is required for the Msal Authentication since it takes the SecurePassword as a parameter
            SecureString pass = new NetworkCredential("", onedrivecrawlerCrawlJobData.Password).SecurePassword;

            client = GetAuthenticatedGraphClient(onedrivecrawlerCrawlJobData,
                onedrivecrawlerCrawlJobData.UserName,
                pass);
        }

        public IEnumerable<Microsoft.Graph.DriveItem> GetDriveItems()
        {
            var request = client.Me.Drive.Root.Children.Request();

            var results = request.GetAsync().Result;
            string driveId = null;
            foreach (var item in results)
            {
                // if not assigned, get the Drive ID
                if(driveId == null)
                {
                    driveId = item.ParentReference.DriveId;
                }

                // checking if an item is a folder to expand it and yield it's child items
                if (item.Folder != null && item.Folder.ChildCount > 0)
                {
                    List<Microsoft.Graph.DriveItem> children = ExpandFolder(driveId, item.Id);
                    foreach (var c in children)
                        yield return c;
                }

                yield return item;
            }
        }

        private List<Microsoft.Graph.DriveItem> ExpandFolder(string driveId, string folderId)
        {
            List<Microsoft.Graph.DriveItem> items = new List<Microsoft.Graph.DriveItem>();
            var children = client.Drives[driveId].Items[folderId].Children
                        .Request()
                        .GetAsync();

            // if subfolder encountered, recursively expand and concat the list of items
            foreach (var c in children.Result)
            {
                if(c.Folder != null)
                    items.AddRange(ExpandFolder(driveId, c.Id));

                items.Add(c);
            }

            return items;
        }

        private static IAuthenticationProvider CreateAuthorizationProvider(OneDriveCrawlerCrawlJobData config, string userName, SecureString userPassword)
        {
            var clientId = config.ApplicationId;
            var authority = $"https://login.microsoftonline.com/{config.TenantId}/v2.0";

            List<string> scopes = new List<string>();
            scopes.Add("User.Read");
            scopes.Add("Files.Read");

            var cca = PublicClientApplicationBuilder.Create(clientId)
                                                    .WithAuthority(authority)
                                                    .Build();
            return MsalAuthenticationProvider.GetInstance(cca, scopes.ToArray(), userName, userPassword, config.TenantId, config.Password, config.AppAuth);
        }

        private static GraphServiceClient GetAuthenticatedGraphClient(OneDriveCrawlerCrawlJobData config, string userName, SecureString userPassword)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            WebRequest.DefaultWebProxy = new WebProxy("dk.proxy.mid.dom");

            var authenticationProvider = CreateAuthorizationProvider(config, userName, userPassword);
            var graphClient = new GraphServiceClient(authenticationProvider);
            return graphClient;
        }

        public AccountInformation GetAccountInformation()
        {
            //TODO - return some unique information about the remote data source
            // that uniquely identifies the account
            return new AccountInformation("", "");
        }
    }
}
