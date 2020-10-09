using CluedIn.Core.Webhooks;
using CluedIn.Crawling.OneDriveCrawler.Core;

namespace CluedIn.Provider.OneDriveCrawler.WebHooks
{
    public class Name_WebhookPreValidator : BaseWebhookPrevalidator
    {
        public Name_WebhookPreValidator()
            : base(OneDriveCrawlerConstants.ProviderId, OneDriveCrawlerConstants.ProviderName)
        {
        }
    }
}
