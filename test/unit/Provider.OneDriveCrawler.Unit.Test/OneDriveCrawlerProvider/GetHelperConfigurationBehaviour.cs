using System;
using System.Linq;
using AutoFixture.Xunit2;
using CluedIn.Core.Crawling;
using CluedIn.Crawling.OneDriveCrawler.Core;
using Should;
using Xunit;

namespace CluedIn.Provider.OneDriveCrawler.Unit.Test.OneDriveCrawlerProvider
{
    public class GetHelperConfigurationBehaviour : OneDriveCrawlerProviderTest
    {
        private readonly CrawlJobData _jobData;

        public GetHelperConfigurationBehaviour()
        {
            _jobData = new OneDriveCrawlerCrawlJobData();
        }

        [Fact]
        public void Throws_ArgumentNullException_With_Null_CrawlJobData_Parameter()
        {
            var ex = Assert.Throws<AggregateException>(
                () => Sut.GetHelperConfiguration(null, null, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
                    .Wait());

            ((ArgumentNullException)ex.InnerExceptions.Single())
                .ParamName
                .ShouldEqual("jobData");
        }

        [Theory]
        [InlineAutoData]
        public void Returns_ValidDictionary_Instance(Guid organizationId, Guid userId, Guid providerDefinitionId)
        {
            Sut.GetHelperConfiguration(null, _jobData, organizationId, userId, providerDefinitionId)
                .Result
                .ShouldNotBeNull();
        }

        // TODO Add test for throws arg exception for incorrect data param


        [Theory]
        [InlineAutoData("UserName", "UserName", "mts@cluedin.com")]
        [InlineAutoData("Password", "Password", "some-value")]
        [InlineAutoData("TenantId", "TenantId", "f5ae2861-b3fc-449d-a9e7-49c14d011ac0")]
        [InlineAutoData("ApplicationId", "ApplicationId", "0333d932-8824-4ff8-ae2b-86d0c4d53177")]
        // TODO add data for other properties that need populating
        // Fill in the values for expected results ....
        public void Returns_Expected_Data(string key, string propertyName, object expectedValue, Guid organizationId, Guid userId, Guid providerDefinitionId) // TODO add additional parameters to populate CrawlJobData instance
        {
            var property = _jobData.GetType().GetProperty(propertyName);
            property?.SetValue(_jobData, expectedValue);

            var result = Sut.GetHelperConfiguration(null, _jobData, organizationId, userId, providerDefinitionId)
                            .Result;

            result
                .ContainsKey(key)
                .ShouldBeTrue(
                    $"{key} not found in results");

            result[key]
                .ShouldEqual(expectedValue);
        }
    }
}
