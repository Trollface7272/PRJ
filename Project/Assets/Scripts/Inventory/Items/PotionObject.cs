using Entity.Player;
using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Potion")]
    public class PotionObject : ItemObject {
        public int healing;
        private void Awake() {
            type = ItemType.Potion;
            maxStack = 99;
        }

        public override void Clicked() {
            var player = PlayerController.Instance.player;
            if (player.CurrentHealth >= player.MaxHealth) return;
            player.CurrentHealth += healing;
            player.inventory.RemoveFromStack(player.activeSlot, 1);
        }
    }
}
