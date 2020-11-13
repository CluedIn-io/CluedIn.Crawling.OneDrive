using CluedIn.Core;
using CluedIn.Crawling.OneDrive.Core;

using ComponentHost;

namespace CluedIn.Crawling.OneDrive
{
    [Component(OneDriveConstants.CrawlerComponentName, "Crawlers", ComponentType.Service, Components.Server, Components.ContentExtractors, Isolation = ComponentIsolation.NotIsolated)]
    public class OneDriveCrawlerComponent : CrawlerComponentBase
    {
        public OneDriveCrawlerComponent([NotNull] ComponentInfo componentInfo)
            : base(componentInfo)
        {
        }
    }
}

