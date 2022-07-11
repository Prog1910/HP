using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace HighwayPursuit
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager singleton;

        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _gameMenuPanel;
        [SerializeField] private GameObject _gameOverPanel;

        [SerializeField] private Text _distanceText;

        public Text DistanceText { get => _distanceText; }

        private void Awake()
        {
            if (singleton == null)
                singleton = this;
        }

        private void Start()
        {
            GameManager.singleton.GameStatus = GameStatus.NONE;
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
