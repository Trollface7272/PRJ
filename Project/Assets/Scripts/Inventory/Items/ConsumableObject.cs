using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Consumable Object", menuName = "Inventory System/Items/Consumable")]
    public class ConsumableObject : ItemObject {
        private void Awake() {
            type = ItemType.Consumable;
            maxStack = 999;
        }

        public override void Clicked() {
            throw new System.NotImplementedException();
        }
    }
}
