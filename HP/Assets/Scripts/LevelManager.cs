using System.Collections.Generic;
using UnityEngine;

namespace HighwayPursuit
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Singleton;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private GameObject _roadPrefab;
        [SerializeField] private float _roadLength = 10f;
        [SerializeField] private int _maxRoadsCount = 5;
        [SerializeField] private GameObject[] _vehiclePrefabs;
        private List<GameObject> _roadList;
        private Vector3 _nextRoadPosition;
        private GameObject _roadHolder;
        private PlayerController _playerController;
        public PlayerController PlayerController { get => _playerController; }
        public GameObject[] VehiclePrefabs { get => _vehiclePrefabs; }

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
                _nextRoadPosition += Vector3.forward * 10f;
                _roadList.Add(road);
            }

            GameObject player = new GameObject("Player");
            player.transform.position = Vector3.zero;
            player.AddComponent<PlayerController>();
            _playerController = player.GetComponent<PlayerController>();
        }
    }
}