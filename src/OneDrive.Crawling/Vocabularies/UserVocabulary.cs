/*using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.Crawling.OneDrive.Vocabularies
{
    public class UserVocabulary : EntityVocabulary
    {
        public UserVocabulary()
        {
            VocabularyName = "OneDrive User";
            KeyPrefix = "onedrive.user";
            KeySeparator = ".";
            Grouping = EntityType.Infrastructure.User;

            AddGroup("User", group =>
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

        public VocabularyKey Manager { get; private set; }

        public VocabularyKey Outlook { get; private set; }

        public VocabularyKey Skills { get; private set; }

        public VocabularyKey Responsibilities { get; private set; }

        public VocabularyKey PreferredName { get; private set; }

        public VocabularyKey PastProjects { get; private set; }

        public VocabularyKey MySite { get; private set; }

        public VocabularyKey Interests { get; private set; }

        public VocabularyKey HireDate { get; private set; }

        public VocabularyKey Birthday { get; private set; }

        public VocabularyKey AboutMe { get; private set; }

        public VocabularyKey DeviceEnrollmentLimit { get; private set; }

        public VocabularyKey Schools { get; private set; }

        public VocabularyKey UserType { get; private set; }
    }
}
*/
