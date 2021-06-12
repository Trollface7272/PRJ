using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Crafting;
using Inventory.Items;
using UnityEngine;

namespace Inventory {
    [CreateAssetMenu(fileName = "New Item List Object", menuName = "Inventory System/Item List")]
    public class ItemListObject : ScriptableObject {
        public List<ItemObject> items;
        public List<BlockObject> tiles;
        public List<OreObject> ores;
        public List<RecipeObject> recipes;

        public BlockObject FindTileByName(string oreName) {
            return tiles.FirstOrDefault(tile => tile.name == oreName);
        }
    }
}