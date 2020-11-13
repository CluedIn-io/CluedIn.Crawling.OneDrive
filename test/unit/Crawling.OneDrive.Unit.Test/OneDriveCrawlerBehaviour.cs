using CluedIn.Core.Crawling;
using CluedIn.Crawling;
using CluedIn.Crawling.OneDrive;
using CluedIn.Crawling.OneDrive.Infrastructure.Factories;
using Moq;
using Should;
using Xunit;

namespace Crawling.OneDrive.Unit.Test
{
    public class OneDriveCrawlerBehaviour
    {
        private readonly ICrawlerDataGenerator _sut;

        public OneDriveCrawlerBehaviour()
        {
            var nameClientFactory = new Mock<IOneDriveClientFactory>();

            _sut = new OneDriveCrawler(nameClientFactory.Object);
        }

        [Fact]
        public void GetDataReturnsData()
        {
            var jobData = new CrawlJobData();

            _sut.GetData(jobData)
                .ShouldNotBeNull();
        }
    }
}
