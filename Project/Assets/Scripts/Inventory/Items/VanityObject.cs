using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Vanity Object", menuName = "Inventory System/Items/Vanity")]
    public class VanityObject : ItemObject {
        private void Awake() {
            stackable = false;
            maxStack = 1;
        }
    }
}