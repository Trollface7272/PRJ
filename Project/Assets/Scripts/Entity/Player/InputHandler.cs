using UnityEngine;
using Entity.Player;

using Hud;

namespace Entity.Player {
    public class InputHandler : MonoBehaviour {
        private PlayerController _playerController;
        private void Start() {
            _playerController = FindObjectOfType<PlayerController>();
        }
        
        private void Update() {
            _playerController.VelocityBase = Input.GetAxis("Horizontal");
            _playerController.Jump = (Input.GetButtonDown("Jump") && _playerController.CanJump) || _playerController.Jump;
            if (Input.GetKeyDown(KeyCode.Tab))
                HudControler.Instance.ToggleInv();
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
                ChangeActiveSlot((byte) (PlayerController.Instance.player.activeSlot-1));
            else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
                ChangeActiveSlot((byte) (PlayerController.Instance.player.activeSlot+1));
        }

        private void ChangeActiveSlot(byte a) {
            if (a > 5) a -= 6;
            if (a < 0) a += 6;
            PlayerController.Instance.player.activeSlot = a;
            HudControler.Instance.UpdateSlot();
        }
    }
}
