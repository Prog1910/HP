using UnityEngine;

namespace HighwayPursuit
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Singleton;
        [HideInInspector] public GameStatus GameStatus = GameStatus.NONE;
        [HideInInspector] public int CurrentCarIndex = 0;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }



    }
}
