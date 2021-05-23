using System;
using UnityEngine;

public class Grounded : MonoBehaviour {
    private GameObject _player;

    private void Start() {
        _player = gameObject.transform.parent.gameObject;
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.collider.CompareTag("Ground")) return;
        var movement = _player.GetComponent<Movement>();
        movement.isGrounded = true;
        movement.isInAir = false;
        movement.airTime = 0;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (!collision.collider.CompareTag("Ground")) return;
        var movement = _player.GetComponent<Movement>();
        movement.isGrounded = false;
        movement.doubleJumpReady = true;
        movement.isInAir = true;
    }
}
