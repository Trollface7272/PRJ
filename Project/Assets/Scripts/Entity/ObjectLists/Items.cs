using Entity.Inventory.Item;
using UnityEngine;

namespace Entity.ObjectLists {
    [CreateAssetMenu(fileName = "Items", menuName = "Lists/Items")]
    public class Items : ScriptableObject {
        public ItemObject[] items;
    }
}