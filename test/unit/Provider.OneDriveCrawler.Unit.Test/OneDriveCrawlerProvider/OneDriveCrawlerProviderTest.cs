using Castle.Windsor;
using CluedIn.Core;
using CluedIn.Core.Providers;
using CluedIn.Crawling.OneDriveCrawler.Infrastructure.Factories;
using Moq;

namespace CluedIn.Provider.OneDriveCrawler.Unit.Test.OneDriveCrawlerProvider
{
    public abstract class OneDriveCrawlerProviderTest
    {
        protected readonly ProviderBase Sut;

        protected Mock<IOneDriveCrawlerClientFactory> NameClientFactory;
        protected Mock<IWindsorContainer> Container;

        protected OneDriveCrawlerProviderTest()
        {
            Container = new Mock<IWindsorContainer>();
            NameClientFactory = new Mock<IOneDriveCrawlerClientFactory>();
            var applicationContext = new ApplicationContext(Container.Object);
            Sut = new OneDriveCrawler.OneDriveCrawlerProvider(applicationContext, NameClientFactory.Object);
        }
    }
}
