using UnityEngine;
using DG.Tweening;

namespace HighwayPursuit
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _laneWidth = 3f;
        [SerializeField] private float _DOMoveXDuration = 0.15f;
        [SerializeField] private float _crashForce = 100f;

        private float _endXPosition;
        private Rigidbody _playerRigidbody;
        private Collider _playerCollider;

        private void Start()
        {
            _playerRigidbody = gameObject.GetComponent<Rigidbody>();
            _playerRigidbody.isKinematic = true;
            _playerRigidbody.useGravity = false;
            SpawnVehicle(GameManager.Singleton.CurrentCarIndex);
        }

        private void OnDisable()
        {
            InputManager.Singleton.SwipeCallback -= SwipeMethod;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyController>())
            {
                if (GameManager.Singleton.GameStatus == GameStatus.PLAYING)
                {
                    DOTween.Kill(this);
                    LevelManager.Singleton.GameOver();
                    _playerRigidbody.isKinematic = false;
                    _playerRigidbody.useGravity = true;
                    _playerRigidbody.AddForce(Random.insideUnitCircle.normalized * _crashForce);
                    _playerCollider.isTrigger = false;
                }
            }
        }

        public void GameStarted()
        {
            InputManager.Singleton.SwipeCallback += SwipeMethod;
        }

        public void SpawnVehicle(int carIndex)
        {
            if (transform.childCount > 0)
                Destroy(transform.GetChild(0).gameObject);

            GameObject child = Instantiate(LevelManager.Singleton.VehiclePrefabs[carIndex], transform);
            _playerCollider = child.GetComponent<Collider>();
            _playerCollider.isTrigger = true;
        }

        private void SwipeMethod(SwipeType swipeType)
        {
            switch (swipeType)
            {
                case SwipeType.LEFT:
                    _endXPosition = transform.position.x - _laneWidth;
                    break;
                case SwipeType.RIGHT:
                    _endXPosition = transform.position.x + _laneWidth;
                    break;
            }

            _endXPosition = Mathf.Clamp(_endXPosition, -_laneWidth, _laneWidth);
            transform.DOMoveX(_endXPosition, _DOMoveXDuration);
        }
    }
}