using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory {
    public class InvClickHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

        public void OnClick() {
            var pos = InventoryUtils.GetRowAndSlot(gameObject.transform.parent.gameObject.transform, transform);
            Hud.Instance.HideToolTip();
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
            }
            if (!PlayerController.Instance.player.cursor.item) OnPointerEnter(null);
        }

        public void CraftOnClick() {
            PlayerController.Instance.player.Craft(InventoryUtils.SlotToRecipe(transform));
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (!Hud.Instance.IsInvVisible || PlayerController.Instance.player.cursor.item) return;
            var pos = InventoryUtils.GetRowAndSlot(gameObject.transform.parent.gameObject.transform, transform);
            switch (pos[0]) {
                case 0:
                case 1:
                case 2:
                case 3:
                    Hud.Instance.ItemHovered(pos[0] * 6 + pos[1]);
                    break;
                case 4:
                    Hud.Instance.VanityHovered(pos[1]);
                    break;
                case 5:
                    Hud.Instance.ArmorHovered(pos[1]);
                    break;
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            Hud.Instance.HideToolTip();
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            var pos = InventoryUtils.GetRowAndSlot(gameObject.transform.parent.gameObject.transform, transform);
            Hud.Instance.HideToolTip();
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
