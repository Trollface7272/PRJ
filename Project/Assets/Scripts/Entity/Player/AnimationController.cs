using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.Player {
    public class AnimationController : MonoBehaviour {
        private static AnimationController _instance;

        public static AnimationController Instance {
            get {
                if (!_instance) _instance = FindObjectOfType<AnimationController>();
                return _instance;
            }
        }
        public Animator playerAnimator;
        private string _currAnim;

        private bool _isSwinging;
        private bool _isRunning;

        private void Start() {
            _isSwinging = false;
            _isRunning = false;
        }

        private void Update() {
            switch (_isRunning) {
                case true when _isSwinging:
                    PlayAnimation("Player_Running_Swinging");
                    break;
                case true:
                    PlayAnimation("Player_Running");
                    break;
                default: {
                    PlayAnimation(_isSwinging ? "Player_Swinging" : "Player_Idle");
                    break;
                }
            }
        }

        public void Swing(int speed) {
            _isSwinging = true;
            Task.Delay(speed).ContinueWith(e => _isSwinging = false);
        }

        public bool CanSwing() => !_isSwinging;

        public void Running(bool v) {
            _isRunning = v;
        }

        public void PlayAnimation(string animName) {
            if (_currAnim == animName) return;
            playerAnimator.Play(animName);
            _currAnim = animName;
        }
    }
}