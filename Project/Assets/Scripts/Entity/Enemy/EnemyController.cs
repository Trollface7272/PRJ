using System;
using Entity.Player;
using UnityEngine;

namespace Entity.Enemy {
    public class EnemyController : MonoBehaviour {
        private EnemyObject _enemy;
        public EnemyObject Enemy {
            get => _enemy;
            set {
                _enemy = value;
                Refresh();
            }
        }

        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private new Collider2D collider;
        [SerializeField]
        private Rigidbody2D rigidBody;

        private double _lastJumpTime;

        private void Refresh() {
            spriteRenderer.sprite = Enemy.sprite;
        }

        private void FixedUpdate() {
            var stuck = false;
            var velocity = rigidBody.velocity;
            if (Mathf.Abs(velocity.x) == 0) stuck = true;
            velocity = (PlayerController.Instance.transform.position.x > transform.position.x) ? new Vector2(_enemy.movementSpeed, velocity.y) : new Vector2(-_enemy.movementSpeed, velocity.y);
            rigidBody.velocity = velocity;
            if (stuck && (_lastJumpTime < (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)) {
                _lastJumpTime = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 0.5f;
                rigidBody.AddForce(new Vector2(0, _enemy.jumpPower), ForceMode2D.Impulse);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Enemy Collision Handle")) HandleCollisionWithPlayer();
        }
        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Enemy Collision Handle")) HandleCollisionWithPlayer();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (!other.collider.CompareTag("World - Solid")) Physics2D.IgnoreCollision(other.collider, collider);
        }

        private void HandleCollisionWithPlayer() {
            PlayerController.Instance.Hurt(Enemy.dmg);
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
    }
}