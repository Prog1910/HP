using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using UnityEngine.SocialPlatforms.Impl;

namespace HighwayPursuit
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Singleton;

        [SerializeField] private float _carSelectionOffset = 8f;
        [SerializeField] private float _DOMoveXDuration = 0.5f;
        [SerializeField] private Text _distanceText;
        [SerializeField] private GameObject _mainMenuPanel, _gameMenuPanel, _gameOverPanel;
        [SerializeField] private GameObject _selectMenuPanel, _selectHolder, _carHolder;

        private int _currentCarIndex;
        private Vector3 _startCarHolderPosition;

        public Text DistanceText => _distanceText;

        private void Awake()
        {
            if (Singleton == null)
                Singleton = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            GameManager.Singleton.GameStatus = GameStatus.NONE;
            _currentCarIndex = GameManager.Singleton.CurrentCarIndex;
            _startCarHolderPosition = _carHolder.transform.position;
            _carHolder.transform.position -= Vector3.right * _carSelectionOffset * _currentCarIndex;
            PopulateCars();
        }

        private void OnEnable()
        {
            InputManager.Singleton.SwipeCallback += ActionOnSwipe;
        }

        private void OnDisable()
        {
            InputManager.Singleton.SwipeCallback -= ActionOnSwipe;
        }

        public void PlayButton()
        {
            _mainMenuPanel.SetActive(false);
            _gameMenuPanel.SetActive(true);
            LevelManager.Singleton.GameStarted();
        }

        public void RetryButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GameOver()
        {
            _gameOverPanel.SetActive(true);
            ResetScore();
        }

        public void OpenSelectionPanel(bool value)
        {
            if (value)
            {
                _mainMenuPanel.SetActive(false);
                _selectMenuPanel.SetActive(true);
                _selectHolder.SetActive(true);
                _carHolder.transform.position -= Vector3.right * _carSelectionOffset * _currentCarIndex;
            }
            else
            {
                _mainMenuPanel.SetActive(true);
                _selectMenuPanel.SetActive(false);
                _selectHolder.SetActive(false);
            }
        }

        private void SelectCarPosition()
        {
            float nextXPosition = _startCarHolderPosition.x - _carSelectionOffset * _currentCarIndex;
            _carHolder.transform.DOMoveX(nextXPosition, _DOMoveXDuration);
        }

        public void SelectCarButton()
        {
            GameManager.Singleton.CurrentCarIndex = _currentCarIndex;
            LevelManager.Singleton.PlayerController.SpawnVehicle(GameManager.Singleton.CurrentCarIndex);
            OpenSelectionPanel(false);
        }

        private void ActionOnSwipe(SwipeType swipeType)
        {
            switch (swipeType)
            {
                case SwipeType.RIGHT:
                    if (_currentCarIndex > 0)
                        _currentCarIndex--;
                    break;
                case SwipeType.LEFT:
                    if (_currentCarIndex < LevelManager.Singleton.VehiclePrefabs.Length - 1)
                        _currentCarIndex++;
                    break;
            }

            SelectCarPosition();
        }

        private void PopulateCars()
        {
            for (int i = 0; i < LevelManager.Singleton.VehiclePrefabs.Length; i++)
            {
                GameObject car = Instantiate(LevelManager.Singleton.VehiclePrefabs[i], _carHolder.transform);
                car.transform.Rotate(new Vector3(0, -150, 0));
                car.transform.localPosition = Vector3.right * i * _carSelectionOffset;
            }
        }

        private void ResetScore()
        {
            _distanceText.text = 0.ToString();
        }

        public void UpdateScore(int score)
        {
            _distanceText.text = score.ToString();
        }
    }
}