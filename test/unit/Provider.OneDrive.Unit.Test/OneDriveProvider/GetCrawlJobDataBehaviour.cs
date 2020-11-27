using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using CluedIn.Core.Crawling;
using CluedIn.Core.Providers;
using CluedIn.Crawling.OneDrive.Core;
using Xunit;

namespace CluedIn.Provider.OneDrive.Unit.Test.OneDriveProvider
{
    public class GetCrawlJobDataBehaviour : OneDriveProviderTest
    {
        private readonly ProviderUpdateContext _context;

        public GetCrawlJobDataBehaviour()
        {
            _context = null;
        }

        [Theory]
        [InlineAutoData]
        public async Task ReturnsData(Dictionary<string, object> dictionary, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            Assert.NotNull(
                await Sut.GetCrawlJobData(_context, dictionary, organizationId, userId, providerDefinitionId));
        }

        [Theory]
        [InlineAutoData(OneDriveConstants.KeyName.ClientID, nameof(OneDriveCrawlJobData.ClientID))]
        [InlineAutoData(OneDriveConstants.KeyName.ClientSecret, nameof(OneDriveCrawlJobData.ClientSecret))]
        [InlineAutoData(OneDriveConstants.KeyName.Tenant, nameof(OneDriveCrawlJobData.Tenant))]
        public async Task InitializesProperties(string key, string propertyName, string sampleValue, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            var dictionary = new Dictionary<string, object>()
            {
                [key] = sampleValue
            };

            var sut = await Sut.GetCrawlJobData(_context, dictionary, organizationId, userId, providerDefinitionId);
            Assert.Equal(sampleValue, sut.GetType().GetProperty(propertyName).GetValue(sut));
        }

        [Theory]
        [InlineAutoData]
        public async Task OneDriveCrawlJobDataReturned(Dictionary<string, object> dictionary, Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            Assert.IsType<CluedIn.Crawling.OneDrive.Core.OneDriveCrawlJobData>(
                await Sut.GetCrawlJobData(_context, dictionary, organizationId, userId, providerDefinitionId));
        }
    }
}
