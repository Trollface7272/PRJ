using Entity.Player;
using UnityEngine;
using UnityEngine.Tilemaps;
using World;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Block Object", menuName = "Inventory System/Items/Block")]
    public class BlockObject : ItemObject {
        public Tile tile;
        public int hardness;
        private void Awake() {
            type = ItemType.Block;
            maxStack = 999;
        }

        public override void Clicked() {
            var map = MapController.Instance;
            var player = PlayerController.Instance.player;
            if (!tile) return;
            if (!map.EditBlock(tile, false)) return;
            player.inventory.RemoveFromStack(player.activeSlot, 1);
        }
    }
}
