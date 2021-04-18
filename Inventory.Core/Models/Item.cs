using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Inventory.Core
{
    public class Item : IItem
    {
        public Item()
        {
            DegredationRules = new List<IDegredationRule>();
        }

        public int SellIn { get; set; }
        public int Quality { get; set; }

        [JsonPropertyName("Rules")]
        public IList<IDegredationRule> DegredationRules { get; set; }
        public bool NeverExpires { get; set; }
        public string Name { get; set; }
    }
}
