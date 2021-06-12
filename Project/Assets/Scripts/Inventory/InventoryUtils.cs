using System;
using Entity.Player;
using Inventory.Crafting;
using UnityEngine;

namespace Inventory {
    public static class InventoryUtils {
        public static int[ /*row,slot*/] GetRowAndSlot(Transform rowObj, Transform slotObj) {
            if (slotObj.name == "Result") {
                slotObj = slotObj.parent;
                rowObj = rowObj.parent;
            }
            int row, slot;
            switch (rowObj.name) {
                case "Vanity":
                    row = 4;
                    slot = ParseArmorSlot(slotObj);
                    break;
                case "Armor":
                    row = 5;
                    slot = ParseArmorSlot(slotObj);
                    break;
                case "Crafting":
                    row = 12;
                    slot = Convert.ToInt32(slotObj.name) - 1;
                    break;
                case "Ingredients":
                    row = 13;
                    slot = Convert.ToInt32(slotObj.name) - 1;
                    break;
                default:
                    try {
                        row = Convert.ToInt32(rowObj.name) - 1;
                        slot = Convert.ToInt32(slotObj.name) - 1;
                    } catch (FormatException e) {
                        Debug.Log("error in GetRowAndSlot " + rowObj.name + " - " + slotObj.name);
                        row = -1;
                        slot = -1;
                    }
                    break;
            }
            return new [] {row, slot};
        }

        private static int ParseArmorSlot(Transform slotObj) {
            return slotObj.name switch {
                "Helm" => 0,
                "Chest" => 1,
                "Boots" => 2,
                "Util1" => 3,
                "Util2" => 4,
                "Util3" => 5,
                "Util4" => 6,
                _ => 0
            };
        }

        public static int ArmorToIndex(Transform slotObj) {
            return Convert.ToInt32(slotObj.name.Substring(slotObj.name.Length - 1)) - 1;
        }
    }
}