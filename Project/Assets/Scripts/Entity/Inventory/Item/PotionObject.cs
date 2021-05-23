using UnityEngine;

namespace Entity.Inventory.Item {
    [CreateAssetMenu(fileName = "Potion", menuName = "Items/PotionO")]
    public class PotionObject : ItemObject {
        private void Awake() {
            type = ItemType.Potion;
            maxStack = 99;
        }
    }
}