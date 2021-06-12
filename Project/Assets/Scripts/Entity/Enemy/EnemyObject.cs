using UnityEngine;

namespace Entity.Enemy {
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Entities/Enemy")]
    public class EnemyObject : ScriptableObject {
        public int id;
        public new string name;
        public int dmg;
        public int hp;
        public Sprite sprite;
        public float movementSpeed;
        public float jumpPower;
    }
}
