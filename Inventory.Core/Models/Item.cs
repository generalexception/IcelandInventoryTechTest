using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Inventory.Core
{
    public class Item : IItem
    {
        public int _quality;
        public Item()
        {
            DegredationRules = new List<DegredationRule>();
        }

        public int SellIn { get; set; }

        public int Quality {
            get
            {
                //if (_quality < 0) { return 0; }
                //if (_quality > 50) { return 50; }
                return _quality;
            }
            set
            {
                _quality = value;
            }
        }

        [JsonPropertyName("Rules")]
        public IList<DegredationRule> DegredationRules { get; set; }
        public bool NeverExpires { get; set; }
        public string Name { get; set; }
    }
}
