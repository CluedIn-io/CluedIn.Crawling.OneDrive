using System.Collections.Generic;

using CluedIn.Core.Crawling;
using CluedIn.Crawling.OneDriveCrawler.Core;
using CluedIn.Crawling.OneDriveCrawler.Core.Models;
using CluedIn.Crawling.OneDriveCrawler.Infrastructure.Factories;

namespace CluedIn.Crawling.OneDriveCrawler
{
    public class OneDriveCrawlerCrawler : ICrawlerDataGenerator
    {
        private readonly IOneDriveCrawlerClientFactory clientFactory;
        public OneDriveCrawlerCrawler(IOneDriveCrawlerClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public IEnumerable<object> GetData(CrawlJobData jobData)
        {
            if (!(jobData is OneDriveCrawlerCrawlJobData onedrivecrawlercrawlJobData))
            {
                yield break;
            }

            var client = clientFactory.CreateNew(onedrivecrawlercrawlJobData);

            foreach (var item in client.GetDriveItems())
                yield return new DriveItem(item);
        }       
    }
}
