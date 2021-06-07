using System.Collections;
using System.Collections.Generic;
using Entity.Player;
using UnityEngine;

public class HudControler : MonoBehaviour {
    private static HudControler _instance;
    public static HudControler Instance {
        get {
            if (!_instance) _instance = FindObjectOfType<HudControler>();
            return _instance;
        }
    }
    private PlayerController _pc;
    private Bar Health;
    private Bar Mana;
    private void Start() {
        _pc = PlayerController.Instance;
        var bars = transform.Find("Bars");
        Health = bars.Find("Health").GetComponent<Bar>();
        Mana = bars.Find("Mana").GetComponent<Bar>();
    }

    public void UpdateBars() {
        Debug.Log(_pc.CurrentHealth);
        Debug.Log(_pc.MaxHealth);
        Health.UpdateBar(_pc.CurrentHealth / _pc.MaxHealth);
        Mana.UpdateBar(_pc.CurrentMana / _pc.MaxMana);
    }
}
