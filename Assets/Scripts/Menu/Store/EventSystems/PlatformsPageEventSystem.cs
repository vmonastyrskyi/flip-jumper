using System;
using Scriptable_Objects;
using UnityEngine;

namespace Menu.Store.EventSystems
{
    public class PlatformsPageEventSystem : MonoBehaviour
    {
        public static PlatformsPageEventSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event Action<Platform> OnPlatformSelected;

        public void SelectPlatform(Platform platform)
        {
            OnPlatformSelected?.Invoke(platform);
        }

        public event Action<Platform> OnPlatformPurchased;

        public void PurchasePlatform(Platform platform)
        {
            OnPlatformPurchased?.Invoke(platform);
        }
        
        public event Action<Platform> OnPlatformActivated;

        public void ActivatePlatform(Platform platform)
        {
            OnPlatformActivated?.Invoke(platform);
        }
    }
}