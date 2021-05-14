using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Inventory {
    [RequireComponent(typeof(Rigidbody))]
    public class InventoryItem : ItemData
    {
        public InventoryItem GetItem()
        {
            return this;
        }
    }
}