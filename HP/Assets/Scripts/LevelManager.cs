using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace HighwayPursuit
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Singleton;

        [SerializeField] private int _maxRoadsCount = 5;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _roadLength = 10f;
        [SerializeField] private GameObject _roadPrefab;
        [SerializeField] private GameObject[] _vehiclePrefabs;

        private int _lastRoadIndex, _topRoadIndex;
        private int _score;
        private Vector3 _nextRoadPosition;
        private GameObject _roadHolder;
        private PlayerController _playerController;
        private List<GameObject> _roadList;
        private EnemyManager _enemyManager;
        private UIManager _uiManager;

        public PlayerController PlayerController => _playerController;

        public GameObject[] VehiclePrefabs => _vehiclePrefabs;

        private static int Mod(int x, int y) => (x % y + y) % y;

        private void Awake()
        {
            if (Singleton == null)
                Singleton = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _roadHolder = new GameObject("RoadHolder");
            _roadList = new List<GameObject>();
            _uiManager = UIManager.Singleton;

            for (int i = 0; i < _maxRoadsCount; i++)
            {
                GameObject road = Instantiate(_roadPrefab, _nextRoadPosition, Quaternion.identity,
                    _roadHolder.transform);
                road.name = "Road " + i.ToString();
                _nextRoadPosition += Vector3.forward * _roadLength;
                _roadList.Add(road);
            }

            _enemyManager = new EnemyManager(_nextRoadPosition, _moveSpeed);
            SpawnPlayer();
            _enemyManager.SpawnEnemies(_vehiclePrefabs);
        }

        private void Update()
        {
            if (GameManager.Singleton.GameStatus != GameStatus.FAILED)
            {
                MoveRoad();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyController>())
                _enemyManager.ActivateEnemy();
        }

        private void SpawnPlayer()
        {
            GameObject player = new GameObject("Player") { transform = { position = Vector3.zero } };
            _playerController = player.AddComponent<PlayerController>();
        }

        private void MoveRoad()
        {
            foreach (var road in _roadList)
                road.transform.Translate(-transform.forward * (_moveSpeed * Time.deltaTime));

            if (_roadList[_lastRoadIndex].transform.position.z <= -_roadLength)
            {
                _topRoadIndex = Mod(_lastRoadIndex - 1, _roadList.Count);
                _roadList[_lastRoadIndex].transform.position =
                    _roadList[_topRoadIndex].transform.position + transform.forward * _roadLength;
                _lastRoadIndex = Mod(++_lastRoadIndex, _roadList.Count);
                UpdateScore();
            }
        }

        public void GameStarted()
        {
            GameManager.Singleton.GameStatus = GameStatus.PLAYING;
            _enemyManager.ActivateEnemy();
            _playerController.GameStarted();
            InputManager.Singleton.enabled = true;
        }

        public void GameOver()
        {
            GameManager.Singleton.GameStatus = GameStatus.FAILED;
            if (Camera.main != null)
                Camera.main.transform.DOShakePosition(1f, Random.insideUnitCircle.normalized, 5, 10f, false, true)
                    .OnComplete
                    (
                        () => UIManager.Singleton.GameOver()
                    );
            UIManager.Singleton.GameOver();
            InputManager.Singleton.enabled = false;
        }

        private void UpdateScore()
        {
            _uiManager.UpdateScore(++_score);
        }
    }
}