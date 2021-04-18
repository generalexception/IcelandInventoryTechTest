using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Inventory.Core
{
    public class Item : IItem
    {
        public Item()
        {
            DegredationRules = new List<DegredationRule>();
        }

        public int SellIn { get; set; }
        public int Quality { get; set; }

        [JsonPropertyName("Rules")]
        public IList<DegredationRule> DegredationRules { get; set; }
        public bool NeverExpires { get; set; }
        public string Name { get; set; }
    }
}
