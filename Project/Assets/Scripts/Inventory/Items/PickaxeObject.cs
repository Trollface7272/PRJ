using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Pickaxe")]
    public class PickaxeObject : ItemObject {
        public int miningPower;
        private void Awake() {
            type = ItemType.Pickaxe;
            maxStack = 1;
        }
    }
}