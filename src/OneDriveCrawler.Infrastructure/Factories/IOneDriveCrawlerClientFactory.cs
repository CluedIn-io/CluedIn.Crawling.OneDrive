using CluedIn.Crawling.OneDriveCrawler.Core;

namespace CluedIn.Crawling.OneDriveCrawler.Infrastructure.Factories
{
    public interface IOneDriveCrawlerClientFactory
    {
        OneDriveCrawlerClient CreateNew(OneDriveCrawlerCrawlJobData onedrivecrawlerCrawlJobData);
    }
}
