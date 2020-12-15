using System.Collections.Generic;
using System.Configuration;
using CluedIn.Core;
using CluedIn.Core.Configuration;
using CluedIn.Core.Crawling;

namespace CluedIn.Crawling.OneDrive.Core
{
    public class OneDriveCrawlJobData : CrawlJobData
    {
        public OneDriveCrawlJobData()
        {

        }

        public OneDriveCrawlJobData(IDictionary<string, object> configuration)
        {
            TargetUrl = configuration.GetValue(OneDriveConstants.KeyName.TargetUrl, ConfigurationManager.AppSettings.GetValue(OneDriveConstants.KeyName.TargetUrl, default(string)));
            Tenant = configuration.GetValue(OneDriveConstants.KeyName.Tenant, ConfigurationManager.AppSettings.GetValue(OneDriveConstants.KeyName.Tenant, default(string)));
            ClientID = configuration.GetValue(OneDriveConstants.KeyName.ClientID, ConfigurationManager.AppSettings.GetValue(OneDriveConstants.KeyName.ClientID, default(string)));
            ClientSecret = configuration.GetValue(OneDriveConstants.KeyName.ClientSecret, ConfigurationManager.AppSettings.GetValue(OneDriveConstants.KeyName.ClientSecret, default(string)));
            UseProxy = GetValue<bool>(configuration, OneDriveConstants.KeyName.UseProxy);
        }

        public string ApiKey { get; set; }

        public string TargetUrl { get; set; }

        public string Tenant { get; set; }

        public string ClientID { get; set; }

        public string ClientSecret { get; set; }
        public bool UseProxy { get; set; }
    }
}
