using UnityEngine;

namespace Game
{
    public class TempDataManager : MonoBehaviour
    {
        public static TempDataManager instance;

        public int EarnedScore { get; set; }
        public int EarnedCoins { get; set; }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }
    }
}