using Entity.Player;
using UnityEngine;
using World;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Pickaxe")]
    public class PickaxeObject : ItemObject {
        public int miningPower;
        private void Awake() {
            type = ItemType.Pickaxe;
            maxStack = 1;
        }

        public override void Clicked() {
            var map = MapController.Instance;
            var player = PlayerController.Instance.player;
            var tile = map.TileAtCursor();
            if (!tile) return;
            var block = ItemUtils.TileToItem(tile);
            if (!ItemUtils.IsBreakable(this, block)) return;
            player.inventory.AddItem(block, 1);
            map.EditBlock(null, true);
        }
    }
}