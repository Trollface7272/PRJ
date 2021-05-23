using UnityEngine;

namespace Inventory.Items {
    public enum ItemType {
        Weapon,
        Pickaxe,
        Axe,
        Armor,
        Potion,
        Block,
        Material,
        Consumable,
        Helmet,
        Pants,
        Utility,
        Vanity
    }
    public abstract class ItemObject : ScriptableObject {
        public int id;
        public new string name;
        public Sprite sprite;
        public ItemType type;
        public bool stackable;
        public int maxStack;
        [TextArea(15,20)]
        public string description;
    }
}