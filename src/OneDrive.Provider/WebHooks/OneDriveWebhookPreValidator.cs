using CluedIn.Core.Webhooks;
using CluedIn.Crawling.OneDrive.Core;

namespace CluedIn.Provider.OneDrive.WebHooks
{
    public class Name_WebhookPreValidator : BaseWebhookPrevalidator
    {
        public Name_WebhookPreValidator()
            : base(OneDriveConstants.ProviderId, OneDriveConstants.ProviderName)
        {
        }
    }
}
