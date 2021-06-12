using System;
using Hud;
using UnityEngine;
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

        private double _hurtTime;



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
            player.CurrentHealth = player.MaxHealth;
            player.CurrentMana = player.MaxMana;
            HudControler.Instance.UpdateSlot();
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
                player.CurrentHealth -= 5;
                player.CurrentMana -= 5;
            }
        }

        public void Teleport(float x, float y) => transform.position = new Vector3(x, y, 0);

        public void SetSpawn(float x, float y) => _spawnPoint = new Vector3(x, y, 0);

        public void TeleportToSpawn() => transform.position = _spawnPoint;

        

        public bool Hurt(int dmg) {
            if (_hurtTime > (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds) return false;
            player.CurrentHealth -= dmg;
            _hurtTime = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 0.5f;
            return true;
        }
    }
}
