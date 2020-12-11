using System;
using System.Linq;
using System.Collections.Generic;
using CluedIn.Core.Crawling;
using CluedIn.Crawling.OneDrive.Core;
using CluedIn.Crawling.OneDrive.Core.Models;
using CluedIn.Crawling.OneDrive.Infrastructure.Factories;
using CluedIn.SaxoBank.Common;

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

            if (onedrivecrawlJobData.LastCrawlFinishTime != default(DateTimeOffset))
                onedrivecrawlJobData.LastCrawlFinishTime = onedrivecrawlJobData.LastCrawlFinishTime.AddHours(-3);

            foreach (var user in client.GetUsers())
            {
                //yield return new CluedInUser(user);
                foreach (var drive in client.GetDrives(user))
                {
                    //yield return new CluedInDrive(drive);
                    foreach (var item in client.GetDriveItems(drive))
                    {
                        if (item.CreatedDateTime > onedrivecrawlJobData.LastCrawlFinishTime || item.LastModifiedDateTime > onedrivecrawlJobData.LastCrawlFinishTime)
                        {
                            var extension = "." + item.Name.Split('.').LastOrDefault();
                            if (extension != null && SaxoBankCommonConstants.Extensions.Any(sup => sup.ToLowerInvariant() == extension.ToLowerInvariant()))
                            {
                                if (item.File != null)
                                    yield return new CluedInDriveItem(item, drive);
                            }
                        }
                    }
                }
            }
        }
    }
}
