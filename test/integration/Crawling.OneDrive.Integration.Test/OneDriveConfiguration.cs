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
            StreamReader reader = File.OpenText(@"..\..\..\OneDriveConfiguration.xml");
            XDocument doc = XDocument.Load(reader);
            var configuration = doc.Descendants().ToDictionary(config => config.Name.LocalName, config => config.Value);
            return new Dictionary<string, object>
            {
                { OneDriveConstants.KeyName.ClientID, configuration[OneDriveConstants.KeyName.ClientID] },
                { OneDriveConstants.KeyName.ClientSecret,configuration[OneDriveConstants.KeyName.ClientSecret]},
                { OneDriveConstants.KeyName.Tenant, configuration[OneDriveConstants.KeyName.Tenant] }
            };
        }
    }
}
