using System.Collections.Generic;
using Inventory.Core;
using Xunit;
using System.Linq;

namespace Inventory.Test
{
    /*
        Rules:
        • All items have a SellIn value which denotes the number of days we have to sell the item
        • All items have a Quality value which denotes how valuable the item is
        • At the end of each day our system lowers both values for every item
        • Once the sell by date has passed, Quality degrades twice as fast
        • The Quality of an item is never negative
        • The Quality of an item is never more than 50
        • "Aged Brie" increases in Quality the older it gets
        • “Frozen Item” decreases in Quality by 1
        • "Soap" never has to be sold or decreases in Quality
        • "Christmas Crackers", like “Aged Brie”, increases in Quality as its SellIn value approaches; Its
        • quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less
        • but quality drops to 0 after Christmas
        • "Fresh Item" degrade in Quality twice as fast as “Frozen Item”
    */

    public class ItemTests
    {
        private readonly IItemProcessor itemProcessor;
        private readonly IItem itemUnderTest;

        public ItemTests()
        {

        }

        [Fact]
        public void AtTheEndOfTheDay_SystemLowersBothValuesForItem()
        {
            itemUnderTest.Quality = 2;
            itemUnderTest.SellIn = 3;

            itemProcessor.ProcessItems();
            Assert.Equal(1, itemUnderTest.Quality);
            Assert.Equal(2, itemUnderTest.SellIn);
        }

        [Fact]
        public void OnceSellByDateIsLessThanZero_ThenQualityDegradesTwiceAsFast()
        {
            itemUnderTest.SellIn = -1;
            itemUnderTest.Quality = 10;
            itemProcessor.ProcessItems();
            Assert.Equal(8, itemUnderTest.Quality);
        }

        [Fact]
        public void WhenQualityIsAtZero_AndQualityDecreased_ThenQualityRemainsAtZero()
        {
            itemUnderTest.Quality = 0;
            itemProcessor.ProcessItems();
            Assert.Equal(0, itemUnderTest.Quality);
        }

        [Fact]
        public void WhenQualityIsAtFifty_AndQualityIncreased_ThenQualityRemainsAtFifty()
        {
            itemUnderTest.Quality = 50;
            itemProcessor.ProcessItems();
            Assert.Equal(50, itemUnderTest.Quality);
        }

        [Fact]
        public void WhenDegredationFactorIsNegative_ThenQualityIncreasesByDegredationFactor()
        {
            itemUnderTest.DegredationRules.Single().DegredationValue = -2;
            itemUnderTest.Quality = 10;
            itemProcessor.ProcessItems();
            Assert.Equal(12, itemUnderTest.Quality);
        }

        [Fact]
        public void WhenDegredationFactorIsPositive_ThenQualityDecreasesByDegredationFactor()
        {
            itemUnderTest.DegredationRules.Single().DegredationValue = 4;
            itemUnderTest.Quality = 10;
            itemProcessor.ProcessItems();
            Assert.Equal(6, itemUnderTest.Quality);
        }

        [Fact]
        public void WhenDegredationFactorIsZero_ThenQualityRemainsTheSame()
        {
            itemUnderTest.DegredationRules.Single().DegredationValue = 0;
            itemUnderTest.Quality = 48;
            itemProcessor.ProcessItems();
            Assert.Equal(48, itemUnderTest.Quality);
        }
    }
}
