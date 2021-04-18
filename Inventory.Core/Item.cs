using System.Collections.Generic;

namespace Inventory.Core
{
    // All items have a SellIn value which denotes the number of days we have to sell the item
    // All items have a Quality value which denotes how valuable the item is
    public interface IItem
    {
        public int SellIn { get; set; }
        public int Quality { get; set; }
        public IList<IDegredationRule> DegredationRules { get; set; }
        public bool NeverExpires { get; set; }
    }

    public class Item : IItem
    {
        public int _quality;
        public Item()
        {
            DegredationRules = new List<IDegredationRule>();
        }

        public int SellIn { get; set; }

        public int Quality {
            get
            {
                return _quality;
            }
            set
            {
                _quality = value;
                if (value < 0) { _quality = 0; }
                if (value > 50) { _quality = 50; }
            }
        }

        public IList<IDegredationRule> DegredationRules { get; set; }
        public bool NeverExpires { get; set; }
    }
}
