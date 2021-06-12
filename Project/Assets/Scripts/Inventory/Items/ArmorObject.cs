using Entity.Player;
using Hud;
using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
    public class ArmorObject : ItemObject {
        public int armor;
        public ArmorType armorType;
        private void Awake() {
            maxStack = 1;
        }

        public override void Clicked() {
            var player = PlayerController.Instance.player;
            player.inventory.items[player.activeSlot].item = player.armor.items[0].item;
            player.armor.items[(int)armorType].item = this;
            HudControler.Instance.UpdateHud();
        }
    }
}
