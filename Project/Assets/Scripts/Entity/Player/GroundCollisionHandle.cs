using System;
using UnityEngine;

namespace Entity.Player {
    public class GroundCollisionHandle : MonoBehaviour {
        private PlayerController _pc;

        private void Awake() {
            _pc = PlayerController.Instance;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (!collision.collider.CompareTag("World - Solid")) return;
            _pc.CanJump = true;
        }
        
        private void OnCollisionExit2D(Collision2D collision) {
            if (!collision.collider.CompareTag("World - Solid")) return;
            _pc.CanJump = false;
        }
        
    }
}