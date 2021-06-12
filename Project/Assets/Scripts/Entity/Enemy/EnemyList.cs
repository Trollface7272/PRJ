using System.Collections.Generic;
using UnityEngine;

namespace Entity.Enemy {
    [CreateAssetMenu(fileName = "New Enemy List", menuName = "Entities/Enemy List")]
    public class EnemyList : ScriptableObject {
        public List<EnemyObject> daytimeEnemies;
        public List<EnemyObject> nighttimeEnemies;
    }
}