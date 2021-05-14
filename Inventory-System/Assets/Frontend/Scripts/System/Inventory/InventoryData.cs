using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Inventory {
    public class InventoryData : MonoBehaviour
    {
        public InventoryItem clothes;
        public List<InventoryItem> clothesItems;

        public InventoryItem backpack;
        public List<InventoryItem> backpackItems;

        public InventoryItem vest;
        public List<InventoryItem> vestItems;

        public InventoryItem glasses; //очки
        public InventoryItem head; // Все что на голову
        public InventoryItem lips; // Губы (сигареты и т.д.)

    }
}