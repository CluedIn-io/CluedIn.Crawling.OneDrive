using CluedIn.Core;
using CluedIn.Crawling.OneDriveCrawler.Core;

using ComponentHost;

namespace CluedIn.Crawling.OneDriveCrawler
{
    [Component(OneDriveCrawlerConstants.CrawlerComponentName, "Crawlers", ComponentType.Service, Components.Server, Components.ContentExtractors, Isolation = ComponentIsolation.NotIsolated)]
    public class OneDriveCrawlerCrawlerComponent : CrawlerComponentBase
    {
        public OneDriveCrawlerCrawlerComponent([NotNull] ComponentInfo componentInfo)
            : base(componentInfo)
        {
        }
    }
}

