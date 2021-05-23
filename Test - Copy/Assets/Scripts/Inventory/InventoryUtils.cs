using System;
using Inventory.Crafting;
using UnityEngine;

public static class InventoryUtils {
    public static int[ /*row,slot*/] GetRowAndSlot(Transform rowObj, Transform slotObj) {
        int row;
        switch (rowObj.name) {
            case "Hotbar":
                row = 0;
                break;
            case "Row1":
                row = 1;
                break;
            case "Row2":
                row = 2;
                break;
            case "Row3":
                row = 3;
                break;
            case "Vanity":
                row = 4;
                break;
            case "Armor":
                row = 5;
                break;
            default:
                row = 0;
                break;
        }

        var slot = Convert.ToInt32(slotObj.name.Substring(slotObj.name.Length - 1)) - 1;
        return new [] {row, slot};
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