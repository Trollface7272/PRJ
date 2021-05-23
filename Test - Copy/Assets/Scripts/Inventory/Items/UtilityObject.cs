using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Utility Object", menuName = "Inventory System/Items/Utility")]
    public class UtilityObject : ItemObject {
        private void Awake() {
            stackable = false;
            maxStack = 1;
        }
    }
}