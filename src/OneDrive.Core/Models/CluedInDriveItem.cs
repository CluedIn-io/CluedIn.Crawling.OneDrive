using Microsoft.Graph;

namespace CluedIn.Crawling.OneDrive.Core.Models
{
    public class CluedInDriveItem
    {
        public CluedInDriveItem(DriveItem item)
        {
            this.DriveItem = item;
        }
        public DriveItem DriveItem { get; set; }
    }
}
