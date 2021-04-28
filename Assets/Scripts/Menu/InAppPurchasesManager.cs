using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Menu
{
    public class InAppPurchasesManager : MonoBehaviour, IStoreListener
    {
        public const string Coins50 = "coins_50";
        public const string Coins250 = "coins_250";
        public const string Coins500 = "coins_500";
        public const string Coins2000 = "coins_2000";

        public const string RemoveAds = "remove_ads";

        private static IStoreController _storeController;
        private static IExtensionProvider _storeExtensionProvider;
        private string[] _consumableProducts;
        private string[] _nonConsumableProducts;

        private void Awake()
        {
            _consumableProducts = new[]
            {
                Coins50, Coins250, Coins500, Coins2000
            };

            _nonConsumableProducts = new[]
            {
                RemoveAds
            };
        }

        private void Start()
        {
            InitializePurchasing();
        }

        private void InitializePurchasing()
        {
            if (IsInitialized()) return;

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (var product in _consumableProducts)
                builder.AddProduct(product, ProductType.Consumable);

            foreach (var product in _nonConsumableProducts)
                builder.AddProduct(product, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }

        private static bool IsInitialized()
        {
            return _storeController != null && _storeExtensionProvider != null;
        }

        public void BuyProduct(string productId)
        {
            BuyProductId(productId);
        }

        private void BuyProductId(string productId)
        {
            if (!IsInitialized()) return;

            var product = _storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log($"Purchasing product asynchronously: '{product.definition.id}'");
                _storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log(
                    "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                OnPurchaseFailed(product, PurchaseFailureReason.ProductUnavailable);
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("OnInitialized: PASS");
            _storeController = controller;
            _storeExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            if (_consumableProducts.Contains(args.purchasedProduct.definition.id))
            {
                InAppPurchasesEventSystem.Instance.PurchaseConsumable(args);
                Debug.Log(args.purchasedProduct.definition.id + " Bought!");
            }
            else if (_nonConsumableProducts.Contains(args.purchasedProduct.definition.id))
            {
                InAppPurchasesEventSystem.Instance.PurchaseNonConsumable(args);
                Debug.Log(args.purchasedProduct.definition.id + " Bought!");
            }
            else
            {
                Debug.Log($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
            }

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log(
                $"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
            InAppPurchasesEventSystem.Instance.FailPurchase(product, failureReason);
        }
    }
}