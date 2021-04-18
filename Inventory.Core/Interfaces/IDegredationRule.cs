namespace Inventory.Core
{
    public interface IDegredationRule
    {
        /// <summary>
        /// Specifies how much to degrade the item quality by.
        /// </summary>
        public int DegredationValue { get; set; }

        /// <summary>
        /// Specifies whether to degrade the quality by a factor of or absolulte value.
        /// </summary>
        public DegredationType DegredationType { get; set; }

        /// <summary>
        /// Specifies a threshold to apply the rule.
        /// </summary>
        public int SellInThreshold { get; set; }
    }
}
