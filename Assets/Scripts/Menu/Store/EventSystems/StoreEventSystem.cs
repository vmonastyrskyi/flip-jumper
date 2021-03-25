using System;
using Scriptable_Objects;
using UnityEngine;

namespace Menu.Store.EventSystems
{
    public class StoreEventSystem : MonoBehaviour
    {
        public static StoreEventSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event Action OnSuccessfulPurchase;

        public void SuccessfulPurchase()
        {
            OnSuccessfulPurchase?.Invoke();
        }
    }
}