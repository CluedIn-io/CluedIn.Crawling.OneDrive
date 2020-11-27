using System;
using System.IO;
using System.Security.Cryptography;
using CluedIn.Core;
using CluedIn.Core.Agent.Jobs;
using CluedIn.Core.Data;
using CluedIn.Core.FileTypes;
using CluedIn.Core.IO;
using CluedIn.Crawling.Factories;
using CluedIn.Crawling.Helpers;
using CluedIn.Crawling.OneDrive.Core;
using CluedIn.Crawling.OneDrive.Core.Models;
using CluedIn.Crawling.OneDrive.Vocabularies;
using Microsoft.Graph;

namespace CluedIn.Crawling.OneDrive.ClueProducers
{
    public partial class DriveItemClueProducer : BaseClueProducer<CluedInDriveItem>
    {
        private readonly IClueFactory factory;
        private readonly IAgentJobProcessorArguments state;
        private readonly ApplicationContext appContext;

        public DriveItemClueProducer(IClueFactory factory, IAgentJobProcessorArguments state, ApplicationContext appContext)
        {
            this.factory = factory;
            this.state = state;
            this.appContext = appContext;
        }

        protected override Clue MakeClueImpl(CluedInDriveItem input, Guid accountId)
        {
            var clue = factory.Create(EntityType.Infrastructure.DirectoryItem, input.Id, accountId);
            var data = clue.Data.EntityData;
            if (input.File != null)
            {
                if (input.AdditionalData.ContainsKey("@microsoft.graph.downloadUrl"))
                {
                    Index(input, input.AdditionalData["@microsoft.graph.downloadUrl"].ToString(), clue);
                }
            }

            PopulateBasicProperties(clue, input);
            if (input.ODataType != null)
            {

            }
            if (input.Content != null)
            {

            }

            return clue;
        }
    }
}
