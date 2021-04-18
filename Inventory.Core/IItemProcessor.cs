using System;
using System.Collections.Generic;

namespace Inventory.Core
{
    public interface IItemProcessor
    {
        public IList<IItem> ItemsToProcess { get; set; }

        public void ProcessItems();
    }

    // All items have a SellIn value which denotes the number of days we have to sell the item
    // All items have a Quality value which denotes how valuable the item is
    public interface IItem
    {
        public int SellIn { get; set; }
        public int Quality { get; set; }
        public IList<IDegredationRule> DegredationRules { get; set; }
        public bool NeverExpires { get; set; }
    }

    public interface IDegredationRule
    {
        public int DegredationValue { get; set; }
        public DegredationType DegredationType { get; set; }
        public int SellInThreshold { get; set; }
    }

    public enum DegredationType
    {
        Factor,
        Absolute
    }
}
