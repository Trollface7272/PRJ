using System;
using Hud;
using UnityEngine;
using Inventory;
using Inventory.Items;
using SavesSystem;
using Unity.Mathematics;

namespace Entity.Player {
    public class PlayerController : MonoBehaviour {
        private static PlayerController _instance;

        public static PlayerController Instance {
            get {
                if (!_instance) _instance = FindObjectOfType<PlayerController>();
                return _instance;
            }
        }

        [SerializeField] public PlayerObject player;
        [SerializeField] public GameObject sword;

        private double _hurtTime;
        private SwordController _swordController;

        


        [SerializeField] [Range(0, 30)] private float movementSpeed;
        [SerializeField] [Range(0, 30)] private float jumpHeight;
        [SerializeField] private Camera cam;


        private Vector3 _velocity = new Vector3(0, 0, 0);
        private Rigidbody2D _rigidbody2D;
        
        [NonSerialized]
        public Vector3 SpawnPoint = Vector3.zero;

        [NonSerialized] 
        public bool Jump;
        [NonSerialized] 
        public bool CanJump;
        public float VelocityBase {
            set => _velocity.x = value * movementSpeed;
        }

        private void Start() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            player.CurrentHealth = player.MaxHealth;
            player.CurrentMana = player.MaxMana;
            _swordController = sword.GetComponentInChildren<SwordController>();
            HudControler.Instance.UpdateSlot();
            //FillInv();
            if (SaveController.LoadOnStart) SaveController.LoadGame();
        }
        
        private void FixedUpdate() {
            if (Jump && CanJump) {
                Jump = false;
                _rigidbody2D.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            }
            _rigidbody2D.velocity = new Vector3 (_velocity.x, _rigidbody2D.velocity.y, 0);
            AnimationController.Instance.Running(math.abs(_velocity.x) > 0);
            if (_velocity.x > 0) transform.localScale = new Vector3(1, 1, 1);
            else if (_velocity.x < 0) transform.localScale = new Vector3(-1, 1, 1);
        }

        private void Update() {
            var pos = transform.position;
            pos.z = -10;
            cam.transform.position = pos;
            if (Input.GetKeyDown("h")) {
                player.CurrentHealth -= 5;
                player.CurrentMana -= 5;
            }
        }

        public void Teleport(float x, float y) => transform.position = new Vector3(x, y, 0);

        public void SetSpawn(float x, float y) => SpawnPoint = new Vector3(x, y, 0);

        public void TeleportToSpawn() => transform.position = SpawnPoint;

        

        public bool Hurt(int dmg) {
            if (_hurtTime > (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds) return false;
            player.CurrentHealth -= dmg;
            _hurtTime = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 0.5f;
            return true;
        }

        public void FillInv() {
            var t = player.inventory.items;
            var l = player.itemList.items;
            t[0] = new InventorySlot(l[8], 1);
            t[1] = new InventorySlot(l[12], 1);
            t[2] = new InventorySlot(l[2], 500);
            t[3] = new InventorySlot(l[7], 500);
            t[4] = new InventorySlot(l[18], 1);
            t[5] = new InventorySlot(l[19], 1);
            t[6] = new InventorySlot(l[20], 1);
            t[7] = new InventorySlot(l[21], 50);
            t[8] = new InventorySlot(l[22], 50);
            t[9] = new InventorySlot(l[23], 500);
            t[10] = new InventorySlot(l[24], 500);
            t[11] = new InventorySlot(l[25], 500);
            t[12] = new InventorySlot(l[26], 500);
        }

        public void SwingSword() {
            if (!AnimationController.Instance.CanSwing()) return;
            _swordController.SetSword(player.inventory.items[player.activeSlot].item as SwordObject);
            _swordController.Swing();
            AnimationController.Instance.Swing(_swordController.sword.swingDelay);
        }

        
    }
}
