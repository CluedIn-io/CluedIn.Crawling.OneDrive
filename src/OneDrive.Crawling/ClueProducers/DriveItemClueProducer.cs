﻿using System;
using CluedIn.Core;
using CluedIn.Core.Agent.Jobs;
using CluedIn.Core.Data;
using CluedIn.Crawling.Factories;
using CluedIn.Crawling.OneDrive.Core;
using CluedIn.Crawling.OneDrive.Core.Models;

namespace CluedIn.Crawling.OneDrive.ClueProducers
{
    public partial class DriveItemClueProducer : BaseClueProducer<CluedInDriveItem>
    {
        private readonly IClueFactory factory;
        private readonly IAgentJobProcessorState<OneDriveCrawlJobData> state;
        private readonly ApplicationContext appContext;

        public DriveItemClueProducer(IClueFactory factory, IAgentJobProcessorState<OneDriveCrawlJobData> state, ApplicationContext appContext)
        {
            this.factory = factory;
            this.state = state;
            this.appContext = appContext;
        }

        protected override Clue MakeClueImpl(CluedInDriveItem i, Guid accountId)
        {
            var input = i.DriveItem;
            var clue = factory.Create(EntityType.Infrastructure.DirectoryItem, input.Id, accountId);
            var data = clue.Data.EntityData;
        
            if (input.File != null)
            {
                if (input.AdditionalData.ContainsKey("@microsoft.graph.downloadUrl"))
                {
                    Index(i, input.AdditionalData["@microsoft.graph.downloadUrl"].ToString(), clue);
                }
            }

            PopulateBasicProperties(clue, i);
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
