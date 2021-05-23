using System;
using UnityEngine;

public class Bar : MonoBehaviour {
    //[SerializeField] public GameObject bar;
    private Transform _bar;
    private void Start() {
        _bar = gameObject.GetComponent<Transform>();
    }

    public void SetValue(float scale) {
        if (scale > 1 || scale < 0) {
            Console.Write("Tried to change scale to {0}", scale);
            return;
        }
        _bar.localScale = new Vector3(scale, 1f);
    }
}
