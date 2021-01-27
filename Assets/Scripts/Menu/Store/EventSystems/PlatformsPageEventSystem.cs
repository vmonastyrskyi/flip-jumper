using System;
using UnityEngine;

namespace Menu.Store.EventSystems
{
    public class PlatformsPageEventSystem : MonoBehaviour
    {
        public static PlatformsPageEventSystem instance;

        private void Awake()
        {
            instance = this;
        }

        public event Action OnSelectedPlatformChange;

        public void ChangeSelectedPlatform()
        {
            OnSelectedPlatformChange?.Invoke();
        }

        public event Action OnSuccessfulPlatformPurchase;

        public void SuccessfulPlatformPurchase()
        {
            OnSuccessfulPlatformPurchase?.Invoke();
        }
    }
}