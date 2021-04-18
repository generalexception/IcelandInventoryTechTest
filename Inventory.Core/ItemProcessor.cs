﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Core
{
    public class ItemProcessor : IItemProcessor
    {
        private readonly IList<Item> _itemsToProcess;

        public ItemProcessor(IList<Item> ItemsToProcess)
        {
            _itemsToProcess = ItemsToProcess;
        }

        public void ProcessItems()
        {
            foreach (var item in _itemsToProcess)
            {
                if (item.DegredationRules.Count == 0)
                {
                    continue;
                }

                else
                {
                    var rule = item.DegredationRules
                        .OrderBy(rule => Math.Abs(item.SellIn - rule.SellInThreshold))
                        .First();

                    ApplyDegredationRule(item, rule);
                }

                DecrementSellInValue(item);
            }
        }

        /// <summary>
        /// Applies a DegredationRule to an Item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rule"></param>
        private void ApplyDegredationRule(IItem item, IDegredationRule rule)
        {
            if (rule.DegredationType == DegredationType.Absolute)
                item.Quality = rule.DegredationValue;

            DecrementQuality(item, rule.DegredationValue);
        }

        /// <summary>
        /// Reduces the items SellIn value if SellIn is false.
        /// </summary>
        /// <param name="item"></param>
        private void DecrementSellInValue(IItem item)
        {
            if (item.NeverExpires)
                return;
            item.SellIn--;
        }

        /// <summary>
        /// Decrements the quality of the item. If the items SellIn date is negative then quality decreases twofold.
        /// </summary>
        /// <param name="item">The item to decrease the quality on.</param>
        /// <param name="value">How much to decrease it by.</param>
        private void DecrementQuality(IItem item, int value)
        {
            if (item.SellIn < 0)
                item.Quality -= value * 2;
            else
                item.Quality -= value;

            if (item.Quality > 50)
                item.Quality = 50;

            if (item.Quality < 0)
                item.Quality = 0;
        }
    }
}
