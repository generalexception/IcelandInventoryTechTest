using System.Collections.Generic;

namespace Inventory.Core
{
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
        public string Name { get; set; }
    }
}
