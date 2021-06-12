using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {
    [SerializeField]
    private Image img;

    public void UpdateBar(float val) {
        img.fillAmount = val;
    }
}
