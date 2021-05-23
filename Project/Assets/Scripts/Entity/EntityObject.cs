using Entity.Inventory;
using UnityEngine;

namespace Entity {
    [CreateAssetMenu(fileName = "Entity", menuName = "Entity/Entity")]
    public class EntityObject : ScriptableObject {
        public InventoryObject inventory;
        public InventoryObject armor;
        public InventoryObject vanity;
    }
}