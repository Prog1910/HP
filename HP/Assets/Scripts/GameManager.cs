using UnityEngine;

namespace HighwayPursuit
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager singleton;

        [HideInInspector] public GameStatus GameStatus = GameStatus.NONE;
        [HideInInspector] public int CurrentCarIndex = 0;

        private void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }



    }
}
