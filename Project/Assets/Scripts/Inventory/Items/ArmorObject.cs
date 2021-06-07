using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
    public class ArmorObject : ItemObject {
        public int armor;
        private void Awake() {
            maxStack = 1;
        }
    }
}
