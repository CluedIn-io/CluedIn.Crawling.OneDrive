using System;
using CluedIn.Core.Data;
using CluedIn.Crawling.Factories;
using CluedIn.Crawling.Helpers;
using CluedIn.Crawling.OneDrive.Core.Models;
using CluedIn.Crawling.OneDrive.Vocabularies;
using Microsoft.Graph;

namespace CluedIn.Crawling.OneDrive.ClueProducers
{
    public static class BaseItemHelper
    {
        public static void PopulateBaseItemClue(Clue clue, BaseItem input, IClueFactory factory, BaseItemVocabulary vocabulary)
        {
            var data = clue.Data.EntityData;
            data.Name = input.Name;
            data.Description = input.Description;
            data.CreatedDate = input.CreatedDateTime;
            data.ModifiedDate = input.LastModifiedDateTime;
            data.Uri = new Uri(input.WebUrl);

            //if (input.ParentReference != null)
            //{
            //    if (!string.IsNullOrWhiteSpace(input.ParentReference.Id))
            //        factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.DirectoryItem, EntityEdgeType.Parent, input, input.ParentReference.Id);
            //}

            //if (input.CreatedByUser != null)
            //{
            //    if (!string.IsNullOrWhiteSpace(input.CreatedByUser.Id))
            //        factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.User, EntityEdgeType.CreatedBy, input, input.CreatedByUser.Id);
            //}

            //if (input.LastModifiedByUser != null)
            //{
            //    if (!string.IsNullOrWhiteSpace(input.LastModifiedByUser.Id))
            //        factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.User, EntityEdgeType.ModifiedBy, input, input.LastModifiedByUser.Id);
            //}

            //if (input.CreatedBy != null)
            //{
            //    if (input.CreatedBy.User != null)
            //        if (!string.IsNullOrWhiteSpace(input.CreatedBy.User.Id))
            //            factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.User, EntityEdgeType.CreatedBy, input, input.CreatedBy.User.Id);

            //    if (input.CreatedBy.Application != null)
            //        if (!string.IsNullOrWhiteSpace(input.CreatedBy.Application.Id))
            //            factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.Application, EntityEdgeType.CreatedBy, input, input.CreatedBy.Application.Id);

            //    if (input.CreatedBy.Device != null)
            //        if (!string.IsNullOrWhiteSpace(input.CreatedBy.Device.Id))
            //            factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure, EntityEdgeType.CreatedBy, input, input.CreatedBy.Device.Id);
            //}

            //if (input.LastModifiedBy != null)
            //{
            //    if (input.LastModifiedBy.User != null)
            //        if (!string.IsNullOrWhiteSpace(input.LastModifiedBy.User.Id))
            //            factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.User, EntityEdgeType.ModifiedBy, input, input.LastModifiedBy.User.Id);

            //    if (input.LastModifiedBy.Application != null)
            //        if (!string.IsNullOrWhiteSpace(input.LastModifiedBy.Application.Id))
            //            factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.Application, EntityEdgeType.ModifiedBy, input, input.LastModifiedBy.Application.Id);

            //    if (input.LastModifiedBy.Device != null)
            //        if (!string.IsNullOrWhiteSpace(input.LastModifiedBy.Device.Id))
            //            factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure, EntityEdgeType.ModifiedBy, input, input.LastModifiedBy.Device.Id);
            //}


            data.Properties[vocabulary.Id] = input.Id.PrintIfAvailable();
            data.Properties[vocabulary.ODataType] = input.ODataType.PrintIfAvailable();

            data.Properties[vocabulary.Name] = input.Name.PrintIfAvailable();
            data.Properties[vocabulary.ETag] = input.ETag.PrintIfAvailable();
            data.Properties[vocabulary.ParentReference] = input.ParentReference.PrintIfAvailable();
            data.Properties[vocabulary.WebUrl] = input.WebUrl.PrintIfAvailable();
            data.Properties[vocabulary.CreatedBy] = input.CreatedBy?.User?.DisplayName?.PrintIfAvailable();
            data.Properties[vocabulary.CreatedByUser] = input.CreatedByUser?.DisplayName?.PrintIfAvailable();
            data.Properties[vocabulary.CreatedDateTime] = input.CreatedDateTime.PrintIfAvailable();
            data.Properties[vocabulary.Description] = input.Description.PrintIfAvailable();
            data.Properties[vocabulary.LastModifiedBy] = input.LastModifiedBy?.User?.DisplayName?.PrintIfAvailable();
            data.Properties[vocabulary.LastModifiedByUser] = input.LastModifiedByUser?.DisplayName?.PrintIfAvailable();
            data.Properties[vocabulary.LastModifiedDateTime] = input.LastModifiedDateTime.PrintIfAvailable();
        }
    }
}
