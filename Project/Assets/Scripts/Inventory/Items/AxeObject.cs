using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Weapon Object", menuName = "Inventory System/Items/Axe")]
    public class AxeObject : ItemObject {
        private void Awake() {
            type = ItemType.Axe;
            maxStack = 1;
        }

        public override void Clicked() {
            throw new System.NotImplementedException();
        }
    }
}