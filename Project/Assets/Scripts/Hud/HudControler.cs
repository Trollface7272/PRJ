using System.Collections.Generic;
using System.Linq;
using Entity.Player;
using UnityEngine;
using TMPro;
using Inventory;
using Inventory.Items;
using Inventory.Crafting;
using UnityEngine.Playables;

namespace Hud {
    public class HudControler : MonoBehaviour {
        private static HudControler _instance;
        public static HudControler Instance {
            get {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                if (!_instance) _instance = FindObjectOfType<HudControler>();
                return _instance;
            }
        }
        private PlayerController _pc;
        private Bar _health;
        private Bar _mana;

        [SerializeField] private GameObject hotbar;
        [SerializeField] private GameObject[] rows;
        [SerializeField] private GameObject cursor;
        [SerializeField] private GameObject crafting;
        [SerializeField] private GameObject armor;
        [SerializeField] private GameObject vanity;
        [SerializeField] private new Camera camera;
        [SerializeField] private GameObject toolTip;

        private SpriteRenderer[] _hotbarSr;
        private SpriteRenderer[] _hotbarBg;
        private TextMeshProUGUI[] _hotbarTxt;
        private SpriteRenderer _curSr;
        private const int RowCount = 3;
        private const int SlotCount = 6;
        private PlayerObject _player;
        private int _craftingOffset = 0;
        
        private SpriteRenderer[,] _invSr;
        private TextMeshProUGUI[,] _invTxt;

        private SpriteRenderer[] _armSr;
        private SpriteRenderer[] _vanSr;

        private SpriteRenderer[] _craftSr;
        private Recipe[] _craftRc;

        private SpriteRenderer[,] _incSr;
        private TextMeshProUGUI[,] _incTxt;

        private TextMeshProUGUI _ttTxtTitle;
        private TextMeshProUGUI _ttTxtDesc;

        public bool IsInvVisible { get; private set; } = true;

        private void Start() {
            _pc = PlayerController.Instance;
            var bars = transform.Find("Bars");
            _health = bars.Find("Health").GetComponent<Bar>();
            _mana = bars.Find("Mana").GetComponent<Bar>();
            HideToolTip();
            _player = PlayerController.Instance.player;
            _hotbarSr = new SpriteRenderer[SlotCount];
            _hotbarBg = new SpriteRenderer[SlotCount];
            _hotbarTxt = new TextMeshProUGUI[SlotCount];
            _invSr = new SpriteRenderer[RowCount,SlotCount];
            _invTxt = new TextMeshProUGUI[RowCount,SlotCount];
            _armSr = new SpriteRenderer[7];
            _vanSr = new SpriteRenderer[7];
            _craftSr = new SpriteRenderer[5];
            _craftRc = new Recipe[5];
            _incSr = new SpriteRenderer[5, 10];
            _incTxt = new TextMeshProUGUI[5, 10];
            
            for (var i = 0; i < SlotCount; i++) { //Load Hotbar Elements
                _hotbarBg[i] = hotbar.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>();
                _hotbarSr[i] = hotbar.transform.GetChild(i).GetChild(1).GetComponent<SpriteRenderer>();
                _hotbarTxt[i] = hotbar.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();
            }
            
            for (var i = 0; i < RowCount; i++) { //Load inventory elements
                for (var j = 0; j < SlotCount; j++) {
                    _invSr[i,j] = rows[i].transform.GetChild(j).GetChild(1).GetComponent<SpriteRenderer>();
                    _invTxt[i,j] = rows[i].transform.GetChild(j).GetChild(2).GetComponent<TextMeshProUGUI>();
                }
            }
            
            for (var i = 0; i < _player.armor.length; i++) { //Load vanity and armor elements
                _armSr[i] = armor.transform.GetChild(i).GetChild(1).GetComponent<SpriteRenderer>();
                _vanSr[i] = vanity.transform.GetChild(i).GetChild(1).GetComponent<SpriteRenderer>();
            }

            for (var i = 0; i < 5; i++) { //Load crafting elements
                _craftSr[i] = crafting.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>();
                _craftRc[i] = crafting.transform.GetChild(i).GetChild(0).GetComponent<Recipe>();
            }
            
            for (var i = 0; i < 5; i++) { //Load crafting ingredients elements
                for (var j = 0; j < 10; j++) {
                    _incSr[i, j] = crafting.transform.GetChild(i).GetChild(1).GetChild(j).GetChild(1).GetComponent<SpriteRenderer>();
                    _incTxt[i, j] = crafting.transform.GetChild(i).GetChild(1).GetChild(j).GetChild(2).GetComponent<TextMeshProUGUI>();
                }
            }
            
            
            _curSr = cursor.transform.GetChild(0).GetComponent<SpriteRenderer>();
            _ttTxtTitle = toolTip.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _ttTxtDesc = toolTip.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            
            
            UpdateHud();
            ToggleInv();
        }

        private void Update() {
            if (_player.cursor.item) {
                var pos = camera.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                cursor.transform.position = pos;
            }

            if (toolTip.activeSelf) {
                var pos = camera.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                pos.x += 0.2f;
                pos.y += 0.2f;
                toolTip.transform.position = pos;
            }
        } 

        public void UpdateBars() {
            _health.UpdateBar(_player.CurrentHealth / _player.MaxHealth);
            _mana.UpdateBar(_player.CurrentMana / _player.MaxMana);
        }

        public void UpdateHud() {
            UpdateHotbar();
            UpdateInventory();
            UpdateCursor();
            UpdateArmor();
            UpdateVanity();
            if (IsInvVisible) {
                _player.CheckForRecipes();
                UpdateCrafting();
            }
        }

        private void UpdateHotbar() {
            for (var i = 0; i < SlotCount; i++) {
                var item = _player.inventory.GetItemAt(i);
                _hotbarTxt[i].text = "";
                if (!item) {
                    _hotbarSr[i].sprite = null;
                    _hotbarTxt[i].text = "";
                    continue;
                }
                _hotbarSr[i].sprite = item.sprite;
                _hotbarTxt[i].text = item.stackable ? _player.inventory.GetStackAt(i).ToString() : "";
            }
        }

        private void UpdateInventory() {
            for (var j = 0; j< rows.Length; j++) {
                for (var i = 0; i < SlotCount; i++) {
                    var item = _player.inventory.GetItemAt(SlotCount * (j + 1) + i);
                    if (!item) {
                        _invSr[j, i].sprite = null;
                        _invTxt[j, i].text = "";
                        continue;
                    }
                    _invSr[j, i].sprite = item.sprite;
                    _invTxt[j, i].text = item.stackable ? _player.inventory.GetStackAt(SlotCount * (j + 1) + i).ToString() : "";
                }
            }
        }

        private void UpdateCursor() {
            if (!_player.cursor.item) {
                cursor.gameObject.SetActive(false);
                return;
            }
            cursor.gameObject.SetActive(true);
            _curSr.sprite = _player.cursor.item.sprite;
        }

        private void UpdateArmor() {
            for (var i = 0; i < _player.armor.length; i++) {
                var item = _player.armor.GetItemAt(i);
                _armSr[i].sprite = item ? item.sprite : null;
            }
        }

        private void UpdateVanity() {
            for (var i = 0; i < _player.vanity.length; i++) {
                var item = _player.vanity.GetItemAt(i);
                _vanSr[i].sprite = item ? item.sprite : null;
            }
        }

        private void UpdateCrafting() {
            var recObj = _player.itemList.recipes.Where(rcp => rcp.craftable).ToList();
            var index = 0;
            for (var i = _craftingOffset; i < recObj.Count && index < 5; i++, index++) {
                var recipe = recObj[i];
                if (!recipe.craftable) continue;
                _craftSr[index].sprite = recipe.result.sprite;
                _craftRc[index].RecipeId = recipe.id;

                for (var j = 0; j < 10; j++) {
                    if (!recipe || recipe.items.Length <= j ||
                        !recipe.items[j].item) {
                        _incSr[index, j].gameObject.transform.parent.gameObject.SetActive(false);
                        _incTxt[index, j].text = "";
                        continue;
                    }
                    _incSr[index, j].gameObject.transform.parent.gameObject.SetActive(true);
                    _incSr[index, j].sprite = recipe.items[j].item.sprite;
                    _incTxt[index, j].text = recipe.items[j].count.ToString();
                }
            }

            if (index < 5 && _craftingOffset > 0) {
                _craftingOffset += index - 5;
                if (_craftingOffset < 0) _craftingOffset = 0;
                UpdateCrafting();
                return;
            }
            for (; index < 5; index++) {
                _craftSr[index].sprite = null;
                _craftRc[index].RecipeId = -1;
                for (var j = 0; j < 10; j++) {
                    _incSr[index, j].gameObject.transform.parent.gameObject.SetActive(false);
                    _incTxt[index, j].text = "";
                }
            }
        }

        public void ToggleInv() {
            _player.CheckForRecipes();
            UpdateCrafting();
            IsInvVisible = !IsInvVisible;
            crafting.SetActive(IsInvVisible);
            armor.SetActive(IsInvVisible);
            vanity.SetActive(IsInvVisible);
            foreach (var row in rows) {
                row.SetActive(IsInvVisible);
            }
        }

        public void UpdateSlot() {
            for (var i = 0; i < SlotCount; i++) {
                _hotbarBg[i].color = (i == _player.activeSlot) ? Color.red : new Color(0, 1, 0.3764529f, 0.3764529f);
            }
        }

        public void ItemHovered(int index) {
            var inv = PlayerController.Instance.player.inventory.items;
            if (inv[index].item == null) return; 
            _ttTxtTitle.text = inv[index].item.name;
            _ttTxtDesc.text = inv[index].item.description;
            toolTip.SetActive(true);
        }

        public void VanityHovered(int index) {
            var inv = PlayerController.Instance.player.vanity.items;
            if (inv[index].item == null) return;
            _ttTxtTitle.text = inv[index].item.name;
            _ttTxtDesc.text = inv[index].item.description;
            toolTip.SetActive(true);
        }

        public void ArmorHovered(int index) {
            var inv = PlayerController.Instance.player.armor.items;
            if (inv[index].item == null) return;
            _ttTxtTitle.text = inv[index].item.name;
            _ttTxtDesc.text = inv[index].item.description;
            toolTip.SetActive(true);
        }
        public void RecipeHovered(ItemObject itm) {
            _ttTxtTitle.text = itm.name;
            _ttTxtDesc.text = itm.description;
            toolTip.SetActive(true);
        }
        
        public void IngredientHovered(ItemObject itm) {
            _ttTxtTitle.text = itm.name;
            _ttTxtDesc.text = itm.description;
            toolTip.SetActive(true);
        }

        public void HideToolTip() {
            toolTip.SetActive(false);
        }
        
        public void AddCraftOffset(int offset) {
            if (_craftingOffset + offset < 0) return;
            var count = _player.itemList.recipes.Count(rcp => rcp.craftable);

            if (count - 5 < _craftingOffset + offset) return;
            _craftingOffset += offset;
            UpdateCrafting();
        }
    }
}
