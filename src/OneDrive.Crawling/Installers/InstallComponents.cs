using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CluedIn.ContentExtraction.Aspose.Installers;
using CluedIn.Core;

namespace CluedIn.Crawling.OneDrive.Installers
{
    public class InstallComponents : IWindsorInstaller
    {
        public void Install([NotNull] IWindsorContainer container, [NotNull] IConfigurationStore store)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (store == null) throw new ArgumentNullException(nameof(store));

            // TODO Add further dependencies to the container here ...

            // Temporary register Aspose content extractor in container
            container.Install(new AsposeContentExtractionInstaller());
        }
    }
}
