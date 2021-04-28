using System;
using System.Collections;
using Loader;
using LocalSave;
using Menu.Settings;
using Menu.Store.EventSystems;
using PlayGames;
using PlayGames.Dao;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using Util;
using Button = UnityEngine.UI.Button;

namespace Menu.Controllers
{
    public class StoreController : MonoBehaviour
    {
        [SerializeField] private GameObject platformWithPlayer;

        [Header("Buttons")]
        [SerializeField] private Button closeButton;

        [Header("Panels")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject storePanel;

        [Header("Coins Panel")]
        [SerializeField] private TextMeshProUGUI coinsLabel;

        private GameData _gameData;

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(CloseStore);
        }

        private IEnumerator Start()
        {
            _gameData = DataManager.Instance.GameData;

            yield return null;

            UpdateCoinsLabel();

            StoreEventSystem.Instance.OnSuccessfulPurchase += UpdateCoinsLabel;

            SettingsEventSystem.Instance.OnGameDataUpdated += UpdateCoinsLabel;
            
            CurrencyPageEventSystem.Instance.OnRewardedForVideo += () => { IncreaseCoins(10); };

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
            var localData = LocalSaveSystem.LoadLocalData();

            localData.SaveTime = DateTime.Now.Ticks;
            localData.Coins += coins;
            _gameData.Coins += coins;

            if (PlayGamesServices.IsAuthenticated && InternetConnection.Available())
                PlayGamesServices.SaveCloudData(CloudData.FromLocalData(localData));
            LocalSaveSystem.SaveLocalData(localData);

            StoreEventSystem.Instance.SuccessfulPurchase();
        }

        private void UpdateCoinsLabel()
        {
            coinsLabel.SetText(_gameData.Coins.ToString());
        }

        private void CloseStore()
        {
            platformWithPlayer.SetActive(true);
            storePanel.SetActive(false);
            menuPanel.SetActive(true);
        }
    }
}