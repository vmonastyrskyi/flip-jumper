using System;
using UnityEngine;

namespace Menu.Store.EventSystems
{
    public class MapsPageEventSystem : MonoBehaviour
    {
        public static MapsPageEventSystem instance;

        private void Awake()
        {
            instance = this;
        }

        public event Action OnSelectedMapChange;

        public void ChangeSelectedMap()
        {
            OnSelectedMapChange?.Invoke();
        }

        public event Action OnSuccessfulMapPurchase;

        public void SuccessfulMapPurchase()
        {
            OnSuccessfulMapPurchase?.Invoke();
        }
    }
}