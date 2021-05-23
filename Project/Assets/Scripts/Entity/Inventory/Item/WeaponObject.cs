using UnityEngine;

namespace Entity.Inventory.Item {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon")]
    public class WeaponObject : ItemObject {
        private void Awake() {
            type = ItemType.Weapon;
            maxStack = 1;
        }
    }
}