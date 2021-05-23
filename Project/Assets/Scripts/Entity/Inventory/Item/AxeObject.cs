using UnityEngine;

namespace Entity.Inventory.Item {
    [CreateAssetMenu(fileName = "Axe", menuName = "Items/Axe")]
    public class AxeObject : ItemObject {
        private void Awake() {
            type = ItemType.Axe;
            maxStack = 1;
        }
    }
}