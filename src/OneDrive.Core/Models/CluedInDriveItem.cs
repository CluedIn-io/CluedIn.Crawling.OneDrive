using Microsoft.Graph;

namespace CluedIn.Crawling.OneDrive.Core.Models
{
    public class CluedInDriveItem
    {
        public CluedInDriveItem(DriveItem item, Drive drive)
        {
            this.DriveItem = item;
            this.Drive = drive;
        }
        public DriveItem DriveItem { get; set; }

        public Drive Drive { get; set; }
    }
}
