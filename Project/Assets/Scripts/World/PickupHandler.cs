using System;
using Inventory.Items;
using UnityEngine;
using Entity.Player;

namespace World {
    public class PickupHandler : MonoBehaviour {
        public ItemObject item;
        public int count;
        public new BoxCollider2D collider;
        [NonSerialized] public int pickProtection = 0;

        public SpriteRenderer spriteRenderer;

        private void Start() {
            spriteRenderer = transform.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = item.sprite;
            Physics2D.IgnoreCollision(collider, PlayerController.Instance.gameObject.AddComponent<BoxCollider2D>());
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;
            if (pickProtection > (int) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds) {
                Debug.Log("how");
                Physics2D.IgnoreCollision(collider, other);
                return;
            }
            PlayerController.Instance.player.inventory.AddItem(item, count);
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            Debug.Log("collision");
            if (other.gameObject.CompareTag("Player")) 
                Physics2D.IgnoreCollision(collider, other.collider);
        }

        private void OnCollisionStay2D(Collision2D other) {
            if (other.gameObject.CompareTag("Player"))
                Physics2D.IgnoreCollision(collider, other.collider);
        }
    }
}
