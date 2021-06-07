using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {
    private Image img;
    private void Start() {
        img = transform.GetChild(1).GetComponent<Image>();
    }
    
    public void UpdateBar(float val) {
        img.fillAmount = val;
    }
}
