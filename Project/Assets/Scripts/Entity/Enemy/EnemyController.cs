using System;
using Entity.Player;
using UnityEngine;

namespace Entity.Enemy {
    public class EnemyController : MonoBehaviour {
        private EnemyObject _enemy;
        public EnemyObject Enemy {
            get => _enemy;
            set {
                _enemy = Instantiate(value);
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

        private double _hurtTime;

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
            if (velocity.x > 0) transform.localScale = new Vector3(1, 1, 1);
            else if (velocity.x < 0) transform.localScale = new Vector3(-1, 1, 1);
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

        public void Hurt(int damage) {
            if (_hurtTime > (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds) return;
            _hurtTime = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 0.5;
            _enemy.hp -= damage;
            if (_enemy.hp <= 0) {
                Destroy(gameObject);
            }
        }
    }
}