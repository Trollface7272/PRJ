using System;
using Inventory.Crafting;
using UnityEngine;
using Entity.Player;

public static class InventoryUtils {
    public static int[ /*row,slot*/] GetRowAndSlot(Transform rowObj, Transform slotObj) {
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
            default:
                row = Convert.ToInt32(rowObj.name) - 1;
                slot = Convert.ToInt32(slotObj.name) - 1;
                break;
        }

        return new [] {row, slot};
    }

    private static int ParseArmorSlot(Transform slotObj) {
        switch (slotObj.name) {
            case "Helm":  return 0;
            case "Chest": return 1;
            case "Boots": return 2;
            case "Util1": return 3;
            case "Util2": return 4;
            case "Util3": return 5;
            case "Util4": return 6;
            default:      return 0;
        }
    }

    public static int ArmorToIndex(Transform slotObj) {
        return Convert.ToInt32(slotObj.name.Substring(slotObj.name.Length - 1)) - 1;
    }

    public static RecipeObject SlotToRecipe(Transform slotObj) {
        var index = Convert.ToInt32(slotObj.name.Substring(slotObj.name.Length - 1)) - 1;
        var counter = 0;
        foreach (var recipe in PlayerController.Instance.player.recipes.recipes) {
            if (index == counter) return recipe;
            if (recipe.craftable) counter++;
        }

        return null;
    }
}