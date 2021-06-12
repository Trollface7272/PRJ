using Entity.Player;
using UnityEngine.Tilemaps;

namespace Inventory.Items {
    public static class ItemUtils {
        public static bool IsBreakable(PickaxeObject pick, BlockObject tile) {
            return pick.miningPower >= tile.hardness;
        }
    
        public static BlockObject TileToItem(Tile tile) {
            var items = PlayerController.Instance.player.itemList;
            return tile.name switch {
                "dirt_grass" => items.FindTileByName("Grass"),
                "dirt" => items.FindTileByName("Dirt"),
                "stone" => items.FindTileByName("Stone"),
                "stone_coal" => items.FindTileByName("Coal Ore"),
                "stone_browniron" => items.FindTileByName("Iron Ore"),
                "stone_gold" => items.FindTileByName("Gold Ore"),
                "stone_diamond" => items.FindTileByName("Grass"),
                _ => null
            };
        }
    }
}