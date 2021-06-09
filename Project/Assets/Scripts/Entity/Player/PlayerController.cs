using System;
using Hud;
using UnityEngine;
using World;
using Inventory;

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

        [Range(0, 500)] public float BaseHealth;
        [Range(0, 500)] public float BonusHealth;
        public float MaxHealth { get => BaseHealth + BonusHealth; }
        private float _currentHealth;
        public float CurrentHealth {
            get => _currentHealth;
            set {
                _currentHealth = Math.Min(value, MaxHealth);
                if (_currentHealth <= 0) HandleDeath();
                HudControler.Instance.UpdateBars();
            }
        }

        [Range(0, 500)] public float BaseMana;
        [Range(0, 500)] public float BonusMana;
        public float MaxMana { get => BaseMana + BonusMana; }
        private float _currentMana;
        public float CurrentMana {
			get => _currentMana;
			set {
                _currentMana = Math.Min(value, MaxMana);
                HudControler.Instance.UpdateBars();
            }
		}



        [SerializeField] [Range(0, 30)] private float movementSpeed;
        [SerializeField] [Range(0, 30)] private float jumpHeight;
        [SerializeField] private Camera cam;


        private Vector3 _velocity = new Vector3(0, 0, 0);
        private Rigidbody2D _rigidbody2D;
        private Vector3 _spawnPoint = Vector3.zero;

        [NonSerialized] public bool Jump;
        [NonSerialized] public bool CanJump;
        public float VelocityBase {
            set => _velocity.x = value * movementSpeed;
        }

        private void Start() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _currentHealth = MaxHealth;
            _currentMana = MaxMana;
        }
        
        private void FixedUpdate() {
            if (Jump && CanJump) {
                Jump = false;
                _rigidbody2D.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            }
            _rigidbody2D.velocity = new Vector3 (_velocity.x, _rigidbody2D.velocity.y, 0);
            //transform.position += _velocity * Time.deltaTime;
        }

        private void Update() {
            var pos = transform.position;
            pos.z = -10;
            cam.transform.position = pos;
            if (Input.GetKeyDown("h")) {
                CurrentHealth = CurrentHealth - 5;
                CurrentMana = CurrentHealth - 5;
            }
        }

        public void Teleport(float x, float y) => transform.position = new Vector3(x, y, 0);

        public void SetSpawn(float x, float y) => _spawnPoint = new Vector3(x, y, 0);

        public void TeleportToSpawn() => transform.position = _spawnPoint;

        public void HandleDeath() {
            TeleportToSpawn();
            CurrentHealth = BaseHealth;
            CurrentMana = BaseMana;
        }

        public bool UseMana(float val) {
            if (CurrentMana > val) return false;
            CurrentMana -= val;
            return true;
        }
    }
}
