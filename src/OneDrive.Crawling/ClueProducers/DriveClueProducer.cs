using System;
using CluedIn.Core;
using CluedIn.Core.Agent.Jobs;
using CluedIn.Core.Data;
using CluedIn.Crawling.Factories;
using CluedIn.Crawling.Helpers;
using CluedIn.Crawling.OneDrive.Core;
using CluedIn.Crawling.OneDrive.Core.Models;
using CluedIn.Crawling.OneDrive.Vocabularies;

namespace CluedIn.Crawling.OneDrive.ClueProducers
{
    public class DriveClueProducer : BaseClueProducer<CluedInDrive>
    {
        private readonly IClueFactory factory;
        private readonly IAgentJobProcessorState<OneDriveCrawlJobData> state;
        private readonly ApplicationContext appContext;

        public DriveClueProducer(IClueFactory factory, IAgentJobProcessorState<OneDriveCrawlJobData> state, ApplicationContext appContext)
        {
            this.factory = factory;
            this.state = state;
            this.appContext = appContext;
        }

        protected override Clue MakeClueImpl(CluedInDrive i, Guid accountId)
        {
            var input = i.Drive;
            var clue = factory.Create(EntityType.Infrastructure.Folder, input.Id, accountId);
            var data = clue.Data.EntityData;
            data.Codes.Add(new EntityCode(EntityType.Infrastructure.DirectoryItem, OneDriveConstants.CodeOrigin, input.Id));
            var vocab = OneDriveVocabularies.DriveItem;
            BaseItemHelper.PopulateBaseItemClue(clue, input, factory, OneDrive.Vocabularies.OneDriveVocabularies.Drive);
            if (input.AdditionalData != null)
            {
                if (input.AdditionalData.ContainsKey("@microsoft.graph.downloadUrl"))
                {

                }
            }

            if (input.Owner != null)
            {
                if (input.Owner.User != null)
                    if (!string.IsNullOrWhiteSpace(input.Owner.User.Id))
                        factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.User, EntityEdgeType.OwnedBy, input, input.Owner.User.Id);

                if (input.Owner.Application != null)
                    if (!string.IsNullOrWhiteSpace(input.Owner.Application.Id))
                        factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.Application, EntityEdgeType.OwnedBy, input, input.Owner.Application.Id);

                if (input.Owner.Device != null)
                    if (!string.IsNullOrWhiteSpace(input.Owner.Device.Id))
                        factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure, EntityEdgeType.OwnedBy, input, input.Owner.Device.Id);
            }

            if (input.Quota != null)
            {

            }

            if (input.SharePointIds != null)
            {

            }

            if (input.System != null)
            {

            }

            if (input.List != null)
            {

            }

            if (input.Root != null)
            {
                if (!string.IsNullOrWhiteSpace(input.Root.Id))
                    factory.CreateOutgoingEntityReference(clue, EntityType.Infrastructure.DirectoryItem, EntityEdgeType.Parent, input, input.Root.Id);
            }

            if (input.Special != null)
            {

            }

            data.Properties[OneDriveVocabularies.Drive.DriveType] = input.DriveType;

            return clue;
        }
    }
}
