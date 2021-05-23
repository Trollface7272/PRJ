using Entity.Inventory.Item;
using UnityEngine;

namespace Entity.ObjectLists {
    [CreateAssetMenu(fileName = "Tiles", menuName = "Lists/Tiles")]
    public class Tiles : ScriptableObject {
        public BlockObject[] items;
    }
}