using Entity.Player;
using UnityEngine;

namespace Hud {
    public class Recipe : MonoBehaviour {
        public int RecipeId;

        public void Craft() {
            if (RecipeId == -1) return;
            PlayerController.Instance.player.Craft(RecipeId-1);
        }
    }
}