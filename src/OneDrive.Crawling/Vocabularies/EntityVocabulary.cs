using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.Crawling.OneDrive.Vocabularies
{
    public class EntityVocabulary : SimpleVocabulary
    {
        public EntityVocabulary()
        {
            VocabularyName = "OneDrive Entity";
            KeyPrefix = "onedrive.entity";
            KeySeparator = ".";
            Grouping = EntityType.Unknown;

            AddGroup("OneDrive Entity", group =>
            {
                Id = group.Add(new VocabularyKey("Id", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                ODataType = group.Add(new VocabularyKey("ODataType", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
            });
        }

        public VocabularyKey Id { get; private set; }

        public VocabularyKey ODataType { get; private set; }

    }
}
