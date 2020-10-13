using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CluedIn.Core;
using Newtonsoft.Json;

namespace CluedIn.Crawling.OneDriveCrawler.Core.Models
{
    public class DriveItemLocal : Microsoft.Graph.DriveItem, IIdentifiable
    {
        [JsonIgnore]
        object IIdentifiable.Id => Id;
    }
}
