using Entity.Player;
using Hud;
using UnityEngine;
using UnityEngine.Tilemaps;
using World;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Station Object", menuName = "Inventory System/Items/Station")]
    public class StationObject : ItemObject {
        public StationType stationType;
        public Tile tile;
        public int hardness;
        private void Awake() {
            maxStack = 1;
            type = ItemType.Station;
        }

        public override void Clicked() {
            var map = MapController.Instance;
            var player = PlayerController.Instance.player;
            tile = map.TileAtCursor();
            if (tile) return;
            map.EditBlock(tile, false);
            player.inventory.RemoveFromStack(player.activeSlot, 1);
        }
    }
}
