using System.Linq;
using Entity.Player;
using UnityEngine.Tilemaps;

namespace Inventory.Items {
    public static class ItemUtils {
        public static bool IsBreakable(PickaxeObject pick, BlockObject tile) {
            return pick.miningPower >= tile.hardness;
        }
    
        public static BlockObject TileToItem(Tile tile) {
            var items = PlayerController.Instance.player.itemList.tiles;
            return items.FirstOrDefault(blockObject => blockObject.tile.name == tile.name);
        }
    }
}