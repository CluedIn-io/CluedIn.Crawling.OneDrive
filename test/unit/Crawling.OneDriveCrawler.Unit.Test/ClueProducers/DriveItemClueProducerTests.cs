using AutoFixture.Xunit2;
using System;
using Xunit;
using CluedIn.Core.Data;
using CluedIn.Crawling;
using CluedIn.Crawling.OneDriveCrawler.ClueProducers;
using CluedIn.Crawling.OneDriveCrawler.Core.Models;

namespace Crawling.OneDriveCrawler.Unit.Test.ClueProducers
{
    public class DriveItemClueProducerTests : BaseClueProducerTest<DriveItem>
    {
        protected override BaseClueProducer<DriveItem> Sut =>
            new DriveItemClueProducer(_clueFactory.Object);

        protected override EntityType ExpectedEntityType => EntityType.Files;

        [Theory]
        [InlineAutoData]
        public void ClueHasEdgeToFolder(DriveItem driveitem)
        {
            var clue = Sut.MakeClue(driveitem, Guid.NewGuid());
            _clueFactory.Verify(
                //TODO verify some methods were called
                );
        }

        //TODO add other tests
    }
}
