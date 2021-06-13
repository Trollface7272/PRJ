using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Crafting;
using Inventory.Items;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Inventory {
    [CreateAssetMenu(fileName = "New Item List Object", menuName = "Inventory System/Item List")]
    public class ItemListObject : ScriptableObject {
        public List<ItemObject> items;
        public List<BlockObject> tiles;
        public List<OreObject> ores;
        public List<RecipeObject> recipes;

        public BlockObject FindTileByName(string tileName) {
            return tiles.FirstOrDefault(tile => tile.name == tileName);
        }
        public BlockObject FindTileByTile(Tile t) {
            return tiles.FirstOrDefault(tile => tile.tile.name == t.name);
        }

        public ItemObject FindItemByName(string itemName) {
            return items.FirstOrDefault(item => item.name == itemName);
        }
    }
}