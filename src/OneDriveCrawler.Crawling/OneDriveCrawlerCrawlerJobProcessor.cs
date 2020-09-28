using CluedIn.Crawling.OneDriveCrawler.Core;

namespace CluedIn.Crawling.OneDriveCrawler
{
    public class OneDriveCrawlerCrawlerJobProcessor : GenericCrawlerTemplateJobProcessor<OneDriveCrawlerCrawlJobData>
    {
        public OneDriveCrawlerCrawlerJobProcessor(OneDriveCrawlerCrawlerComponent component) : base(component)
        {
        }
    }
}
