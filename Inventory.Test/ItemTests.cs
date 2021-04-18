using Inventory.Core;
using Xunit;
using System.Linq;

namespace Inventory.Test
{
    public class ItemTests
    {
        private readonly IItemProcessor itemProcessor;
        private readonly IItem itemUnderTest;

        public ItemTests()
        {

        }

        // At the end of each day our system lowers both values for every item
        [Fact]
        public void AtTheEndOfTheDay_SystemLowersBothValuesForItem()
        {
            itemUnderTest.Quality = 2;
            itemUnderTest.SellIn = 3;

            itemProcessor.ProcessItems();
            Assert.Equal(1, itemUnderTest.Quality);
            Assert.Equal(2, itemUnderTest.SellIn);
        }

        // Once the sell by date has passed, Quality degrades twice as fast
        [Fact]
        public void OnceSellByDateIsLessThanZero_ThenQualityDegradesTwiceAsFast()
        {
            itemUnderTest.SellIn = -1;
            itemUnderTest.Quality = 10;
            itemProcessor.ProcessItems();
            Assert.Equal(8, itemUnderTest.Quality);
        }

        // The Quality of an item is never negative
        [Fact]
        public void WhenQualityIsAtZero_AndQualityDecreased_ThenQualityRemainsAtZero()
        {
            itemUnderTest.Quality = 0;
            itemProcessor.ProcessItems();
            Assert.Equal(0, itemUnderTest.Quality);
        }

        // The Quality of an item is never more than 50
        [Fact]
        public void WhenQualityIsAtFifty_AndQualityIncreased_ThenQualityRemainsAtFifty()
        {
            itemUnderTest.Quality = 50;
            itemProcessor.ProcessItems();
            Assert.Equal(50, itemUnderTest.Quality);
        }

        // "Aged Brie" increases in Quality the older it gets
        [Fact]
        public void WhenDegredationFactorIsNegative_ThenQualityIncreasesByDegredationFactor()
        {
            itemUnderTest.DegredationRules.Single().DegredationValue = -2;
            itemUnderTest.Quality = 10;
            itemProcessor.ProcessItems();
            Assert.Equal(12, itemUnderTest.Quality);
        }

        // “Frozen Item” decreases in Quality by 1
        // "Fresh Item" degrade in Quality twice as fast as “Frozen Item”
        [Fact]
        public void WhenDegredationFactorIsPositive_ThenQualityDecreasesByDegredationFactor()
        {
            itemUnderTest.DegredationRules.Single().DegredationValue = 4;
            itemUnderTest.Quality = 10;
            itemProcessor.ProcessItems();
            Assert.Equal(6, itemUnderTest.Quality);
        }

        // "Soap" never has to be sold
        [Fact]
        public void WhenDegredationFactorIsZero_ThenQualityRemainsTheSame()
        {
            itemUnderTest.DegredationRules.Single().DegredationValue = 0;
            itemUnderTest.Quality = 48;
            itemProcessor.ProcessItems();
            Assert.Equal(48, itemUnderTest.Quality);
        }

        // "Soap" never decreases in Quality
        [Fact]
        public void WhenItemNeverExpiresIsTrue_ThenSellInRemainsTheSame()
        {
            itemUnderTest.SellIn = 4;
            itemUnderTest.NeverExpires = true;
            itemProcessor.ProcessItems();
            Assert.Equal(4, itemUnderTest.SellIn);
        }

        // "Christmas Crackers", like “Aged Brie”, increases in Quality as its SellIn value approaches; Its
        // quality increases by 2 when there are 10 days or less
        [Fact]
        public void WhenThereAreTenDaysOrLess_ThenQualityIncreasesByTwo()
        {
            itemUnderTest.Quality = 4;
            itemUnderTest.SellIn = 10;

            IDegredationRule rule1 = itemUnderTest.DegredationRules.Single();
            rule1.DegredationValue = -2;
            rule1.SellInThreshold = 10;

            IDegredationRule rule2 = null;
            rule2.DegredationValue = -3;
            rule2.SellInThreshold = 5;
            itemUnderTest.DegredationRules.Add(rule2);

            IDegredationRule rule3 = null;
            rule3.DegredationValue = 0;
            rule3.DegredationType = DegredationType.Absolute;
            rule3.SellInThreshold = -1;
            itemUnderTest.DegredationRules.Add(rule3);

            itemProcessor.ProcessItems();
            Assert.Equal(6, itemUnderTest.Quality);
        }

        // and by 3 when there are 5 days or less
        [Fact]
        public void WhenThereAreThreeDaysOrLess_ThenQualityIncreasesByThree()
        {
            itemUnderTest.Quality = 4;
            itemUnderTest.SellIn = 3;

            IDegredationRule rule1 = itemUnderTest.DegredationRules.Single();
            rule1.DegredationValue = -2;
            rule1.SellInThreshold = 10;

            IDegredationRule rule2 = null;
            rule2.DegredationValue = -3;
            rule2.SellInThreshold = 5;
            itemUnderTest.DegredationRules.Add(rule2);

            IDegredationRule rule3 = null;
            rule3.DegredationValue = 0;
            rule3.DegredationType = DegredationType.Absolute;
            rule3.SellInThreshold = -1;
            itemUnderTest.DegredationRules.Add(rule3);

            itemProcessor.ProcessItems();
            Assert.Equal(7, itemUnderTest.Quality);
        }

        // but quality drops to 0 after Christmas
        [Fact]
        public void OnceSellByDateIsLessThanZero_ThenQualityDropsToZero()
        {
            itemUnderTest.Quality = 4;
            itemUnderTest.SellIn = -1;

            IDegredationRule rule1 = itemUnderTest.DegredationRules.Single();
            rule1.DegredationValue = -2;
            rule1.SellInThreshold = 10;

            IDegredationRule rule2 = null;
            rule2.DegredationValue = -3;
            rule2.SellInThreshold = 5;
            itemUnderTest.DegredationRules.Add(rule2);

            IDegredationRule rule3 = null;
            rule3.DegredationValue = 0;
            rule3.DegredationType = DegredationType.Absolute;
            rule3.SellInThreshold = -1;
            itemUnderTest.DegredationRules.Add(rule3);

            itemProcessor.ProcessItems();
            Assert.Equal(0, itemUnderTest.Quality);
        }
    }
}
