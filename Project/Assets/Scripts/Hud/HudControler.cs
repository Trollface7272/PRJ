using Entity.Player;
using UnityEngine;
using TMPro;
using Inventory;
using Inventory.Items;
using Inventory.Crafting;

namespace Hud {
    public class HudControler : MonoBehaviour {
        private static HudControler _instance;
        public static HudControler Instance {
            get {
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
        
        private SpriteRenderer[,] _invSr;
        private TextMeshProUGUI[,] _invTxt;

        private SpriteRenderer[] _armSr;
        private SpriteRenderer[] _vanSr;

        private SpriteRenderer[] _craftSr;

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
                _craftSr[i] = crafting.transform.GetChild(i).GetChild(1).GetComponent<SpriteRenderer>();
            }
            
            for (var i = 0; i < 5; i++) { //Load crafting ingredients elements
                for (var j = 0; j < 10; j++) {
                    _incSr[i, j] = crafting.transform.GetChild(i).GetChild(3).GetChild(j).GetChild(1).GetComponent<SpriteRenderer>();
                    _incTxt[i, j] = crafting.transform.GetChild(i).GetChild(3).GetChild(j).GetChild(2).GetComponent<TextMeshProUGUI>();
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
            _health.UpdateBar(_pc.CurrentHealth / _pc.MaxHealth);
            _mana.UpdateBar(_pc.CurrentMana / _pc.MaxMana);
        }

        public void UpdateHud() {
            UpdateHotbar();
            UpdateInventory();
            UpdateCursor();
            UpdateArmor();
            UpdateVanity();
            if (IsInvVisible)
                UpdateCrafting();
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
            var index = 0;
            for (var i = 0; i < 5; i++) {
                RecipeObject recipe = null;
                if (i < _player.recipes.recipes.Count) {
                    recipe = _player.recipes.recipes[i];

                    if (!recipe.craftable) continue;

                    _craftSr[index].sprite = recipe.result.sprite;
                } else {
                    _craftSr[index].sprite = null;
                }
                for (var j = 0; j < 10; j++) {
                    if (!recipe || recipe.items.Length <= j ||
                        !recipe.items[j].item) {
                        _incSr[i, j].gameObject.transform.parent.gameObject.SetActive(false);
                        _incTxt[i, j].text = "";
                        continue;
                    }
                    _incSr[i, j].gameObject.transform.parent.gameObject.SetActive(true);
                    _incSr[i, j].sprite = recipe.items[j].item.sprite;
                    _incTxt[i, j].text = recipe.items[j].count.ToString();
                }
                index++;
            }
        }

        public void ToggleInv() {
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

        public void HideToolTip() {
            toolTip.SetActive(false);
        }
    }
}
