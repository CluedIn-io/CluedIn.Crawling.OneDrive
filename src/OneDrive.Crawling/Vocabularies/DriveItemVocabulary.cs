using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.Crawling.OneDrive.Vocabularies
{
    public class DriveItemVocabulary : BaseItemVocabulary
    {
        public DriveItemVocabulary()
        {
            VocabularyName = "OneDrive DriveItem";
            KeyPrefix = "onedrive.driveItem";
            KeySeparator = ".";
            Grouping = EntityType.Unknown;

            AddGroup("DriveItem", group =>
            {
                WebDavUrl = group.Add(new VocabularyKey("WebDavUrl", VocabularyKeyDataType.Uri, VocabularyKeyVisibility.Visible));
                Size = group.Add(new VocabularyKey("Size", VocabularyKeyDataType.Integer, VocabularyKeyVisibility.Visible));
                CTag = group.Add(new VocabularyKey("CTag", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                DriveOwnerUserEmail = group.Add(new VocabularyKey("OwnerUserEmail", VocabularyKeyDataType.Email, VocabularyKeyVisibility.Hidden));
                DriveOwnerUserId = group.Add(new VocabularyKey("DriveOwnerUserId", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                Path = group.Add(new VocabularyKey("Path", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Hidden));
                DriveName = group.Add(new VocabularyKey("DriveName", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Hidden));
            });
        }

        public VocabularyKey WebDavUrl { get; private set; }

        public VocabularyKey Size { get; private set; }

        public VocabularyKey CTag { get; private set; }

        public VocabularyKey DriveOwnerUserEmail { get; private set; }

        public VocabularyKey Path { get; private set; }

        public VocabularyKey DriveName { get; private set; }

        public VocabularyKey DriveOwnerUserId { get; private set; }

    }
}
