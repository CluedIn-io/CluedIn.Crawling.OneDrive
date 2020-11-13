using CluedIn.Crawling.OneDrive.Core;

namespace CluedIn.Crawling.OneDrive.Infrastructure.Factories
{
    public interface IOneDriveClientFactory
    {
        OneDriveClient CreateNew(OneDriveCrawlJobData onedriveCrawlJobData);
    }
}
