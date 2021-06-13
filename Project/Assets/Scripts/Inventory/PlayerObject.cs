using System;
using System.Linq;
using Hud;
using Inventory.Crafting;
using Inventory.Items;
using UnityEngine;
using World;
using Entity.Player;

namespace Inventory {
    [CreateAssetMenu(fileName = "New Player", menuName = "Inventory System/Player")]
    public class PlayerObject : ScriptableObject {
        public InventoryObject inventory;
        public InventoryObject armor;
        public InventoryObject vanity;
        public ItemListObject itemList;
        public InventorySlot cursor = new InventorySlot(null, 0);
        public GameObject droppedItemPrefab;
        [Range(0, 500)] public float baseHealth;
        [Range(0, 500)] public float bonusHealth;
        public float MaxHealth => baseHealth + bonusHealth;
        private float _currentHealth;
        public float CurrentHealth {
            get => _currentHealth;
            set {
                _currentHealth = Math.Min(value, MaxHealth);
                if (_currentHealth <= 0) HandleDeath();
                HudControler.Instance.UpdateBars();
            }
        }

        [Range(0, 500)] public float baseMana;
        [Range(0, 500)] public float bonusMana;
        public float MaxMana => baseMana + bonusMana;
        private float _currentMana;
        public float CurrentMana {
            get => _currentMana;
            set {
                _currentMana = Math.Min(value, MaxMana);
                HudControler.Instance.UpdateBars();
            }
        }

        public byte activeSlot;
        
        public void ItemClicked(int slot) {
            if (!HudControler.Instance.IsInvVisible) return;
            if (cursor.item == null) {
                cursor = inventory.items[slot];
                inventory.items[slot] = new InventorySlot(null, 0);
            } else {
                if (inventory.items[slot].item && 
                    inventory.items[slot].item.id == cursor.item.id &&
                    inventory.items[slot].count < inventory.items[slot].item.maxStack &&
                    cursor.count < cursor.item.maxStack) {
                    if (inventory.items[slot].count + cursor.count <= inventory.items[slot].item.maxStack) {
                        inventory.items[slot].count += cursor.count;
                        cursor.item = null;
                        cursor.count = 0;
                        HudControler.Instance.UpdateHud();
                        return;
                    }

                    cursor.count = inventory.items[slot].count + cursor.count - cursor.item.maxStack;
                    inventory.items[slot].count = inventory.items[slot].item.maxStack;
                    HudControler.Instance.UpdateHud();
                    return;
                }

                var temp = cursor;
                cursor = inventory.items[slot];
                inventory.items[slot] = temp;
            }

            HudControler.Instance.UpdateHud();
        }

        public void VanityClicked(int slot) {
            if (slot > 6 || slot < 0) return;
            var item = cursor.item;
            var equipped = armor.items[slot].item;
            if (!item && equipped) {
                cursor.item = armor.items[slot].item;
                armor.items[slot] = new InventorySlot(null, 0);
                HudControler.Instance.UpdateHud();
                return;
            }

            if (!item) return;
            if (item.type != ItemType.Vanity) return;
            var itm = (VanityObject) item;
            switch (slot) {
                case 0 when itm.armorType != ArmorType.Helmet:
                case 1 when itm.armorType != ArmorType.Chestplate:
                case 2 when itm.armorType != ArmorType.Pants:
                case 3 when itm.armorType != ArmorType.Utility:
                case 4 when itm.armorType != ArmorType.Utility:
                case 5 when itm.armorType != ArmorType.Utility:
                case 6 when itm.armorType != ArmorType.Utility:
                    return;
            }

            if (equipped &&itm) {
                var temp = cursor.item;
                cursor.item = armor.items[slot].item;
                armor.items[slot].item = temp;
            } else if (itm) {
                armor.items[slot].item = cursor.item;
                cursor.item = null;
            }
            HudControler.Instance.UpdateHud();
        }

        public void ArmorClicked(int slot) {
            if (slot > 6 || slot < 0) return;
            var item = cursor.item;
            var equipped = armor.items[slot].item;
            if (!item && equipped) {
                cursor.item = armor.items[slot].item;
                armor.items[slot] = new InventorySlot(null, 0);
                HudControler.Instance.UpdateHud();
                return;
            }

            if (!item) return;
            if (item.type != ItemType.Armor) return;
            var itm = (ArmorObject) item;
            switch (slot) {
                case 0 when itm.armorType != ArmorType.Helmet:
                case 1 when itm.armorType != ArmorType.Chestplate:
                case 2 when itm.armorType != ArmorType.Pants:
                case 3 when itm.armorType != ArmorType.Utility:
                case 4 when itm.armorType != ArmorType.Utility:
                case 5 when itm.armorType != ArmorType.Utility:
                case 6 when itm.armorType != ArmorType.Utility:
                    return;
            }

            if (equipped &&itm) {
                var temp = cursor.item;
                cursor.item = armor.items[slot].item;
                armor.items[slot].item = temp;
            } else if (itm) {
                armor.items[slot].item = cursor.item;
                cursor.item = null;
            }
            HudControler.Instance.UpdateHud();
        }
        
        public void DropItem(int slot) {
            var item = inventory.items[slot];
            if (!item.item) return;
            var itemObj = Instantiate(droppedItemPrefab, PlayerController.Instance.transform.position - new Vector3(0, -1, 0), Quaternion.identity);
            var pickHandler = itemObj.GetComponent<PickupHandler>();
            var rigidBody = itemObj.GetComponent<Rigidbody2D>();
            itemObj.transform.SetParent(GameObject.Find("Entities").transform.Find("Items"));
            
            rigidBody.AddForce(new Vector2(5,5), ForceMode2D.Impulse);
            pickHandler.PickProtection = (int) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 2;
            pickHandler.item = item.item;
            pickHandler.count = item.count;

            inventory.RemoveFromStack(slot, item.count);
            HudControler.Instance.UpdateHud();
        }

        public void CheckForRecipes() {
            foreach (var recipe in itemList.recipes.Where(recipe => recipe)) {
                recipe.craftable = true;
                foreach (var item in recipe.items) {
                    if (inventory.CountItem(item.item) >= item.count) continue;
                    recipe.craftable = false;
                    break;
                }
            }
        }

        public void Craft(RecipeObject recipe) {
            if (cursor.item && cursor.item != recipe.result || cursor.item == recipe.result && !recipe.result.stackable) return;
            foreach (var item in recipe.items) {
                inventory.RemoveItems(item.item, item.count);
            }
            cursor.item = recipe.result;
            cursor.count += 1;
            HudControler.Instance.UpdateHud();
        }
        
        public void Craft(int recipeId) {
            var recipe = itemList.recipes[recipeId];
            Craft(recipe);
        }

        private void HandleDeath() {
            _currentHealth = baseHealth;
            _currentMana = baseMana;
            PlayerController.Instance.TeleportToSpawn();
        }
        
        public bool UseMana(float val) {
            if (CurrentMana > val) return false;
            CurrentMana -= val;
            return true;
        }
    }
}