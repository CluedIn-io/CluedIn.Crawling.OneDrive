using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.Crawling.OneDrive.Vocabularies
{
    public class BaseItemVocabulary : EntityVocabulary
    {
        public BaseItemVocabulary()
        {
            VocabularyName = "OneDrive BaseItem";
            KeyPrefix = "onedrive.baseItem";
            KeySeparator = ".";
            Grouping = EntityType.Unknown;

            AddGroup("OneDrive BaseItem", group =>
            {
                CreatedBy = group.Add(new VocabularyKey("CreatedBy", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                CreatedDateTime = group.Add(new VocabularyKey("CreatedDateTime", VocabularyKeyDataType.DateTime, VocabularyKeyVisibility.Visible));
                Description = group.Add(new VocabularyKey("Description", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                ETag = group.Add(new VocabularyKey("ETag", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                LastModifiedBy = group.Add(new VocabularyKey("LastModifiedBy", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                LastModifiedDateTime = group.Add(new VocabularyKey("LastModifiedDateTime", VocabularyKeyDataType.DateTime, VocabularyKeyVisibility.Visible));
                Name = group.Add(new VocabularyKey("Name", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                ParentReference = group.Add(new VocabularyKey("ParentReference", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                WebUrl = group.Add(new VocabularyKey("WebUrl", VocabularyKeyDataType.Uri, VocabularyKeyVisibility.Visible));
                CreatedByUser = group.Add(new VocabularyKey("CreatedByUser", VocabularyKeyDataType.PersonName, VocabularyKeyVisibility.Visible));
                LastModifiedByUser = group.Add(new VocabularyKey("LastModifiedByUser", VocabularyKeyDataType.PersonName, VocabularyKeyVisibility.Visible));
            });
        }

        public VocabularyKey CreatedBy { get; private set; }

        public VocabularyKey CreatedDateTime { get; private set; }

        public VocabularyKey Description { get; private set; }

        public VocabularyKey ETag { get; private set; }

        public VocabularyKey LastModifiedBy { get; private set; }

        public VocabularyKey LastModifiedDateTime { get; private set; }

        public VocabularyKey Name { get; private set; }

        public VocabularyKey ParentReference { get; private set; }

        public VocabularyKey WebUrl { get; private set; }

        public VocabularyKey CreatedByUser { get; private set; }

        public VocabularyKey LastModifiedByUser { get; private set; }

    }
}
