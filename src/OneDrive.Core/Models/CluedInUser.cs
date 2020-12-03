using Microsoft.Graph;

namespace CluedIn.Crawling.OneDrive.Core.Models
{
    public class CluedInUser
    {
        public CluedInUser(User item)
        {
            this.User = item;
        }
        public User User { get; set; }
    }
}
