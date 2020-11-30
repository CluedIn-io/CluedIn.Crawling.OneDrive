using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;

using CluedIn.Core;
using CluedIn.Crawling.OneDrive.Infrastructure.Factories;
using RestSharp;
using CluedIn.Crawling.OneDrive.Infrastructure;
using CluedIn.Crawling.OneDrive.Core;

namespace CluedIn.Provider.Installers
{
    public class InstallComponents : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .AddFacilityIfNotExists<TypedFactoryFacility>()
                .Register(Component.For<IOneDriveClientFactory>().AsFactory().OnlyNewServices())
                .Register(Component.For<OneDriveClient>().OnlyNewServices().LifestyleTransient());

            if (!container.Kernel.HasComponent(typeof(OneDriveCrawlJobData)))
                container.Register(Component.For<OneDriveCrawlJobData>());
        }
    }
}
