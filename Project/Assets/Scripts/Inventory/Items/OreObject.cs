using UnityEngine;

namespace Inventory.Items {
    [CreateAssetMenu(fileName = "New Ore Object", menuName = "Inventory System/Items/Ore")]
    public class OreObject : ScriptableObject {
        public BlockObject block;
        public float spawnChance;
        public int seedOffset;
        public Vector2 spawn;
    }
}