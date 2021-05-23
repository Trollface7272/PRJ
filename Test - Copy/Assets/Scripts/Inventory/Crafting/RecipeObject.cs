using Inventory.Items;
using UnityEngine;

namespace Inventory.Crafting {
    [CreateAssetMenu(fileName = "Recipe", menuName = "Inventory System/Recipe")]
    public class RecipeObject : ScriptableObject {
        public int id;
        public InventorySlot[] items;
        public ItemObject result;
        public bool craftable = false;
    }
}
