using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
    public class WeaponObject : ItemObject {
        public WeaponType weaponType;
        public int damage;
        private void Awake() {
            type = ItemType.Weapon;
            maxStack = 1;
        }

        public override void Clicked() {
            
        }
    }
}
