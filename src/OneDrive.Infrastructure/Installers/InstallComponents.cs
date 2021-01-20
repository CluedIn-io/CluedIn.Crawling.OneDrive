using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Facilities.TypedFactory;

using CluedIn.Core;
using CluedIn.Crawling.OneDrive.Infrastructure.Factories;
using RestSharp;
using CluedIn.ContentExtraction.Aspose.Installers;
using CluedIn.ContentExtraction.Aspose.ContentExtraction.AsposeExtractors;
using CluedIn.Crawling.ContentExtraction;

namespace CluedIn.Crawling.OneDrive.Infrastructure.Installers
{
    public class InstallComponents : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .AddFacilityIfNotExists<TypedFactoryFacility>()
                .Register(Component.For<IOneDriveClientFactory>().AsFactory().OnlyNewServices())
                .Register(Component.For<OneDriveClient>().OnlyNewServices().LifestyleTransient());

            if (!container.Kernel.HasComponent(typeof(IRestClient)) && !container.Kernel.HasComponent(typeof(RestClient)))
                container.Register(Component.For<IRestClient, RestClient>());

            container.Register(Component.For<IContentExtractor>().ImplementedBy(typeof(AsposeContentExtractor)).LifestyleSingleton());
        }
    }
}
