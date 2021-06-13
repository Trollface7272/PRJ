using Entity.Player;
using Hud;
using UnityEngine;
using UnityEngine.Tilemaps;
using World;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Station Object", menuName = "Inventory System/Items/Station")]
    public class StationObject : BlockObject {
        public StationType stationType;
        private void Awake() {
            maxStack = 1;
            type = ItemType.Station;
        }

        public override void Clicked() {
            var map = MapController.Instance;
            var player = PlayerController.Instance.player;
            var t = map.TileAtCursor();
            if (t) return;
            map.EditBlock(this,false);
            player.inventory.RemoveFromStack(player.activeSlot, 1);
        }
    }
}
