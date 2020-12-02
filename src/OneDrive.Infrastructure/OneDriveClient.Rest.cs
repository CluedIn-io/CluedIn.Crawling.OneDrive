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
    public partial class OneDriveClient
    {
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
    }
}
