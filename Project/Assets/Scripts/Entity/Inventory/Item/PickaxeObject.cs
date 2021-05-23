using UnityEngine;

namespace Entity.Inventory.Item {
    [CreateAssetMenu(fileName = "Pickaxe", menuName = "Items/Pickaxe")]
    public class PickaxeObject : ItemObject {
        private void Awake() {
            type = ItemType.Pickaxe;
            maxStack = 1;
        }
    }
}