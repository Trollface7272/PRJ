using System;
using System.Collections.Generic;
using Hud;
using Inventory.Crafting;
using Inventory.Items;
using UnityEngine;
using World;

namespace Inventory {
    [CreateAssetMenu(fileName = "New Player", menuName = "Inventory System/Player")]
    public class PlayerObject : ScriptableObject {
        public InventoryObject inventory;
        public InventoryObject armor;
        public InventoryObject vanity;
        public InventoryObject itemList;
        public CraftingObject recipes;
        public InventorySlot cursor = new InventorySlot(null, 0);
        public GameObject droppedItemPrefab;
        
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
                        Hud.Instance.UpdateHud();
                        return;
                    }

                    cursor.count = inventory.items[slot].count + cursor.count - cursor.item.maxStack;
                    inventory.items[slot].count = inventory.items[slot].item.maxStack;
                    Hud.Instance.UpdateHud();
                    return;
                }

                var temp = cursor;
                cursor = inventory.items[slot];
                inventory.items[slot] = temp;
            }

            Hud.Instance.UpdateHud();
        }

        public void VanityClicked(int slot) {
            var item = cursor.item;
            var equipped = vanity.items[slot].item;
            if (!item && equipped) {
                cursor.item = vanity.items[slot].item;
                vanity.items[slot] = new InventorySlot(null, 0);
                Hud.Instance.UpdateHud();
                return;
            }
            if (!item || (slot != 0 || item.type != ItemType.Helmet) && (slot != 1 || item.type != ItemType.Armor) &&
                (slot != 2 || item.type != ItemType.Pants) && (slot <= 2 || item.type != ItemType.Armor) ||
                slot > 6) return;
            if (equipped &&item) {
                var temp = cursor.item;
                cursor.item = vanity.items[slot].item;
                vanity.items[slot].item = temp;
            } else if (item) {
                vanity.items[slot].item = cursor.item;
                cursor.item = null;
            }
            Hud.Instance.UpdateHud();
        }

        public void ArmorClicked(int slot) {
            var item = cursor.item;
            var equipped = armor.items[slot].item;
            if (!item && equipped) {
                cursor.item = armor.items[slot].item;
                armor.items[slot] = new InventorySlot(null, 0);
                Hud.Instance.UpdateHud();
                return;
            }
            if (!item || (slot != 0 || item.type != ItemType.Helmet) && (slot != 1 || item.type != ItemType.Armor) &&
                (slot != 2 || item.type != ItemType.Pants) && (slot <= 2 || item.type != ItemType.Armor) ||
                  slot > 6) return;
            if (equipped &&item) {
                var temp = cursor.item;
                cursor.item = armor.items[slot].item;
                armor.items[slot].item = temp;
            } else if (item) {
                armor.items[slot].item = cursor.item;
                cursor.item = null;
            }
            Hud.Instance.UpdateHud();
        }

        public void CheckForRecipes() {
            foreach (var recipe in recipes.recipes) {
                recipe.craftable = true;
                foreach (var item in recipe.items) {
                    if (inventory.CountItem(item.item) >= item.count) continue;
                    recipe.craftable = false;
                    break;
                }
            }
            Hud.Instance.UpdateHud();
        }

        public void Craft(RecipeObject recipe) {
            if (cursor.item && cursor.item != recipe.result || (cursor.item == recipe.result && !recipe.result.stackable)) return;
            foreach (var item in recipe.items) {
                inventory.RemoveItems(item.item, item.count);
            }
            cursor.item = recipe.result;
            cursor.count += 1;
            Hud.Instance.UpdateHud();
        }

        public void DropItem(int slot) {
            var item = inventory.items[slot];
            var itemObj = Instantiate(droppedItemPrefab, PlayerController.Instance.transform.position, Quaternion.identity);
            var pickHandler = itemObj.GetComponent<PickupHandler>();
            var rigidBody = itemObj.GetComponent<Rigidbody2D>();
            
            rigidBody.AddForce(new Vector2(20,2));
            pickHandler.pickProtection = (int) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 1;
            pickHandler.item = item.item;
            pickHandler.count = item.count;

            inventory.items[slot].item = null;
            Hud.Instance.UpdateHud();
        }
    }
}