using System;
using System.Net;
using System.Threading.Tasks;
using CluedIn.Core.Logging;
using CluedIn.Core.Providers;
using CluedIn.Crawling.OneDriveCrawler.Core;
using Newtonsoft.Json;
using RestSharp;
//using Microsoft.Extensions.Logging;
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
        private readonly ILogger log;
        private readonly GraphServiceClient client;

        public OneDriveCrawlerClient(ILogger log, OneDriveCrawlerCrawlJobData onedrivecrawlerCrawlJobData) // TODO: pass on any extra dependencies
        {
            if (onedrivecrawlerCrawlJobData == null)
            {
                throw new ArgumentNullException(nameof(onedrivecrawlerCrawlJobData));
            }


            this.log = log ?? throw new ArgumentNullException(nameof(log));

            var config = LoadAppSettings(onedrivecrawlerCrawlJobData);
            SecureString pass = new NetworkCredential("", onedrivecrawlerCrawlJobData.Password).SecurePassword;

            client = GetAuthenticatedGraphClient(onedrivecrawlerCrawlJobData,
                onedrivecrawlerCrawlJobData.UserName,
                pass);
        }

        public IEnumerable<Microsoft.Graph.DriveItem> GetDriveItems()
        {
            var request = client.Me.Drive.Root.Children.Request();

            var results = request.GetAsync().Result;
            foreach (var item in results)
            {
                // checking if an item is of any other type taht we would consider
                if (item.Folder == null && item.Photo == null && item == null)
                {
                    yield return item;
                }
            }
        }

        private static IConfigurationRoot LoadAppSettings(OneDriveCrawlerCrawlJobData data)
        {
            try
            {
                var config = new ConfigurationBuilder().Build();

                //if (string.IsNullOrEmpty(config["applicationId"]) ||
                //    string.IsNullOrEmpty(config["tenantId"]))

                // config["applicationId"] = data.

                return config;
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
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
            return MsalAuthenticationProvider.GetInstance(cca, scopes.ToArray(), userName, userPassword);
        }

        private static GraphServiceClient GetAuthenticatedGraphClient(OneDriveCrawlerCrawlJobData config, string userName, SecureString userPassword)
        {
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
