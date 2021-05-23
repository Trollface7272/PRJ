using UnityEngine;

namespace Entity.Inventory.Item {
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
        Utility
    }
    public abstract class ItemObject : ScriptableObject {
        public int id;
        public new string name;
        public Sprite sprite;
        public ItemType type;
        public int maxStack;
        [TextArea(15,20)]
        public string description;
    }
}