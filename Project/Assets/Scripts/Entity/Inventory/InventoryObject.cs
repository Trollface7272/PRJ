using System;
using System.Collections.Generic;
using Entity.Inventory.Item;
using UnityEngine;

namespace Entity.Inventory {
    [CreateAssetMenu(fileName = "Inventory", menuName = "Entity/Inventory")]
    public class InventoryObject : ScriptableObject {
        public List<Slot> Items;

    }

    public class Slot {
        public ItemObject Item;
        public int Count;

        public Slot(ItemObject item, int count) {
            Item = item;
            Count = count;
        }
    }
}