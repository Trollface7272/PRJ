using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Enemy;
using Inventory.Items;
using System.Threading.Tasks;
using UnityEngine;

namespace Entity.Player {
    public class SwordController : MonoBehaviour {
        private static SwordController _instance;

        public static SwordController Instance {
            get {
                if (!_instance) _instance = FindObjectOfType<SwordController>();
                return _instance;
            }
        }

        public Animator animator;
        public SpriteRenderer sr;
        public SwordObject sword;
        public Collider2D coll;
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Enemy")) return;
            var enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.Hurt(sword.damage); 
        }

        public void Swing() {
            sr.sprite = sword.sprite;
            coll.enabled = true;
            sr.enabled = true;
            animator.speed = 0.5f / (sword.swingDelay / 1000f);
            animator.Play("Sword_Swing");
            StartCoroutine(HideSword(sword.swingDelay / 1000f));
        }

        public void SetSword(SwordObject s) {
            sword = s;
        }

        private IEnumerator HideSword(float delay) {
            yield return new WaitForSeconds(delay);
            sr.sprite = null;
            coll.enabled = false;
            animator.Play("Sword_Idle");
        }
    }
}