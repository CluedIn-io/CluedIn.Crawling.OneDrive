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
                { OneDriveCrawlerConstants.KeyName.ApiKey, "demo" }
            };
    }
  }
}
