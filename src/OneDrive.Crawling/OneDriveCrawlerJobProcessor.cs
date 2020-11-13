using CluedIn.Crawling.OneDrive.Core;

namespace CluedIn.Crawling.OneDrive
{
    public class OneDriveCrawlerJobProcessor : GenericCrawlerTemplateJobProcessor<OneDriveCrawlJobData>
    {
        public OneDriveCrawlerJobProcessor(OneDriveCrawlerComponent component) : base(component)
        {
        }
    }
}
