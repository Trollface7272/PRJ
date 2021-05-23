using UnityEngine;

namespace Entity.Inventory.Item {
    [CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
    public class ConsumableObject : ItemObject {
        private void Awake() {
            type = ItemType.Consumable;
            maxStack = 99;
        }
    }
}