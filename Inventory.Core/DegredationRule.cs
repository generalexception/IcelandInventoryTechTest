namespace Inventory.Core
{
    public interface IDegredationRule
    {
        public int DegredationValue { get; set; }
        public DegredationType DegredationType { get; set; }
        public int SellInThreshold { get; set; }
    }

    public class DegredationRule : IDegredationRule
    {
        public int DegredationValue { get; set; }
        public DegredationType DegredationType { get; set; }
        public int SellInThreshold { get; set; }
    }
}
