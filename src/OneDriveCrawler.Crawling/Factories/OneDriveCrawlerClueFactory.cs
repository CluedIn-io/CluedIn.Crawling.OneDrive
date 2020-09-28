using System;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Crawling.Factories;
using CluedIn.Crawling.OneDriveCrawler.Core;
using RuleConstants = CluedIn.Core.Constants.Validation.Rules;

namespace CluedIn.Crawling.OneDriveCrawler.Factories
{
    public class OneDriveCrawlerClueFactory : ClueFactory
    {
        public OneDriveCrawlerClueFactory()
            : base(OneDriveCrawlerConstants.CodeOrigin, OneDriveCrawlerConstants.ProviderRootCodeValue)
        {
        }

        protected override Clue ConfigureProviderRoot([NotNull] Clue clue)
        {
            if (clue == null)
            {
                throw new ArgumentNullException(nameof(clue));
            }

            var data = clue.Data.EntityData;
            data.Name = OneDriveCrawlerConstants.CrawlerName;
            data.Uri = new Uri(OneDriveCrawlerConstants.Uri);
            data.Description = OneDriveCrawlerConstants.CrawlerDescription;

            clue.ValidationRuleSuppressions.AddRange(new[] {RuleConstants.PROPERTIES_001_MustExist,});

            clue.ValidationRuleSuppressions.AddRange(new[]
            {
                RuleConstants.METADATA_002_Uri_MustBeSet, RuleConstants.PROPERTIES_002_Unknown_VocabularyKey_Used
            });

            return clue;
        }
    }
}
