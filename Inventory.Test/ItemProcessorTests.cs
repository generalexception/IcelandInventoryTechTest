using Inventory.Core;
using Xunit;
using System.Collections.Generic;

namespace Inventory.Test
{
    public class ItemProcessorTests
    {
        private IItemProcessor itemProcessor;
        private readonly IItem itemUnderTest;

        public ItemProcessorTests()
        {
            itemUnderTest = new Item();
        }

        // At the end of each day our system lowers both values for every item
        [Fact]
        public void AtTheEndOfTheDay_SystemLowersBothValuesForItem()
        {
            itemUnderTest.Quality = 2;
            itemUnderTest.SellIn = 3;

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
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

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
            itemProcessor.ProcessItems();

            Assert.Equal(8, itemUnderTest.Quality);
            Assert.Equal(-2, itemUnderTest.SellIn);
        }

        // The Quality of an item is never negative
        [Fact]
        public void WhenQualityIsAtZero_AndQualityDecreased_ThenQualityRemainsAtZero()
        {
            itemUnderTest.Quality = 0;
            itemUnderTest.SellIn = 9;

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
            itemProcessor.ProcessItems();

            Assert.Equal(0, itemUnderTest.Quality);
            Assert.Equal(8, itemUnderTest.SellIn);
        }

        // The Quality of an item is never more than 50
        [Fact]
        public void WhenQualityIsAtFifty_AndQualityIncreased_ThenQualityRemainsAtFifty()
        {
            itemUnderTest.DegredationRules.Add(new DegredationRule { DegredationValue = -1 });
            itemUnderTest.Quality = 50;
            itemUnderTest.SellIn = 9;

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
            itemProcessor.ProcessItems();

            Assert.Equal(50, itemUnderTest.Quality);
            Assert.Equal(8, itemUnderTest.SellIn);
        }

        // "Aged Brie" increases in Quality the older it gets
        [Fact]
        public void WhenDegredationFactorIsNegative_ThenQualityIncreasesByDegredationFactor()
        {
            itemUnderTest.DegredationRules.Add(new DegredationRule { DegredationValue = -2 });
            itemUnderTest.Quality = 10;
            itemUnderTest.SellIn = 9;

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
            itemProcessor.ProcessItems();

            Assert.Equal(12, itemUnderTest.Quality);
            Assert.Equal(8, itemUnderTest.SellIn);
        }

        // “Frozen Item” decreases in Quality by 1
        // "Fresh Item" degrade in Quality twice as fast as “Frozen Item”
        [Fact]
        public void WhenDegredationFactorIsPositive_ThenQualityDecreasesByDegredationFactor()
        {
            itemUnderTest.DegredationRules.Add(new DegredationRule { DegredationValue = 4 });
            itemUnderTest.Quality = 10;
            itemUnderTest.SellIn = 9;

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
            itemProcessor.ProcessItems();

            Assert.Equal(6, itemUnderTest.Quality);
            Assert.Equal(8, itemUnderTest.SellIn);
        }

        // "Soap" never decreases in Quality
        [Fact]
        public void WhenDegredationFactorIsZero_ThenQualityRemainsTheSame()
        {
            itemUnderTest.DegredationRules.Add(new DegredationRule { DegredationValue = 0 });
            itemUnderTest.Quality = 48;
            itemUnderTest.SellIn = 9;

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
            itemProcessor.ProcessItems();

            Assert.Equal(48, itemUnderTest.Quality);
            Assert.Equal(8, itemUnderTest.SellIn);
        }

        // "Soap" never has to be sold
        [Fact]
        public void WhenItemNeverExpiresIsTrue_ThenSellInRemainsTheSame()
        {
            itemUnderTest.SellIn = 4;
            itemUnderTest.NeverExpires = true;

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
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

            var rule1 = new DegredationRule
            {
                DegredationValue = -2,
                SellInThreshold = 10
            };
            itemUnderTest.DegredationRules.Add(rule1);

            var rule2 = new DegredationRule
            {
                DegredationValue = -3,
                SellInThreshold = 5
            };
            itemUnderTest.DegredationRules.Add(rule2);

            var rule3 = new DegredationRule
            {
                DegredationValue = 0,
                DegredationType = DegredationType.Absolute,
                SellInThreshold = -1
            };
            itemUnderTest.DegredationRules.Add(rule3);

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
            itemProcessor.ProcessItems();

            Assert.Equal(6, itemUnderTest.Quality);
        }

        // and by 3 when there are 5 days or less
        [Fact]
        public void WhenThereAreThreeDaysOrLess_ThenQualityIncreasesByThree()
        {
            itemUnderTest.Quality = 4;
            itemUnderTest.SellIn = 3;

            var rule1 = new DegredationRule
            {
                DegredationValue = -2,
                SellInThreshold = 10
            };
            itemUnderTest.DegredationRules.Add(rule1);

            var rule2 = new DegredationRule
            {
                DegredationValue = -3,
                SellInThreshold = 5
            };
            itemUnderTest.DegredationRules.Add(rule2);

            var rule3 = new DegredationRule
            {
                DegredationValue = 0,
                DegredationType = DegredationType.Absolute,
                SellInThreshold = -1
            };
            itemUnderTest.DegredationRules.Add(rule3);

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
            itemProcessor.ProcessItems();

            Assert.Equal(7, itemUnderTest.Quality);
        }

        // but quality drops to 0 after Christmas
        [Fact]
        public void OnceSellByDateIsLessThanZero_ThenQualityDropsToZero()
        {
            itemUnderTest.Quality = 4;
            itemUnderTest.SellIn = -1;

            var rule1 = new DegredationRule
            {
                DegredationValue = -2,
                SellInThreshold = 10
            };
            itemUnderTest.DegredationRules.Add(rule1);

            var rule2 = new DegredationRule
            {
                DegredationValue = -3,
                SellInThreshold = 5
            };
            itemUnderTest.DegredationRules.Add(rule2);

            var rule3 = new DegredationRule
            {
                DegredationValue = 0,
                DegredationType = DegredationType.Absolute,
                SellInThreshold = -1
            };
            itemUnderTest.DegredationRules.Add(rule3);

            itemProcessor = new ItemProcessor(new List<IItem>() { itemUnderTest });
            itemProcessor.ProcessItems();

            Assert.Equal(0, itemUnderTest.Quality);
        }
    }
}
