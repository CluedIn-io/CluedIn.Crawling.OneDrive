using System.Collections.Generic;
using CluedIn.Core.Crawling;
using CluedIn.Crawling.OneDrive.Core;
using CluedIn.Crawling.OneDrive.Core.Models;
using CluedIn.Crawling.OneDrive.Infrastructure.Factories;

namespace CluedIn.Crawling.OneDrive
{
    public class OneDriveCrawler : ICrawlerDataGenerator
    {
        private readonly IOneDriveClientFactory clientFactory;
        public OneDriveCrawler(IOneDriveClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public IEnumerable<object> GetData(CrawlJobData jobData)
        {
            if (!(jobData is OneDriveCrawlJobData onedrivecrawlJobData))
            {
                yield break;
            }

            var client = clientFactory.CreateNew(onedrivecrawlJobData);

            foreach (var page in client.GetDriveItems())
                foreach (var item in page)
                    yield return item as CluedInDriveItem;
        }
    } 
}
