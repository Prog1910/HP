using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _tilePrefabs;
    [SerializeField] private Transform _roadTransform;
    [SerializeField] private float _zPosition = 0f;
    [SerializeField] private int _maxTilesCount = 10;
    [SerializeField] private float _tileLength = 40f;
    [SerializeField] private Transform _playerTransform;
    private List<GameObject> _activeTiles = new List<GameObject>();

    private void Start()
    {
        SpawnTile(0);
        for (int i = 0; i < _maxTilesCount; i++)
        {
            SpawnTile(Random.Range(0, _tilePrefabs.Length));
        }
    }

    private void Update()
    {
        if (_playerTransform.position.z - 1.5f * _tileLength > _zPosition - (_maxTilesCount * _tileLength))
        {
            SpawnTile(Random.Range(0, _tilePrefabs.Length));
            DestroyTile();
        }
    }

    private void SpawnTile(int tileIndex)
    {
        GameObject newTile = Instantiate(_tilePrefabs[tileIndex], transform.forward * _zPosition, transform.rotation, _roadTransform);
        _activeTiles.Add(newTile);
        _zPosition += _tileLength;
    }

    private void DestroyTile()
    {
        Destroy(_activeTiles[0]);
        _activeTiles.RemoveAt(0);
    }
}