using Entity.Player;
using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Sword Object", menuName = "Inventory System/Items/Sword")]
    public class SwordObject : WeaponObject {
        public int swingDelay;
        private void Awake() {
            weaponType = WeaponType.Sword;
            swingDelay = 500;
            damage = 1;
        }

        public override void Clicked() {
            PlayerController.Instance.SwingSword();
        }
    }
}