using UnityEngine;
using UnityEngine.Tilemaps;

namespace Entity.Inventory.Item {
    [CreateAssetMenu(fileName = "Block", menuName = "Items/Block")]
    public class BlockObject : ItemObject {
        public Tile tile;
        private void Awake() {
            type = ItemType.Block;
            maxStack = 999;
        }
    }
}