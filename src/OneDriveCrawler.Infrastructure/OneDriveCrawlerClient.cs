using System;
using System.Net;
using System.Threading.Tasks;
using CluedIn.Core.Providers;
using CluedIn.Crawling.OneDriveCrawler.Core;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.Extensions.Logging;

namespace CluedIn.Crawling.OneDriveCrawler.Infrastructure
{
    // TODO - This class should act as a client to retrieve the data to be crawled.
    // It should provide the appropriate methods to get the data
    // according to the type of data source (e.g. for AD, GetUsers, GetRoles, etc.)
    // It can receive a IRestClient as a dependency to talk to a RestAPI endpoint.
    // This class should not contain crawling logic (i.e. in which order things are retrieved)
    public class OneDriveCrawlerClient
    {
        private const string BaseUri = "http://sample.com";

        private readonly ILogger<OneDriveCrawlerClient> log;

        private readonly IRestClient client;

        public OneDriveCrawlerClient(ILogger<OneDriveCrawlerClient> log, OneDriveCrawlerCrawlJobData onedrivecrawlerCrawlJobData, IRestClient client) // TODO: pass on any extra dependencies
        {
            if (onedrivecrawlerCrawlJobData == null)
            {
                throw new ArgumentNullException(nameof(onedrivecrawlerCrawlJobData));
            }

            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.client = client ?? throw new ArgumentNullException(nameof(client));

            // TODO use info from onedrivecrawlerCrawlJobData to instantiate the connection
            client.BaseUrl = new Uri(BaseUri);
            client.AddDefaultParameter("api_key", onedrivecrawlerCrawlJobData.ApiKey, ParameterType.QueryString);
        }

        private async Task<T> GetAsync<T>(string url)
        {
            var request = new RestRequest(url, Method.GET);

            var response = await client.ExecuteAsync(request, request.Method);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var diagnosticMessage = $"Request to {client.BaseUrl}{url} failed, response {response.ErrorMessage} ({response.StatusCode})";
                log.LogError(diagnosticMessage);
                throw new InvalidOperationException($"Communication to jsonplaceholder unavailable. {diagnosticMessage}");
            }

            var data = JsonConvert.DeserializeObject<T>(response.Content);

            return data;
        }

        public AccountInformation GetAccountInformation()
        {
            //TODO - return some unique information about the remote data source
            // that uniquely identifies the account
            return new AccountInformation("", "");
        }
    }
}
