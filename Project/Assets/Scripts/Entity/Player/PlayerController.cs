using System;
using UnityEngine;
using World;

namespace Entity.Player {
    public class PlayerController : MonoBehaviour {
        private static PlayerController _instance;

        public static PlayerController Instance {
            get {
                if (!_instance) _instance = FindObjectOfType<PlayerController>();
                return _instance;
            }
        }

        [SerializeField] [Range(0, 30)] private float movementSpeed;
        [SerializeField] [Range(0, 30)] private float jumpHeight;
        [SerializeField] private Camera cam;
        
        
        private Vector3 _moveVec = new Vector3(0, 0, 0);
        private Rigidbody2D _rigidbody2D;
        private Vector3 _spawnPoint = Vector3.zero;

        [NonSerialized] public bool Jump = false;
        public float MoveVal {
            set => _moveVec.x = value * movementSpeed;
        }

        private void Start() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate() {
            if (Jump) {
                Jump = false;
                _rigidbody2D.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            }
        }

        private void Update() {
            var pos = transform.position;
            pos.x -= 2;
            pos.y -= 3;
            pos.z = -10;
            cam.transform.position = pos;
        }

        public void Teleport(float x, float y) => transform.position = new Vector3(x, y, 0);

        public void SetSpawn(float x, float y) => _spawnPoint = new Vector3(x, y, 0);

        public void TeleportToSpawn() => transform.position = _spawnPoint;
    }
}
