using System;
using UnityEngine;

namespace Entity.Player {
    public class GroundCollisionHandle : MonoBehaviour {
        private PlayerController _pc;

        private void Awake() {
            _pc = PlayerController.Instance;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("World - Solid")) return;
            _pc.CanJump = true;
        }
        
        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag("World - Solid")) return;
            _pc.CanJump = false;
        }
        
    }
}