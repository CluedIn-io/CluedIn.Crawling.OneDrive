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

            AddGroup("OneDrive DriveItem", group =>
            {
                WebDavUrl = group.Add(new VocabularyKey("WebDavUrl", VocabularyKeyDataType.Uri, VocabularyKeyVisibility.Visible));
                Size = group.Add(new VocabularyKey("Size", VocabularyKeyDataType.Integer, VocabularyKeyVisibility.Visible));
                CTag = group.Add(new VocabularyKey("CTag", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
            });
        }

        public VocabularyKey WebDavUrl { get; private set; }

        public VocabularyKey Size { get; private set; }

        public VocabularyKey CTag { get; private set; }

    }
}
