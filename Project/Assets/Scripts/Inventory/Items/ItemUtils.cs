using Inventory.Items;
using UnityEngine.Tilemaps;
using Entity.Player;

public static class ItemUtils {
    public static bool IsBreakable(PickaxeObject pick, BlockObject tile) {
        return pick.miningPower >= tile.hardness;
    }
    
    public static ItemObject TileToItem(Tile tile) {
        switch (tile.name) {
            case "dirt_grass":
                return PlayerController.Instance.player.itemList.GetItemAt(0);
            case "dirt":
                return PlayerController.Instance.player.itemList.GetItemAt(1);
            case "stone":
                return PlayerController.Instance.player.itemList.GetItemAt(2);
            case "stone_coal":
                return PlayerController.Instance.player.itemList.GetItemAt(3);
            case "stone_browniron":
                return PlayerController.Instance.player.itemList.GetItemAt(4);
            case "stone_gold":
                return PlayerController.Instance.player.itemList.GetItemAt(5);
            case "stone_diamond":
                return PlayerController.Instance.player.itemList.GetItemAt(6);
        }

        return null;
    }
}