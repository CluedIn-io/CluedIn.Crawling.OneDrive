using System;
using CluedIn.Core.Data;
using CluedIn.Crawling.Helpers;
using CluedIn.Crawling.OneDrive.Core.Models;
using CluedIn.Crawling.OneDrive.Vocabularies;
using Microsoft.Graph;

namespace CluedIn.Crawling.OneDrive.ClueProducers
{
    public partial class DriveItemClueProducer : BaseClueProducer<CluedInDriveItem>
    {
        private void PopulateBasicProperties(Clue clue, BaseItem input)
        {
            var data = clue.Data.EntityData;

            data.Name = input.Name;
            data.Description = input.Description;
            data.CreatedDate = input.CreatedDateTime;
            data.ModifiedDate = input.LastModifiedDateTime;
            data.Uri = new Uri(input.WebUrl);

            if (input.ParentReference != null)
            {
                if (!string.IsNullOrWhiteSpace(input.ParentReference.Id))
                    this.factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.DirectoryItem, EntityEdgeType.Parent, input, input.ParentReference.Id);
            }

            if (input.CreatedByUser != null)
            {
                if (!string.IsNullOrWhiteSpace(input.CreatedByUser.Id))
                    this.factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.User, EntityEdgeType.CreatedBy, input, input.CreatedByUser.Id);
            }

            if (input.LastModifiedByUser != null)
            {
                if (!string.IsNullOrWhiteSpace(input.LastModifiedByUser.Id))
                    this.factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.User, EntityEdgeType.ModifiedBy, input, input.LastModifiedByUser.Id);
            }

            if (input.CreatedBy != null)
            {
                if (input.CreatedBy.User != null)
                    if (!string.IsNullOrWhiteSpace(input.CreatedBy.User.Id))
                        this.factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.User, EntityEdgeType.CreatedBy, input, input.CreatedBy.User.Id);

                if (input.CreatedBy.Application != null)
                    if (!string.IsNullOrWhiteSpace(input.CreatedBy.Application.Id))
                        this.factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.Application, EntityEdgeType.CreatedBy, input, input.CreatedBy.Application.Id);

                if (input.CreatedBy.Device != null)
                    if (!string.IsNullOrWhiteSpace(input.CreatedBy.Device.Id))
                        this.factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure, EntityEdgeType.CreatedBy, input, input.CreatedBy.Device.Id);
            }

            if (input.LastModifiedBy != null)
            {
                if (input.LastModifiedBy.User != null)
                    if (!string.IsNullOrWhiteSpace(input.LastModifiedBy.User.Id))
                        this.factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.User, EntityEdgeType.ModifiedBy, input, input.LastModifiedBy.User.Id);

                if (input.LastModifiedBy.Application != null)
                    if (!string.IsNullOrWhiteSpace(input.LastModifiedBy.Application.Id))
                        this.factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.Application, EntityEdgeType.ModifiedBy, input, input.LastModifiedBy.Application.Id);

                if (input.LastModifiedBy.Device != null)
                    if (!string.IsNullOrWhiteSpace(input.LastModifiedBy.Device.Id))
                        this.factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure, EntityEdgeType.ModifiedBy, input, input.LastModifiedBy.Device.Id);
            }


            data.Properties[OneDriveVocabularies.DriveItem.Id] = input.Id.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.ODataType] = input.ODataType.PrintIfAvailable();

            data.Properties[OneDriveVocabularies.DriveItem.Name] = input.Name.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.ETag] = input.ETag.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.ParentReference] = input.ParentReference.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.WebUrl] = input.WebUrl.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.CreatedBy] = input.CreatedBy.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.CreatedByUser] = input.CreatedByUser.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.CreatedDateTime] = input.CreatedDateTime.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.Description] = input.Description.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.LastModifiedBy] = input.LastModifiedBy.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.LastModifiedByUser] = input.LastModifiedByUser.PrintIfAvailable();
            data.Properties[OneDriveVocabularies.DriveItem.LastModifiedDateTime] = input.LastModifiedDateTime.PrintIfAvailable();
        }
    }
}
