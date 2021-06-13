using Entity.Player;
using UnityEngine;
using World;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Pickaxe")]
    public class PickaxeObject : SwordObject {
        public int miningPower;
        private void Awake() {
            type = ItemType.Pickaxe;
            maxStack = 1;
        }

        public override void Clicked() {
            PlayerController.Instance.SwingSword();
            var t = MapController.Instance.CursorHasTileOrBackground();
            switch (t) {
                case 0:
                    return;
                case 1:
                    EditBackground();
                    break;
                case 2:
                    EditMap();
                    break;
            }
        }

        private void EditMap() {
            var map = MapController.Instance;
            var player = PlayerController.Instance.player;
            var tile = map.TileAtCursor();
            if (!tile) return;
            var block = ItemUtils.TileToItem(tile);
            if (!ItemUtils.IsBreakable(this, block)) return;
            switch (block.name) {
                case "Coal Ore":
                    player.inventory.AddItem(player.itemList.FindItemByName("Coal"), 1); break;
                case "Diamond Ore": 
                    player.inventory.AddItem(player.itemList.FindItemByName("Diamond"), 1); break;
                default: 
                    player.inventory.AddItem(block, 1); break;
            }
            map.EditBlock(null, true);
        }

        private void EditBackground() {
            var map = MapController.Instance;
            var player = PlayerController.Instance.player;
            var tile = map.TileAtCursor();
            if (!tile) return;
            var block = ItemUtils.TileToItem(tile);
            if (!ItemUtils.IsBreakable(this, block)) return;
            player.inventory.AddItem(block, 1);
            map.EditNonSolid(null, true);
        }
    }
}