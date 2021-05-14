using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace System.Inventory {
    public class ItemData : MonoBehaviour
    {
        public string classname;
        public string model;

        public string displayName;
        public string descriptionShort;
        public string picture;
        public string version;

        public string author;
        public string authorUrl;
        public string url;

        public List<InventoryItem> requiredAddons;

        public int maximumLoad;
        public float mass;

        public List<InventoryItem> items;
    }
}