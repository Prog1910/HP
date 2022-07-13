using System.Collections.Generic;
using UnityEngine;

namespace HighwayPursuit
{
    public class EnemyManager
    {
        [SerializeField] private float _laneWidth = 3f;
        [SerializeField] private int _lanesCount = 3;

        private float _moveSpeed;
        private List<EnemyController> _deactiveEnemyList;
        private Vector3[] _enemySpawnPositions;
        private GameObject _enemyHolder;

        public EnemyManager(Vector3 spawnPosition, float moveSpeed)
        {
            this._moveSpeed = moveSpeed;
            _deactiveEnemyList = new List<EnemyController>();
            _enemySpawnPositions = new Vector3[_lanesCount];
            _enemySpawnPositions[0] = spawnPosition - Vector3.right * _laneWidth;
            _enemySpawnPositions[1] = spawnPosition;
            _enemySpawnPositions[2] = spawnPosition + Vector3.right * _laneWidth;
            _enemyHolder = new GameObject("EnemyHolder");
        }

        public void SpawnEnemies(GameObject[] vehiclePrefabs)
        {
            for (int i = 0; i < vehiclePrefabs.Length; i++)
            {
                GameObject enemy = Object.Instantiate(vehiclePrefabs[i], _enemySpawnPositions[i % 3],
                    Quaternion.identity, _enemyHolder.transform);
                enemy.SetActive(false);
                enemy.name = "Enemy";
                EnemyController enemyController = enemy.AddComponent<EnemyController>();
                enemyController.SetDefault(_moveSpeed, this);
                _deactiveEnemyList.Add(enemy.GetComponent<EnemyController>());
            }
        }

        public void ActivateEnemy()
        {
            if (_deactiveEnemyList.Count > 0)
            {
                EnemyController enemy = _deactiveEnemyList[Random.Range(0, _deactiveEnemyList.Count)];
                _deactiveEnemyList.Remove(enemy);
                enemy.transform.position = _enemySpawnPositions[Random.Range(0, _enemySpawnPositions.Length)];
                enemy.ActivateEnemy();
            }
        }

        public void DeactivateEnemy(EnemyController enemy)
        {
            enemy.gameObject.SetActive(false);
            _deactiveEnemyList.Add(enemy);
        }
    }
}