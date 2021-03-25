using Scriptable_Objects;
using UnityEngine;

namespace Loader
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;

        [SerializeField] private GameData gameData;

        public GameData GameData => gameData;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
    }
}