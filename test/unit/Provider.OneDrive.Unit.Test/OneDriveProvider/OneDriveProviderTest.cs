using Castle.Windsor;
using CluedIn.Core;
using CluedIn.Core.Providers;
using CluedIn.Crawling.OneDrive.Infrastructure.Factories;
using Moq;

namespace CluedIn.Provider.OneDrive.Unit.Test.OneDriveProvider
{
    public abstract class OneDriveProviderTest
    {
        protected readonly ProviderBase Sut;

        protected Mock<IOneDriveClientFactory> NameClientFactory;
        protected Mock<IWindsorContainer> Container;

        protected OneDriveProviderTest()
        {
            Container = new Mock<IWindsorContainer>();
            NameClientFactory = new Mock<IOneDriveClientFactory>();
            var applicationContext = new ApplicationContext(Container.Object);
            Sut = new OneDrive.OneDriveProvider(applicationContext, NameClientFactory.Object);
        }
    }
}
