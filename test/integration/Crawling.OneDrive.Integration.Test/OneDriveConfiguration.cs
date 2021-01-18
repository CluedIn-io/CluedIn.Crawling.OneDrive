using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CluedIn.Crawling.OneDrive.Core;

namespace CluedIn.Crawling.OneDrive.Integration.Test
{
    public static class OneDriveConfiguration
    {
        public static Dictionary<string, object> Create()
        {
            //StreamReader reader = File.OpenText(@"..\..\..\OneDriveConfiguration.xml");
            //XDocument doc = XDocument.Load(reader);
            //var configuration = doc.Descendants().ToDictionary(config => config.Name.LocalName, config => config.Value);
            return new Dictionary<string, object>
            {
                { OneDriveConstants.KeyName.ClientID, Environment.GetEnvironmentVariable("saxo_sharepoint_username", EnvironmentVariableTarget.Machine) },
                { OneDriveConstants.KeyName.ClientSecret, "saxo_sharepoint_password" },
                { OneDriveConstants.KeyName.Tenant, Environment.GetEnvironmentVariable("saxo_sharepoint_tenant", EnvironmentVariableTarget.Machine) },
                { OneDriveConstants.KeyName.FullCrawl, true },
            };
        }
    }
}
