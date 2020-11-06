using System.Collections.Generic;
using CluedIn.Core.Crawling;

namespace CluedIn.Crawling.OneDriveCrawler.Core
{
    public class OneDriveCrawlerCrawlJobData : CrawlJobData
    {
        public OneDriveCrawlerCrawlJobData()
        {

        }

        public OneDriveCrawlerCrawlJobData(IDictionary<string, object> configuration)
        {

            if (configuration.ContainsKey(OneDriveCrawlerConstants.KeyName.UserName))
                UserName = configuration[OneDriveCrawlerConstants.KeyName.UserName].ToString();
            if (configuration.ContainsKey(OneDriveCrawlerConstants.KeyName.Password))
                Password = configuration[OneDriveCrawlerConstants.KeyName.Password].ToString();
            if (configuration.ContainsKey(OneDriveCrawlerConstants.KeyName.TenantId))
                TenantId = configuration[OneDriveCrawlerConstants.KeyName.TenantId].ToString();
            if (configuration.ContainsKey(OneDriveCrawlerConstants.KeyName.ApplicationId))
                ApplicationId = configuration[OneDriveCrawlerConstants.KeyName.ApplicationId].ToString();

            AppAuth = GetValue<bool>(configuration, OneDriveCrawlerConstants.KeyName.ApplicationId);

        }
        //public string ApiKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TenantId { get; set; }
        public string ApplicationId { get; set; }
        public bool AppAuth { get; set; }
    }
}
