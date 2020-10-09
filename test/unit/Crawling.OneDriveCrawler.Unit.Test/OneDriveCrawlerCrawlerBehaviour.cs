using CluedIn.Core.Crawling;
using CluedIn.Crawling;
using CluedIn.Crawling.OneDriveCrawler;
using CluedIn.Crawling.OneDriveCrawler.Infrastructure.Factories;
using Moq;
using Should;
using Xunit;

namespace Crawling.OneDriveCrawler.Unit.Test
{
    public class OneDriveCrawlerCrawlerBehaviour
    {
        private readonly ICrawlerDataGenerator _sut;

        public OneDriveCrawlerCrawlerBehaviour()
        {
            var nameClientFactory = new Mock<IOneDriveCrawlerClientFactory>();

            _sut = new OneDriveCrawlerCrawler(nameClientFactory.Object);
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
