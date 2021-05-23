using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Crafting {
    [CreateAssetMenu(fileName = "Crafting", menuName = "Inventory System/Crafting")]
    public class CraftingObject : ScriptableObject {
        [SerializeField] public List<RecipeObject> recipes;
        [NonSerialized]  public bool craftable = false;
    }
}