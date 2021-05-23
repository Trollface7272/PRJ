using UnityEngine;

namespace Entity.Inventory.Item {
    [CreateAssetMenu(fileName = "Material", menuName = "Items/Material")]
    public class MaterialObject : ItemObject {
        private void Awake() {
            type = ItemType.Material;
            maxStack = 999;
        }
    }
}