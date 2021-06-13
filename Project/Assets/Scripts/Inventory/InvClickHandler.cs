using UnityEngine;
using UnityEngine.EventSystems;
using Hud;
using Entity.Player;

namespace Inventory {
    public class InvClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

        public void OnClick() {
            if (transform.parent.name == "Ingredient" || transform.parent.name == "Crafting") return;
            var pos = InventoryUtils.GetRowAndSlot(transform.parent, transform);
            HudControler.Instance.HideToolTip();
            switch (pos[0]) {
                case 0:
                case 1:
                case 2:
                case 3:
                    PlayerController.Instance.player.ItemClicked(pos[0] * 6 + pos[1]);
                    break;
                case 4:
                    PlayerController.Instance.player.VanityClicked(pos[1]);
                    break;
                case 5:
                    PlayerController.Instance.player.ArmorClicked(pos[1]);
                    break;
                case 12:
                    break;
                case 13:
                    break;
            }
            if (!PlayerController.Instance.player.cursor.item) OnPointerEnter(null);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (!HudControler.Instance.IsInvVisible || PlayerController.Instance.player.cursor.item) return;
            var pos = InventoryUtils.GetRowAndSlot(transform.parent, transform);
            switch (pos[0]) {
                case 0:
                case 1:
                case 2:
                case 3:
                    HudControler.Instance.ItemHovered(pos[0] * 6 + pos[1]);
                    break;
                case 4:
                    HudControler.Instance.VanityHovered(pos[1]);
                    break;
                case 5:
                    HudControler.Instance.ArmorHovered(pos[1]);
                    break;
                case 12:
                    var recipeId = transform.GetComponent<Recipe>().RecipeId;
                    if (recipeId < 0) return;
                    var recipe = PlayerController.Instance.player.itemList.recipes[recipeId-1].result;
                    if (!recipe) return;
                    HudControler.Instance.RecipeHovered(recipe);
                    break;
                case 13:
                    var item = PlayerController.Instance.player.itemList.recipes[transform.parent.parent.GetChild(0).GetComponent<Recipe>().RecipeId-1].items[pos[1]].item;
                    if (!item) return;
                    HudControler.Instance.IngredientHovered(item);
                    break;
                default:
                    break;
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            HudControler.Instance.HideToolTip();
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            var pos = InventoryUtils.GetRowAndSlot(gameObject.transform.parent.gameObject.transform, transform);
            HudControler.Instance.HideToolTip();
            switch (pos[0]) {
                case 0:
                case 1:
                case 2:
                case 3:
                    PlayerController.Instance.player.DropItem(pos[0] * 6 + pos[1]);
                    break;
                case 4:
                    //PlayerController.Instance.player.VanityClicked(pos[1]);
                    break;
                case 5:
                    //PlayerController.Instance.player.ArmorClicked(pos[1]);
                    break;
            }
        }
    }
}
