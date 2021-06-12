using System;
using UnityEngine;
using Entity.Player;

using Hud;
using Inventory;
using World;

namespace Entity.Player {
    public class InputHandler : MonoBehaviour {
        private PlayerController _playerController;
        private MapController _mapController;
        private PlayerObject _player;
        private HudControler _hud;
        private void Start() {
            _playerController = FindObjectOfType<PlayerController>();
            _player = _playerController.player;
            _mapController = MapController.Instance;
            _hud = HudControler.Instance;
        }
        
        private void Update() {
            _playerController.VelocityBase = Input.GetAxis("Horizontal");
            _playerController.Jump = (Input.GetButtonDown("Jump") && _playerController.CanJump) || _playerController.Jump;
            HandleInv();
            HandleClick();
        }

        private void ChangeActiveSlot(byte a) {
            if (a > 250) a = 5;
            if (a > 5) a = 0;
            _playerController.player.activeSlot = a;
            _hud.UpdateSlot();
        }

        private void HandleInv() {
            if (Input.GetKeyDown(KeyCode.Tab))
                _hud.ToggleInv();
            if (_hud.IsInvVisible) HandleInvVisible();
            else HandleInvInVisible();
        }

        private void HandleInvVisible() {
            if(Input.GetAxis("Mouse ScrollWheel") > 0f)
                _hud.AddCraftOffset(1);
            else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
                _hud.AddCraftOffset(-1);
        }

        private void HandleInvInVisible() {
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
                ChangeActiveSlot((byte) (_playerController.player.activeSlot-1));
            else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
                ChangeActiveSlot((byte) (_playerController.player.activeSlot+1));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void HandleClick() {
            if (!Input.GetKeyUp("mouse 0")) return;
            if (_hud.IsInvVisible) return;
            var item = _player.inventory.GetItemAt(_player.activeSlot);
            if (!item) return;
            item.Clicked();

        }
    }
}
