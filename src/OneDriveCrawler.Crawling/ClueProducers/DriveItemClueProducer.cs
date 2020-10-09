using System;
using CluedIn.Core.Data;
using CluedIn.Crawling.Factories;
using CluedIn.Crawling.Helpers;
using CluedIn.Crawling.OneDriveCrawler.Vocabularies;
using CluedIn.Crawling.OneDriveCrawler.Core.Models;
using Castle.Core.Internal;

namespace CluedIn.Crawling.OneDriveCrawler.ClueProducers
{
    public class DriveItemClueProducer : BaseClueProducerDistinctIdentifyable<DriveItem>
    {
        private readonly IClueFactory factory;

        public DriveItemClueProducer(IClueFactory factory)
        {
            this.factory = factory;
        }

        protected override Clue MakeClueImpl(DriveItem input, Guid accountId)
        {
            var clue = factory.Create(EntityType.Files,input.Audio.ToString(), accountId);

            var driveitemVocabulary = new DriveItemVocabulary();

            var data = clue.Data.EntityData;

            string name = input.Id;
            if(!input.WebUrl.IsNullOrEmpty())
            {
                name += "|" + input.WebUrl;
            }
            data.Name = name;

            if (input.CreatedBy != null)
            {
                factory.CreateIncomingEntityReference(clue, EntityType.Person, EntityEdgeType.CreatedBy, input.CreatedBy, input.CreatedBy);
                data.Properties[driveitemVocabulary.CreatedBy] = input.CreatedBy.PrintIfAvailable();
            }

            data.Properties[driveitemVocabulary.Audio] = input.Audio.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Content] = input.Content.PrintIfAvailable();
            data.Properties[driveitemVocabulary.CreatedBy] = input.CreatedBy.PrintIfAvailable();
            data.Properties[driveitemVocabulary.CreatedDateTime] = input.CreatedDateTime.PrintIfAvailable();
            data.Properties[driveitemVocabulary.CTag] = input.CTag.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Deleted] = input.Deleted.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Description] = input.Description.PrintIfAvailable();
            data.Properties[driveitemVocabulary.ETag] = input.ETag.PrintIfAvailable();
            data.Properties[driveitemVocabulary.File] = input.File.PrintIfAvailable();
            data.Properties[driveitemVocabulary.FileSystemInfo] = input.FileSystemInfo.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Folder] = input.Folder.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Id] = input.Id.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Image] = input.Image.PrintIfAvailable();
            data.Properties[driveitemVocabulary.LastModifiedBy] = input.LastModifiedBy.PrintIfAvailable();
            data.Properties[driveitemVocabulary.LastModifiedDateTime] = input.LastModifiedDateTime.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Location] = input.Location.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Name] = input.Name.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Package] = input.Package.PrintIfAvailable();
            data.Properties[driveitemVocabulary.ParentReference] = input.ParentReference.PrintIfAvailable();
            data.Properties[driveitemVocabulary.PendingOperations] = input.PendingOperations.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Photo] = input.Photo.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Publication] = input.Publication.PrintIfAvailable();
            data.Properties[driveitemVocabulary.RemoteItem] = input.RemoteItem.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Root] = input.Root.PrintIfAvailable();
            data.Properties[driveitemVocabulary.SearchResult] = input.SearchResult.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Shared] = input.Shared.PrintIfAvailable();
            data.Properties[driveitemVocabulary.SharepointIds] = input.SharepointIds.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Size] = input.Size.PrintIfAvailable();
            data.Properties[driveitemVocabulary.SpecialFolder] = input.SpecialFolder.PrintIfAvailable();
            data.Properties[driveitemVocabulary.Video] = input.Video.PrintIfAvailable();
            data.Properties[driveitemVocabulary.WebDavUrl] = input.WebDavUrl.PrintIfAvailable();
            data.Properties[driveitemVocabulary.WebUrl] = input.WebUrl.PrintIfAvailable();

            return clue;
        }
    }
}
