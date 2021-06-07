using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Potion")]
    public class PotionObject : ItemObject {
        public int healing;
        private void Awake() {
            type = ItemType.Potion;
            maxStack = 99;
        }
    }
}
