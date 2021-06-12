using System;
using Inventory.Items;
using UnityEngine;
using Entity.Player;

namespace World {
    public class PickupHandler : MonoBehaviour {
        public ItemObject item;
        public int count;
        public new BoxCollider2D collider;
        private bool _isPickedUp = false;
        [NonSerialized] public int PickProtection = 0;

        public SpriteRenderer spriteRenderer;

        private void Start() {
            spriteRenderer.sprite = item.sprite;
            Physics2D.IgnoreCollision(collider, PlayerController.Instance.gameObject.AddComponent<BoxCollider2D>());
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player") || _isPickedUp) return;
            if (PickProtection > (int) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds) {
                Physics2D.IgnoreCollision(collider, other);
                return;
            }
            
            _isPickedUp = true;
            PlayerController.Instance.player.inventory.AddItem(item, count);
            Destroy(gameObject);
        }

        private void OnEnable() {
            var otherObjects = GameObject.FindGameObjectsWithTag("Player");

            foreach (var obj in otherObjects) {
                var c = obj.GetComponents<Collider2D>();
                foreach (var coll in c) {
                    Physics2D.IgnoreCollision(coll, collider);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("World - Solid")) {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                return;
            }
            Physics2D.IgnoreCollision(collider, other.collider);
        }

        private void OnCollisionStay2D(Collision2D other) {
            if (other.gameObject.CompareTag("World - Solid")) return;
            Physics2D.IgnoreCollision(collider, other.collider);
        }
    }
}
