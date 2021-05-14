using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Inventory
{
    public class InventoryMaster : InventoryData, ISync
    {
        public void GetItems()
        {
            throw new System.NotImplementedException();
        }

        public void SaveItems()
        {
            throw new System.NotImplementedException();
        }
    }

    public class InventorySlot : ISlotInventory
    {
        public InventorySlot(InventoryItem _item) => Set(_item);
        public InventorySlot(InventoryItem _item, int amount = 1) => Set(_item, amount);
        public InventorySlot Set(InventoryItem _item)
        {
            throw new NotImplementedException();
        }
        public InventorySlot Set(InventoryItem _item, int amount = 1)
        {
            throw new NotImplementedException();
        }
    }
}