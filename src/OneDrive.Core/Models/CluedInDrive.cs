using Microsoft.Graph;

namespace CluedIn.Crawling.OneDrive.Core.Models
{
    public class CluedInDrive
    {
        public CluedInDrive(Drive item)
        {
            this.Drive = item;
        }
        public Drive Drive { get; set; }
    }
}
