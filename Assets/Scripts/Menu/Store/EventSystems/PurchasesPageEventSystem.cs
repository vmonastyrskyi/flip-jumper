using System;
using UnityEngine;

namespace Menu.Store.EventSystems
{
    public class PurchasesPageEventSystem : MonoBehaviour
    {
        public static PurchasesPageEventSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event Action OnRewardedForVideo;

        public void RewardForVideo()
        {
            OnRewardedForVideo?.Invoke();
        }
    }
}