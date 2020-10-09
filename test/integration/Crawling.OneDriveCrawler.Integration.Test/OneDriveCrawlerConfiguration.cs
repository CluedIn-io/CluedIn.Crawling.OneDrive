using System.Collections.Generic;
using CluedIn.Crawling.OneDriveCrawler.Core;

namespace CluedIn.Crawling.OneDriveCrawler.Integration.Test
{
  public static class OneDriveCrawlerConfiguration
  {
    public static Dictionary<string, object> Create()
    {
      return new Dictionary<string, object>
            {
                { OneDriveCrawlerConstants.KeyName.UserName, "mts@cluedin.com" },
                { OneDriveCrawlerConstants.KeyName.Password, "pass" },
                { OneDriveCrawlerConstants.KeyName.TenantId, "f5ae2861-b3fc-449d-a9e7-49c14d011ac0" },
                { OneDriveCrawlerConstants.KeyName.ApplicationId, "0333d932-8824-4ff8-ae2b-86d0c4d53177" }
            };
    }
  }
}
