using Entity.Player;
using UnityEngine;

namespace Hud {
    public class HudControler : MonoBehaviour {
        private static HudControler _instance;
        public static HudControler Instance {
            get {
                if (!_instance) _instance = FindObjectOfType<HudControler>();
                return _instance;
            }
        }
        private PlayerController _pc;
        private Bar _health;
        private Bar _mana;
        private void Start() {
            _pc = PlayerController.Instance;
            var bars = transform.Find("Bars");
            _health = bars.Find("Health").GetComponent<Bar>();
            _mana = bars.Find("Mana").GetComponent<Bar>();
        }

        public void UpdateBars() {
            _health.UpdateBar(_pc.CurrentHealth / _pc.MaxHealth);
            _mana.UpdateBar(_pc.CurrentMana / _pc.MaxMana);
        }
    }
}
