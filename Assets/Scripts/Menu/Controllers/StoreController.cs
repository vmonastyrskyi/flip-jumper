using System.Collections;
using Loader;
using Menu.Store.EventSystems;
using Save;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Controllers
{
    public class StoreController : MonoBehaviour
    {
        [SerializeField] private GameObject platformWithPlayer;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject storePanel;
        [SerializeField] private GameObject coinsPanel;
        [SerializeField] private Button closeButton;

        private GameData _gameData;
        private TextMeshProUGUI _coinsLabel;

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(CloseStore);

            _coinsLabel = coinsPanel.GetComponentInChildren<TextMeshProUGUI>();
        }

        private IEnumerator Start()
        {
            _gameData = DataManager.Instance.GameData;

            yield return null;

            UpdateCoinsLabel();

            StoreEventSystem.Instance.OnSuccessfulPurchase += UpdateCoinsLabel;

            PurchasesPageEventSystem.Instance.OnRewardedForVideo += () => { IncreaseCoins(10); };

            InAppPurchasesEventSystem.Instance.OnPurchasedConsumable += args =>
            {
                switch (args.purchasedProduct.definition.id)
                {
                    case InAppPurchasesManager.Coins50:
                        IncreaseCoins(50);
                        break;
                    case InAppPurchasesManager.Coins250:
                        IncreaseCoins(250);
                        break;
                    case InAppPurchasesManager.Coins500:
                        IncreaseCoins(500);
                        break;
                    case InAppPurchasesManager.Coins2000:
                        IncreaseCoins(2000);
                        break;
                }
            };
        }

        private void IncreaseCoins(int coins)
        {
            var data = SaveSystem.Load();

            data.Coins += coins;
            _gameData.Coins += coins;

            SaveSystem.Save(data);

            StoreEventSystem.Instance.SuccessfulPurchase();
        }

        private void UpdateCoinsLabel()
        {
            _coinsLabel.SetText(_gameData.Coins.ToString());
        }

        private void CloseStore()
        {
            platformWithPlayer.SetActive(true);
            storePanel.SetActive(false);
            menuPanel.SetActive(true);
        }
    }
}