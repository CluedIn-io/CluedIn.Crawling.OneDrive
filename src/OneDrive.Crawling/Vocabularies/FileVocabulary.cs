using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.Crawling.OneDrive.Vocabularies
{
    public class FileVocabulary : DriveItemVocabulary
    {
        public FileVocabulary()
        {
            VocabularyName = "OneDrive File";
            KeyPrefix = "onedrive.file";
            KeySeparator = ".";
            Grouping = EntityType.Infrastructure.DirectoryItem;

            AddGroup("OneDrive File", group =>
            {
                Hashes = group.Add(new VocabularyKey("Hashes", VocabularyKeyDataType.Json, VocabularyKeyVisibility.Visible));
                MimeType = group.Add(new VocabularyKey("MimeType", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                ProcessingMetadata = group.Add(new VocabularyKey("ProcessingMetadata", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                AdditionalData = group.Add(new VocabularyKey("AdditionalData", VocabularyKeyDataType.Json, VocabularyKeyVisibility.Visible));
                Hash = group.Add(new VocabularyKey("Hash", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
            });
        }

        public VocabularyKey Hashes { get; private set; }

        public VocabularyKey MimeType { get; private set; }

        public VocabularyKey ProcessingMetadata { get; private set; }

        public VocabularyKey AdditionalData { get; private set; }

        public VocabularyKey Hash { get; private set; }

    }
}
