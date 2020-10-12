using System;
using CluedIn.Core;
using Newtonsoft.Json;

namespace CluedIn.Crawling.OneDriveCrawler.Core.Models
{
    public class DriveItem :  IIdentifiable
    {
        [JsonIgnore]
        object IIdentifiable.Id => Id;

        public DriveItem(Microsoft.Graph.DriveItem item)
        {
            Id = item.Id;

            // this section defines what type of file is encountered
            Audio = item.Audio != null ? true : false;
            File = item.File != null ? true : false;
            Folder = item.Folder != null ? true : false;
            Image = item.Image != null ? true : false;
            Package = item.Package != null ? true : false;
            Photo = item.Photo != null ? true : false;
            Publication = item.Publication != null ? true : false;
            SpecialFolder = item.SpecialFolder != null ? true : false;
            Video = item.Video != null ? true : false;

            // this section sets up properties of a DriveItem
            CreatedBy = item.CreatedBy?.User.DisplayName;
            CreatedDateTime = item.CreatedDateTime.GetValueOrDefault().DateTime;
            CTag = item.CTag;
            Deleted = item.Deleted?.State;
            Description = item.Description;
            ETag = item.ETag;
            LastAccessedDateTime = item.FileSystemInfo.LastAccessedDateTime.GetValueOrDefault().DateTime;
            LastModifiedDateTime = item.LastModifiedDateTime.GetValueOrDefault().DateTime;
            LastModifiedBy = item.LastModifiedBy?.User.DisplayName;
            Location = item.Location?.ToString();
            Name = item.Name;
            ParentReference = item.ParentReference?.DriveId;
            RemoteItem = item.RemoteItem?.Id;
            Root = item.Root != null ? true : false;
            SearchResult = item.SearchResult?.OnClickTelemetryUrl;
            Shared = item.Shared != null ? true : false;
            SharepointIds = item.SharepointIds?.SiteId;
            Size = item.Size.GetValueOrDefault();
            WebDavUrl = item.WebDavUrl;
            WebUrl = item.WebUrl;
        }

        [JsonProperty("audio")]
        public bool Audio {get; set;}

        //[JsonProperty("content")]
        //public string Content {get; set;}

        [JsonProperty("createdBy")]
        public string CreatedBy {get; set;}

        [JsonProperty("createdDateTime")]
        public DateTime CreatedDateTime {get; set;}

        [JsonProperty("cTag")]
        public string CTag {get; set;}

        [JsonProperty("deleted")]
        public string Deleted {get; set;}

        [JsonProperty("description")]
        public string Description {get; set;}

        [JsonProperty("eTag")]
        public string ETag {get; set;}

        [JsonProperty("file")]
        public bool File {get; set;}

        [JsonProperty("lastAccessedDateTime")]
        public DateTime LastAccessedDateTime { get; set; }

        [JsonProperty("folder")]
        public bool Folder {get; set;}

        [JsonProperty("id")]
        public string Id {get; set;}

        [JsonProperty("image")]
        public bool Image {get; set;}

        [JsonProperty("lastModifiedBy")]
        public string LastModifiedBy {get; set;}

        [JsonProperty("lastModifiedDateTime")]
        public DateTime LastModifiedDateTime {get; set;}

        [JsonProperty("location")]
        public string Location {get; set;}

        [JsonProperty("name")]
        public string Name {get; set;}

        [JsonProperty("package")]
        public bool Package {get; set;}

        [JsonProperty("parentReference")]
        public string ParentReference {get; set;}

        [JsonProperty("photo")]
        public bool Photo {get; set;}

        [JsonProperty("publication")]
        public bool Publication {get; set;}

        [JsonProperty("remoteItem")]
        public string RemoteItem {get; set;}

        [JsonProperty("root")]
        public bool Root {get; set;}

        [JsonProperty("searchResult")]
        public string SearchResult {get; set;}

        [JsonProperty("shared")]
        public bool Shared {get; set;}

        [JsonProperty("sharepointIds")]
        public string SharepointIds {get; set;}

        [JsonProperty("size")]
        public long Size {get; set;}

        [JsonProperty("specialFolder")]
        public bool SpecialFolder {get; set;}

        [JsonProperty("video")]
        public bool Video {get; set;}

        [JsonProperty("webDavUrl")]
        public string WebDavUrl {get; set;}

        [JsonProperty("webUrl")]
        public string WebUrl {get; set;}

    }
}
