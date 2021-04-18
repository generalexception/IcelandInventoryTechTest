using System;
using System.Collections.Generic;

namespace Inventory.Core
{
    public interface IItemProcessor
    {
        public void ProcessItems();
    }

    public class ItemProcessor : IItemProcessor
    {
        private readonly IList<IItem> _itemsToProcess;

        public ItemProcessor(IList<IItem> ItemsToProcess)
        {
            _itemsToProcess = ItemsToProcess;
        }

        public void ProcessItems()
        {
            throw new NotImplementedException();
        }
    }
}
