using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Menu
{
    public class InAppPurchasesEventSystem : MonoBehaviour
    {
        public static InAppPurchasesEventSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event Action<PurchaseEventArgs> OnPurchasedConsumable;

        public void PurchaseConsumable(PurchaseEventArgs args)
        {
            OnPurchasedConsumable?.Invoke(args);
        }
        
        public event Action<PurchaseEventArgs> OnPurchasedNonConsumable;

        public void PurchaseNonConsumable(PurchaseEventArgs args)
        {
            OnPurchasedNonConsumable?.Invoke(args);
        }
        
        public event Action<Product, PurchaseFailureReason> OnPurchaseFailed;

        public void FailPurchase(Product product, PurchaseFailureReason failureReason)
        {
            OnPurchaseFailed?.Invoke(product, failureReason);
        }
    }
}