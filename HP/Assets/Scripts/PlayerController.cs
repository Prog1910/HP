using UnityEngine;
using DG.Tweening;

namespace HighwayPursuit
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private float _endXPosition = 0f;
        private Rigidbody _playerRigidbody;
        private Collider _playerCollider;

        private void Start()
        {
            _playerRigidbody = gameObject.GetComponent<Rigidbody>();
            _playerRigidbody.isKinematic = true;
            _playerRigidbody.useGravity = false;
            SpawnVehicle(GameManager.Singleton.CurrentCarIndex);
        }

        private void OnEnable()
        {
            InputManager.Singleton.SwipeCallback += SwipeMethod;
        }

        private void OnDisable()
        {
            InputManager.Singleton.SwipeCallback -= SwipeMethod;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Enemy")
            {
                if (GameManager.Singleton.GameStatus == GameStatus.PLAYING)
                {
                    DOTween.Kill(this);
                    _playerRigidbody.isKinematic = false;
                    _playerRigidbody.useGravity = true;
                    _playerRigidbody.AddForce(Random.insideUnitCircle.normalized * 100f);
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
                    _endXPosition = transform.position.x - 3;
                    break;
                case SwipeType.RIGHT:
                    _endXPosition = transform.position.x + 3;
                    break;
            }

            _endXPosition = Mathf.Clamp(_endXPosition, -3, 3);
            transform.DOMoveX(_endXPosition, 0.15f);
        }


    }
}