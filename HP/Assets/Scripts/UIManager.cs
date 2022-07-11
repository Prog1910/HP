using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace HighwayPursuit
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Singleton;
        [SerializeField] private GameObject _mainMenuPanel, _gameMenuPanel, _gameOverPanel;
        [SerializeField] private Text _distanceText;
        public Text DistanceText { get => _distanceText; }

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
        }

        public void PlayButton()
        {
            _mainMenuPanel.SetActive(false);
            _gameMenuPanel.SetActive(true);
        }

        public void RertyButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GameOver()
        {
            _gameMenuPanel.SetActive(true);
        }
    }
}
