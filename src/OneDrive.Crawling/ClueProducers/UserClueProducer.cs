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
    public class UserClueProducer : BaseClueProducer<CluedInUser>
    {
        private readonly IClueFactory factory;
        private readonly IAgentJobProcessorState<OneDriveCrawlJobData> state;
        private readonly ApplicationContext appContext;

        public UserClueProducer(IClueFactory factory, IAgentJobProcessorState<OneDriveCrawlJobData> state, ApplicationContext appContext)
        {
            this.factory = factory;
            this.state = state;
            this.appContext = appContext;
        }

        protected override Clue MakeClueImpl(CluedInUser i, Guid accountId)
        {
            var input = i.User;
            var clue = factory.Create(EntityType.Infrastructure.User, input.Id, accountId);
            var data = clue.Data.EntityData;

            data.Name = input.DisplayName;
            data.Description = input.JobTitle;

            if (input.AdditionalData != null)
            {
                if (input.AdditionalData.ContainsKey("@microsoft.graph.downloadUrl"))
                {

                }
            }

            return clue;
        }
    }
}
