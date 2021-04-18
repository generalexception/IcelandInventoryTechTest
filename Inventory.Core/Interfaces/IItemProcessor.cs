namespace Inventory.Core
{
    public interface IItemProcessor
    {
        /// <summary>
        /// Processes the items given to the processor.
        /// This would probably be invoked on a daily basis by something else.
        /// </summary>
        public void ProcessItems();
    }
}
