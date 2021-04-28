using System;
using UnityEngine;

namespace Menu.Settings
{
    public class SettingsEventSystem : MonoBehaviour
    {
        public static SettingsEventSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event Action OnGameDataUpdated;

        public void GameDataUpdated()
        {
            OnGameDataUpdated?.Invoke();
        }
    }
}