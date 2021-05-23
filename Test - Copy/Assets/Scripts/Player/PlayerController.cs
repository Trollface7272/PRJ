using System;
using Inventory;
using Inventory.Items;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {
    [SerializeField] private int baseHealth = 100;
    [SerializeField] private int baseMana = 50;
    [SerializeField] private int baseDamage = 1;
    [SerializeField] private Hud hud;
    [SerializeField] private Tilemap map;
    [SerializeField] public PlayerObject player;

    private int BonusHealth {
        get => _bonusHealth;
        set { _bonusHealth = value; UpdateHealthBar(); }
    }

    private int BonusMana {
        get => _bonusMana;
        set { _bonusMana = value; UpdateManaBar(); }
    }

    public int bonusDamage;

    public static PlayerController Instance { get; private set; }


    private int CurrentHealth {
        get => _currentHealth;
        set { _currentHealth = value; UpdateHealthBar(); }
    }

    private int CurrentMana {
        get => _currentMana;
        set { _currentMana = value; UpdateManaBar();}
    }

    private int MaxHealth => baseHealth + _bonusHealth;

    public int Armor { get; set; } = 0;

    private MapController _mapController;
    private int _currentHealth;
    private int _currentMana;
    private int _bonusHealth;
    private int _bonusMana;

    public Vector3 spawnPoint;
    private void Start() {
        Instance = GetComponent<PlayerController>();
        _currentMana = baseHealth + BonusHealth;
        _currentMana = baseMana + BonusMana;
        _mapController =  MapController.Instance;
        spawnPoint = transform.position;
    }

    private void Update() {
        if (CurrentHealth <= 0) {
            TeleportToSpawn();
            CurrentHealth = baseHealth + BonusHealth;
            CurrentMana = baseMana + BonusMana;
        }
        
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
            ChangeActiveSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
            ChangeActiveSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) 
            ChangeActiveSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) 
            ChangeActiveSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) 
            ChangeActiveSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6)) 
            ChangeActiveSlot(5);
        else if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            ChangeActiveSlot((byte) (player.activeSlot-1));
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            ChangeActiveSlot((byte) (player.activeSlot+1));
        

        if (Input.GetKeyUp("mouse 0")) {
            if (Hud.Instance.IsInvVisible) return;
            var item = player.inventory.GetItemAt(player.activeSlot);
            Tile tile;
            if (item)
                switch (item.type) {
                    case ItemType.Block: 
                        var block = (BlockObject) item;
                        tile = _mapController.TileAtCursor();
                        if (tile) return;
                        _mapController.EditBlock(block.tile);
                        player.inventory.RemoveFromStack(player.activeSlot, 1);
                        break;
                
                    case ItemType.Pickaxe:
                        tile = _mapController.TileAtCursor();
                        if (!tile) return;
                        var itm = (BlockObject) ItemUtils.TileToItem(tile);
                        if (!ItemUtils.IsBreakable((PickaxeObject) item, itm)) return;
                        player.inventory.AddItem(itm, 1);
                        _mapController.EditBlock(null);
                        break;
                    case ItemType.Weapon:
                        break;
                    case ItemType.Axe:
                        break;
                    case ItemType.Potion:
                        var potion = (PotionObject) item;
                        if (CurrentHealth >= MaxHealth) return;
                        ChangeHealth(potion.healing);
                        player.inventory.RemoveFromStack(player.activeSlot, 1);
                        break;
                    case ItemType.Consumable:
                        break;
                    case ItemType.Material:
                        break;
                    case ItemType.Helmet:
                        var helm = (ArmorObject) item;
                        player.inventory.items[player.activeSlot].item = player.armor.items[0].item;
                        player.armor.items[0].item = helm;
                        Hud.Instance.UpdateHud();
                        break;
                    case ItemType.Armor:
                        var armor = (ArmorObject) item;
                        player.inventory.items[player.activeSlot].item = player.armor.items[1].item;
                        player.armor.items[1].item = armor;
                        Hud.Instance.UpdateHud();
                        break;
                    case ItemType.Pants:
                        var pants = (ArmorObject) item;
                        player.inventory.items[player.activeSlot].item = player.armor.items[2].item;
                        player.armor.items[2].item = pants;
                        Hud.Instance.UpdateHud();
                        break;
                    case ItemType.Utility:
                        break;
                    case ItemType.Vanity:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        if (Input.GetKeyDown("k")) {
            ChangeHealth(-5);
            ChangeMana(-5);
        }
    }

    public void ChangeBonusHealth(int value) {
        BonusHealth += value;
    }

    private void ChangeHealth(int value) {
        CurrentHealth += value;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        UpdateHealthBar();
    }
    
    public void ChangeBonusMana(int value) {
        BonusMana += value;
        UpdateManaBar();
    }

    private bool ChangeMana(int value) {
        if (CurrentMana + value < 0) return false;
        CurrentMana += value;
        UpdateManaBar();
        return true;
    }

    private void UpdateHealthBar() {
        hud.UpdateHealth((float)CurrentHealth / (baseHealth+BonusHealth));
    }

    private void UpdateManaBar() {
        hud.UpdateMana((float)CurrentMana / (baseMana + BonusMana));
    }

    private void TeleportToSpawn() => transform.position = spawnPoint;

    private void ChangeActiveSlot(byte slot) {
        if (slot >= 6 || slot < 0) return;
        player.activeSlot = slot;
        hud.UpdateSlot();
    }

}
