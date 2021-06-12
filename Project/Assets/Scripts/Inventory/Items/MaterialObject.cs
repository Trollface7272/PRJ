using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Material")]
    public class MaterialObject : ItemObject {
        private void Awake() {
            type = ItemType.Material;
        }

        public override void Clicked() { }
    }
}