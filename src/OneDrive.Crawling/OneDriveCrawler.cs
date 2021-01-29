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

            if (!onedrivecrawlJobData.FullCrawl)
            {
                if (onedrivecrawlJobData.LastCrawlFinishTime != default(DateTimeOffset))
                    onedrivecrawlJobData.LastCrawlFinishTime = onedrivecrawlJobData.LastCrawlFinishTime.AddDays(-1);
                else
                {
                    onedrivecrawlJobData.LastCrawlFinishTime = DateTimeOffset.Now.AddDays(-8);
                }
            }

            foreach (var user in client.GetUsers().SkipWhile(u => {
                if (string.IsNullOrEmpty(onedrivecrawlJobData.SkipUntilUser))
                    return false;

                return u.DisplayName != onedrivecrawlJobData.SkipUntilUser;
                }))
            {
                //yield return new CluedInUser(user);
                foreach (var drive in client.GetDrives(user))
                {
                    //yield return new CluedInDrive(drive);
                    foreach (var item in client.GetDriveItems(drive).Where(item =>
                    {
                        if ((item.CreatedDateTime > onedrivecrawlJobData.LastCrawlFinishTime || item.LastModifiedDateTime > onedrivecrawlJobData.LastCrawlFinishTime) || onedrivecrawlJobData.FullCrawl)
                        {
                            var extension = "." + item.Name.Split('.').LastOrDefault();
                            if (extension != null && SaxoBankCommonConstants.Extensions.Any(sup => sup.ToLowerInvariant() == extension.ToLowerInvariant()))
                            {
                                if (item.File != null)
                                    return true;
                            }
                        }
                        return false;

                    }))
                    {
                        yield return new CluedInDriveItem(item, drive);

                    }
                }
            }
        }
    }
}
