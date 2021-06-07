using UnityEngine;
using UnityEngine.Tilemaps;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Block Object", menuName = "Inventory System/Items/Block")]
    public class BlockObject : ItemObject {
        public Tile tile;
        public int hardness;
        private void Awake() {
            type = ItemType.Block;
            maxStack = 999;
        }
    }
}
