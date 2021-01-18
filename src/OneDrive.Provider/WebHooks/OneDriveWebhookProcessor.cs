using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

using CluedIn.Core;
using CluedIn.Core.Agent.Jobs;
using CluedIn.Core.Configuration;
using CluedIn.Core.Data;
using CluedIn.Core.DataStore;
using CluedIn.Core.Messages.Processing;
using CluedIn.Core.Providers;
using CluedIn.Core.Webhooks;
using CluedIn.Crawling;
using CluedIn.Crawling.OneDrive.Core;

namespace CluedIn.Provider.OneDrive.WebHooks
{
    public class OneDriveWebhookProcessor : BaseWebhookProcessor
    {
        public OneDriveWebhookProcessor(ApplicationContext appContext)
            : base(appContext)
        {
        }

        public override bool Accept(IWebhookDefinition webhookDefinition)
        {
            return webhookDefinition.ProviderId == OneDriveConstants.ProviderId || base.Accept(webhookDefinition);
        }

        public override IEnumerable<Clue> DoProcess(ExecutionContext context, WebhookDataCommand command)
        {
            try
            {
                if (ConfigurationManager.AppSettings.GetFlag("Feature.Webhooks.Log.Posts", false))
                    context.Log.Debug(() => command.HttpPostData);

                var configurationDataStore = context.ApplicationContext.Container.Resolve<IConfigurationRepository>();
                if (command.WebhookDefinition.ProviderDefinitionId != null)
                {
                    var providerDefinition = context.Organization.Providers.GetProviderDefinition(context, command.WebhookDefinition.ProviderDefinitionId.Value);
                    var jobDataCheck       = context.ApplicationContext.Container.ResolveAll<IProvider>().FirstOrDefault(providerInstance => providerDefinition != null && providerInstance.Id == providerDefinition.ProviderId);
                    var configStoreData    = configurationDataStore.GetConfigurationById(context, command.WebhookDefinition.ProviderDefinitionId.Value);

                    // If you have stopped the provider then don't process the webhooks
                    if (providerDefinition?.WebHooks != null)
                        if (providerDefinition.WebHooks == false || providerDefinition.IsEnabled == false)
                            return new List<Clue>();

                    if (jobDataCheck != null)
                    {
                        var crawlJobData = new OneDriveCrawlJobData();

                        var clues = new List<Clue>();


                        throw new NotImplementedException($"TODO: Implement this to populate '{clues.GetType()}'");
                    }
                }
            }
            catch (Exception exception)
            {
                context.Log.Error(new { command.HttpHeaders, command.HttpQueryString, command.HttpPostData, command.WebhookDefinitionId }, () => "Could not process web hook message", exception);
            }

            return new List<Clue>();
        }
    }
}
