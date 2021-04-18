﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Core
{
    public class ItemProcessor : IItemProcessor
    {
        private readonly IList<IItem> _itemsToProcess;

        public ItemProcessor(IList<IItem> ItemsToProcess)
        {
            _itemsToProcess = ItemsToProcess;
        }

        public void ProcessItems()
        {
            foreach (var item in _itemsToProcess)
            {
                if (item.DegredationRules.Count == 0)
                {
                    DecrementQuality(item);
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
            item.Quality -= rule.DegredationValue;
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
        /// Reduces the quality of the item by 1, or 2 if past sell by date.
        /// </summary>
        /// <param name="item">The item to adjust the quality on.</param>
        private void DecrementQuality(IItem item)
        {
            if (item.SellIn < 0)
            {
                item.Quality -= 2;
                return;
            }

            item.Quality--;
        }
    }
}