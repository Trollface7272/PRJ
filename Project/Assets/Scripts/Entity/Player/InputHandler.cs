using UnityEngine;

namespace Entity.Player {
    public class InputHandler : MonoBehaviour {
        private PlayerController _playerController;
        private void Start() {
            _playerController = FindObjectOfType<PlayerController>();
        }
        
        private void Update() {
            _playerController.MoveVal = Input.GetAxis("Horizontal");
            _playerController.Jump = Input.GetButtonDown("Jump") || _playerController.Jump;
        }
    }
}
