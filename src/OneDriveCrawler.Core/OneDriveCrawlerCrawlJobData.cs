using CluedIn.Core.Crawling;

namespace CluedIn.Crawling.OneDriveCrawler.Core
{
    public class OneDriveCrawlerCrawlJobData : CrawlJobData
    {
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TenantId { get; set; }
        public string ApplicationId { get; set; }
    }
}
