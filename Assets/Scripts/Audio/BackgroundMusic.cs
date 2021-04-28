using UnityEngine;

namespace Audio
{
    public class BackgroundMusic : MonoBehaviour
    {
        private static BackgroundMusic _instance;
        
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
    }
}