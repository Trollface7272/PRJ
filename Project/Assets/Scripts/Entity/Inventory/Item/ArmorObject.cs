using UnityEngine;

namespace Entity.Inventory.Item {
    [CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor")]
    public class ArmorObject : ItemObject {
        private void Awake() {
            type = ItemType.Armor;
            maxStack = 1;
        }
    }
}