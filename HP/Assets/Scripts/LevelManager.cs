using System.Collections.Generic;
using UnityEngine;

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
        private Vector3 _nextRoadPosition;
        private GameObject _roadHolder;
        private PlayerController _playerController;
        private List<GameObject> _roadList;
        private int _lastRoadIndex, _topRoadIndex;
        public PlayerController PlayerController { get => _playerController; }
        public GameObject[] VehiclePrefabs { get => _vehiclePrefabs; }

        private int Mod(int x, int y) => (x % y + y) % y;

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

            for (int i = 0; i < _maxRoadsCount; i++)
            {
                GameObject road = Instantiate(_roadPrefab, _nextRoadPosition, Quaternion.identity, _roadHolder.transform);
                _nextRoadPosition += Vector3.forward * _roadLength;
                _roadList.Add(road);
            }

            GameObject player = new GameObject("Player");
            player.transform.position = Vector3.zero;
            player.AddComponent<PlayerController>();
            _playerController = player.GetComponent<PlayerController>();
        }

        private void Update()
        {
            MoveRoad();
        }

        private void MoveRoad()
        {
            for (int i = 0; i < _roadList.Count; i++)
                _roadList[i].transform.Translate(-transform.forward * _moveSpeed * Time.deltaTime);

            if (_roadList[_lastRoadIndex].transform.position.z <= -_roadLength)
            {
                _topRoadIndex = Mod(_lastRoadIndex - 1, _roadList.Count);

                _roadList[_lastRoadIndex].transform.position = _roadList[_topRoadIndex].transform.position + transform.forward * _roadLength;

                _lastRoadIndex = Mod(++_lastRoadIndex, _roadList.Count);
            }

        }

    }
}