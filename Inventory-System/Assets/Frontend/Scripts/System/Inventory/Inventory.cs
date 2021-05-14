using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Inventory {
    public interface IInventoryMaster
    {
        public void AddItems(InventoryItem item, int amout = 1);
        public void SaveItems();
    }
    public interface ISync
    {
        public void GetItems();
        public void SaveItems();
    }
    
    public interface ISlotInventory
    {
        public InventorySlot Set(InventoryItem _item);
        public InventorySlot Set(InventoryItem _item, int amount = 1);
    }
    public interface IItem
    {
        public void GetItem();
        public void AddItem(InventoryItem data);
    }
}