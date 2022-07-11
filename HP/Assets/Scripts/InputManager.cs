using System;
using UnityEngine;

namespace HighwayPursuit
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Singleton;
        private Vector2 _startPosition, _endPosition, _difference;
        private SwipeType _swipeType = SwipeType.NONE;
        private float _swipeThreshold = 0.15f;
        private float _swipeTimeLimit = 0.25f;
        private float _startTime, _endTime;
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
                    DetectSwipeMethod();
            }
        }

        private void DetectSwipeMethod()
        {
            _swipeType = SwipeType.NONE;
            _difference = _endPosition - _startPosition;

            if (_difference.magnitude > _swipeThreshold * Screen.width)
            {
                if (_difference.x > 0)
                    _swipeType = SwipeType.RIGHT;
                else if (_difference.x < 0)
                    _swipeType = SwipeType.LEFT;
            }

            SwipeCallback(_swipeType);
        }

    }
}
