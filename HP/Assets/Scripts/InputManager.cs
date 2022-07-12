using System;
using UnityEngine;

namespace HighwayPursuit
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Singleton;

        private float _swipeThreshold = 0.15f;
        private float _swipeTimeLimit = 0.25f;
        private float _startTime, _endTime;
        private Vector2 _startPosition, _endPosition;
        private SwipeType _swipeType = SwipeType.NONE;
        
        public Action<SwipeType> SwipeCallback;

        private void Awake()
        {
            if (Singleton == null)
                Singleton = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = _endPosition = Input.mousePosition;
                _startTime = _endTime = Time.time;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _endPosition = Input.mousePosition;
                _endTime = Time.time;

                if (_endTime - _startTime <= _swipeTimeLimit)
                    DetectSwipe();
            }
        }

        private void DetectSwipe()
        {
            _swipeType = SwipeType.NONE;
            Vector2 difference = _endPosition - _startPosition;

            if (difference.magnitude > _swipeThreshold * Screen.width)
            {
                _swipeType = difference.x > 0 ? SwipeType.RIGHT : SwipeType.LEFT;
            }

            SwipeCallback(_swipeType);
        }

    }
}
