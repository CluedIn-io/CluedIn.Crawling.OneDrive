using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.Crawling.OneDriveCrawler.Vocabularies
{
    public class  DriveItemVocabulary : SimpleVocabulary
    {
        public  DriveItemVocabulary()
        {
            VocabularyName = "OneDriveCrawler DriveItem"; 
            KeyPrefix      = "onedrivecrawler.driveitem"; 
            KeySeparator   = ".";
            Grouping       = EntityType.Files; 

            //TODO: Make sure that any properties mapped into CluedIn Vocabulary are not in the group.
            AddGroup("OneDriveCrawler DriveItem Details", group =>
            {
                Audio = group.Add(new VocabularyKey("Audio", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                Content = group.Add(new VocabularyKey("Content", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Hidden));
                CreatedBy = group.Add(new VocabularyKey("CreatedBy", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                CreatedDateTime = group.Add(new VocabularyKey("CreatedDateTime", VocabularyKeyDataType.DateTime, VocabularyKeyVisibility.Visible));
                CTag = group.Add(new VocabularyKey("CTag", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Hidden));
                Deleted = group.Add(new VocabularyKey("Deleted", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                Description = group.Add(new VocabularyKey("Description", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                ETag = group.Add(new VocabularyKey("ETag", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Hidden));
                File = group.Add(new VocabularyKey("File", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                FileSystemInfo = group.Add(new VocabularyKey("FileSystemInfo", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                Folder = group.Add(new VocabularyKey("Folder", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                Id = group.Add(new VocabularyKey("Id", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                Image = group.Add(new VocabularyKey("Image", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                LastModifiedBy = group.Add(new VocabularyKey("LastModifiedBy", VocabularyKeyDataType.DateTime, VocabularyKeyVisibility.Visible));
                LastModifiedDateTime = group.Add(new VocabularyKey("LastModifiedDateTime", VocabularyKeyDataType.DateTime, VocabularyKeyVisibility.Visible));
                Location = group.Add(new VocabularyKey("Location", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                Name = group.Add(new VocabularyKey("Name", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                Package = group.Add(new VocabularyKey("Package", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                ParentReference = group.Add(new VocabularyKey("ParentReference", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                PendingOperations = group.Add(new VocabularyKey("PendingOperations", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                Photo = group.Add(new VocabularyKey("Photo", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                Publication = group.Add(new VocabularyKey("Publication", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                RemoteItem = group.Add(new VocabularyKey("RemoteItem", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                Root = group.Add(new VocabularyKey("Root", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                SearchResult = group.Add(new VocabularyKey("SearchResult", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                Shared = group.Add(new VocabularyKey("Shared", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                SharepointIds = group.Add(new VocabularyKey("SharepointIds", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                Size = group.Add(new VocabularyKey("Size", VocabularyKeyDataType.Integer, VocabularyKeyVisibility.Visible));
                SpecialFolder = group.Add(new VocabularyKey("SpecialFolder", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                Video = group.Add(new VocabularyKey("Video", VocabularyKeyDataType.Boolean, VocabularyKeyVisibility.Visible));
                WebDavUrl = group.Add(new VocabularyKey("WebDavUrl", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                WebUrl = group.Add(new VocabularyKey("WebUrl", VocabularyKeyDataType.Text, VocabularyKeyVisibility.Visible));
                
            });

            //TODO: If the property is already set in the clueproducer, it doesn't have to be here.
             
            //TODO: Don't forget to map all possible properties into already existing CluedIn Vocabularies.
        
        }


        
        public VocabularyKey Audio { get; private set; }
        
        public VocabularyKey Content { get; private set; }
        
        public VocabularyKey CreatedBy { get; private set; }
        
        public VocabularyKey CreatedDateTime { get; private set; }
        
        public VocabularyKey CTag { get; private set; }
        
        public VocabularyKey Deleted { get; private set; }
        
        public VocabularyKey Description { get; private set; }
        
        public VocabularyKey ETag { get; private set; }
        
        public VocabularyKey File { get; private set; }
        
        public VocabularyKey FileSystemInfo { get; private set; }
        
        public VocabularyKey Folder { get; private set; }
        
        public VocabularyKey Id { get; private set; }
        
        public VocabularyKey Image { get; private set; }
        
        public VocabularyKey LastModifiedBy { get; private set; }
        
        public VocabularyKey LastModifiedDateTime { get; private set; }
        
        public VocabularyKey Location { get; private set; }
        
        public VocabularyKey Name { get; private set; }
        
        public VocabularyKey Package { get; private set; }
        
        public VocabularyKey ParentReference { get; private set; }
        
        public VocabularyKey PendingOperations { get; private set; }
        
        public VocabularyKey Photo { get; private set; }
        
        public VocabularyKey Publication { get; private set; }
        
        public VocabularyKey RemoteItem { get; private set; }
        
        public VocabularyKey Root { get; private set; }
        
        public VocabularyKey SearchResult { get; private set; }
        
        public VocabularyKey Shared { get; private set; }
        
        public VocabularyKey SharepointIds { get; private set; }
        
        public VocabularyKey Size { get; private set; }
        
        public VocabularyKey SpecialFolder { get; private set; }
        
        public VocabularyKey Video { get; private set; }
        
        public VocabularyKey WebDavUrl { get; private set; }
        
        public VocabularyKey WebUrl { get; private set; }
        
    }
}
