using System;
using CluedIn.Core;
using Newtonsoft.Json;

namespace CluedIn.Crawling.OneDriveCrawler.Core.Models
{
    public class DriveItem : IIdentifiable
    {
        [JsonIgnore]
        object IIdentifiable.Id => Id;

        [JsonProperty("audio")]
        public bool Audio {get; set;}

        [JsonProperty("content")]
        public string Content {get; set;}

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

        [JsonProperty("fileSystemInfo")]
        public string FileSystemInfo {get; set;}

        [JsonProperty("folder")]
        public bool Folder {get; set;}

        [JsonProperty("id")]
        public string Id {get; set;}

        [JsonProperty("image")]
        public bool Image {get; set;}

        [JsonProperty("lastModifiedBy")]
        public DateTime LastModifiedBy {get; set;}

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

        [JsonProperty("pendingOperations")]
        public string PendingOperations {get; set;}

        [JsonProperty("photo")]
        public bool Photo {get; set;}

        [JsonProperty("publication")]
        public bool Publication {get; set;}

        [JsonProperty("remoteItem")]
        public bool RemoteItem {get; set;}

        [JsonProperty("root")]
        public string Root {get; set;}

        [JsonProperty("searchResult")]
        public string SearchResult {get; set;}

        [JsonProperty("shared")]
        public bool Shared {get; set;}

        [JsonProperty("sharepointIds")]
        public string SharepointIds {get; set;}

        [JsonProperty("size")]
        public int Size {get; set;}

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
