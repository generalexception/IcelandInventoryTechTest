using System.Collections.Generic;

namespace Inventory.Core
{
    public interface IItem
    {
        /// <summary>
        /// All items have a SellIn value which denotes the number of days we have to sell the item.
        /// </summary>
        public int SellIn { get; set; }

        /// <summary>
        /// All items have a Quality value which denotes how valuable the item is.
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// Specifies a list of rules to apply to the item when processed.
        /// </summary>
        public IList<IDegredationRule> DegredationRules { get; set; }

        /// <summary>
        /// Specified if the items SellIn value gets reduced or not when processed.
        /// </summary>
        public bool NeverExpires { get; set; }

        /// <summary>
        /// The name of the Item e.g. "Aged Brie"
        /// </summary>
        public string Name { get; set; }
    }
}
