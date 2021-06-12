using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity.Enemy {
    public class Spawner : MonoBehaviour {
        public Camera mainCamera;
        public GameObject enemyPrefab;
        public EnemyList enemies;

        private void Update() {
            var chance = Random.Range(0, 1000);
            if (chance == 727) {
                SpawnDaytimeEnemy();
            }
        }

        private void SpawnDaytimeEnemy() {
            var enemy = enemies.daytimeEnemies[Random.Range(0, enemies.daytimeEnemies.Count)];
            SpawnEnemy(enemy);
        }

        private void SpawnNighttimeEnemy() {
            
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SpawnEnemy(EnemyObject enemyObj) {
            var enemy = Instantiate(enemyPrefab, mainCamera.transform.position, Quaternion.identity);
            enemy.transform.SetParent(transform);
            var cont = enemy.GetComponent<EnemyController>();
            cont.Enemy = enemyObj;
            var pos = enemy.transform.position;
            pos.z = 0;
            pos.y += 10;
            pos.x += Random.Range(0, 2) == 0 ? 50 : -50;
            enemy.transform.position = pos;
        }
    }
}