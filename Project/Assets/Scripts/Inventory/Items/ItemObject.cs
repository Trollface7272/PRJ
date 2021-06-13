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
        Utility,
        Vanity,
        Station
    }

    public enum ArmorType {
        Helmet = 0,
        Chestplate = 1,
        Pants = 2,
        Utility = 3,
    }

    public enum WeaponType {
        Sword = 0,
    }

    public enum StationType {
        Crafting = 1 << 1,
        Smelting = 1 << 2,
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

        public abstract void Clicked();
    }
}