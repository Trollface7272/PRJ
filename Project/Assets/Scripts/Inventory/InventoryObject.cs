using System.Collections.Generic;
using System.Linq;
using Inventory.Items;
using UnityEditor;
using UnityEngine;
using Entity.Player;
using Hud;

namespace Inventory {
    [CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject {
        public int length;
        public List<InventorySlot> items = new List<InventorySlot>();

        public void AddItem(ItemObject item, int count) {
            var currentCount = CountItem(item);
            if (currentCount == -1) {

                var emptySlot = FindFirstEmptySlot();
                if (emptySlot == -1) return;

                items[emptySlot] = new InventorySlot(item, count);
            }
            else if (item.maxStack >= currentCount + count) {
                AddItemToStack(item, count);
            }
            else {
                var c = item.maxStack - currentCount;

                var emptySlot = FindFirstEmptySlot();
                if (emptySlot == -1) return;

                items[emptySlot] = new InventorySlot(item, count);
                AddItemToStack(item, c);
                items[FindFirstEmptySlot()] = new InventorySlot(item, count - c);
            }
            PlayerController.Instance.player.CheckForRecipes();
            HudControler.Instance.UpdateHud();
        }

        private int FindFirstEmptySlot() {
            for (var i = 0; i < items.Count; i++)
                if (!items[i].item)
                    return i;
            return -1;
        }

        public int CountItem(ItemObject item) {
            foreach (var slot in items)
                if (slot != null && slot.item && slot.item.id == item.id && item.maxStack > slot.count)
                    return slot.count;
            return -1;
        }

        private void AddItemToStack(ItemObject item, int count) {
            foreach (var slot in items) {
                if (slot.item != item || slot.count + count >= item.maxStack) continue;
                slot.count += count;
                break;
            }
        }

        public ItemObject GetItemAt(int index) {
            return items[index] != null ? items[index].item : null;
        }

        public void RemoveFromStack(int index, int count) {
            items[index].count -= count;
            if (items[index].count <= 0) ClearSlot(index);
            HudControler.Instance.UpdateHud();
        }

        private void ClearSlot(int index) {
            items[index].item = null;
            items[index].count = 0;
        }

        public int GetStackAt(int index) {
            return items[index].count;
        }

        public bool RemoveItems(ItemObject item, int count) {
            if (CountItem(item) < count) return false;
            foreach (var itm in items.Where(itm => item && itm.item && item.id == itm.item.id)) {
                if (itm.count > count) {
                    itm.count -= count;
                    break;
                }

                if (itm.count < count) {
                    itm.item = null;
                    break;
                }

                itm.item = null;
                count -= itm.count;
            }
            HudControler.Instance.UpdateHud();
            return true;
        }
    }

    [System.Serializable]
    public class InventorySlot {
        public ItemObject item;
        public int count;

        public InventorySlot(ItemObject item, int count) {
            this.item = item;
            this.count = count;
        } 
    }
}