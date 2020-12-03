using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.Crawling.OneDrive.Vocabularies
{
    public class DriveVocabulary : BaseItemVocabulary
    {
        public DriveVocabulary()
        {
            VocabularyName = "OneDrive Drive";
            KeyPrefix = "onedrive.drive";
            KeySeparator = ".";
            Grouping = EntityType.Infrastructure.Folder;

            AddGroup("Drive", group =>
            {
                DriveType = group.Add(new VocabularyKey("DriveType", VocabularyKeyDataType.Json, VocabularyKeyVisibility.Visible));
            });
        }

        public VocabularyKey DriveType { get; private set; }


    }
}
