using System.Collections.Generic;
using CluedIn.Crawling.OneDrive.Core;

namespace CluedIn.Crawling.OneDrive.Integration.Test
{
  public static class OneDriveConfiguration
  {
    public static Dictionary<string, object> Create()
    {
      return new Dictionary<string, object>
            {
                { OneDriveConstants.KeyName.ApiKey, "demo" }
            };
    }
  }
}
