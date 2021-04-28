using System;
using UnityEngine;

namespace Menu.Store.EventSystems
{
    public class CurrencyPageEventSystem : MonoBehaviour
    {
        public static CurrencyPageEventSystem Instance;

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