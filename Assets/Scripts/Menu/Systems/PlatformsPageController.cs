using System;
using System.Collections;
using System.Linq;
using Menu.Store;
using Menu.Store.EventSystems;
using Scriptable_Objects;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Menu.Systems
{
    public class PlatformsPageController : MonoBehaviour
    {
        [SerializeField] private ScrollSnap platformsScrollView;
        [SerializeField] private GameObject buyPanel;
        [SerializeField] private GameObject selectPanel;
        [SerializeField] private GameObject selectedPanel;
        [SerializeField] private GameObject informationModal;
        [SerializeField] private Sprite coinsSprite;
        [SerializeField] private Sprite gemsSprite;

        private UserData _userData;
        private Button _buyButton;
        private SVGImage _currencyIcon;
        private TextMeshProUGUI _price;
        private Button _selectButton;

        private void Awake()
        {
            _buyButton = buyPanel.transform.GetChild(0).GetComponent<Button>();
            _currencyIcon = _buyButton.transform.GetChild(0).GetComponent<SVGImage>();
            _price = _buyButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _selectButton = selectPanel.GetComponentInChildren<Button>();
        }

        private IEnumerator Start()
        {
            _userData = DataManager.instance.UserData;

            yield return null;

            AttachPlatforms();

            platformsScrollView.onPanelChanged.AddListener(ChangeCharacter);

            PlatformsPageEventSystem.instance.OnSuccessfulPlatformPurchase += SetSelectPanelActive;
            PlatformsPageEventSystem.instance.OnSelectedPlatformChange += SetSelectedPanelActive;
        }

        private void AttachPlatforms()
        {
            var platforms = _userData.Platforms;
            foreach (var platform in platforms)
                platformsScrollView.AddToBack(platform.UiPrefab);

            for (var i = 0; i < platforms.Length; i++)
            {
                var platform = platforms[i];
                var platformItem = platformsScrollView.Panels[i].GetComponent<PlatformItem>();

                if (platform.IsSelected)
                {
                    platformsScrollView.GoToPanel(i);
                    platformItem.PlatformPrefab.GetComponent<Rotate>().enabled = true;
                    SetSelectedPanelActive();
                    break;
                }
            }
        }

        private void ChangeCharacter()
        {
            _buyButton.onClick.RemoveAllListeners();
            _selectButton.onClick.RemoveAllListeners();

            var platformPanels = platformsScrollView.Panels;
            var changedPanelIndex = platformsScrollView.CurrentPanel;
            var changedPanel = platformPanels[changedPanelIndex];
            var platformItem = changedPanel.GetComponent<PlatformItem>();
            var platform = platformItem.Platform;

            if (platform.IsPurchased)
            {
                if (platform.IsSelected)
                {
                    SetSelectedPanelActive();
                }
                else
                {
                    _selectButton.onClick.AddListener(() => SelectPlatform(platform.Id));
                    SetSelectPanelActive();
                }
            }
            else
            {
                _price.SetText(platform.Price.ToString());
                switch (platform.Currency)
                {
                    case PriceCurrency.Coins:
                        _currencyIcon.sprite = coinsSprite;
                        break;
                    case PriceCurrency.Gems:
                        _currencyIcon.sprite = gemsSprite;
                        break;
                    case PriceCurrency.Achievement:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _buyButton.onClick.AddListener(() => ValidatePurchase(platform));
                SetBuyPanelActive();
            }

            foreach (var panel in platformPanels)
            {
                if (panel == changedPanel)
                    continue;

                var item = panel.GetComponent<PlatformItem>();
                item.PlatformPrefab.transform.rotation = Quaternion.Euler(0, -90, -30);
                item.PlatformPrefab.GetComponent<Rotate>().enabled = false;
            }

            platformItem.PlatformPrefab.GetComponent<Rotate>().enabled = true;
        }

        private void ValidatePurchase(Platform platform)
        {
            switch (platform.Currency)
            {
                case PriceCurrency.Coins:
                    var totalCoins = _userData.Coins;
                    var coinsPrice = platform.Price;

                    if (totalCoins >= coinsPrice)
                    {
                        _userData.Coins -= coinsPrice;
                        PurchasePlatform(platform);
                    }
                    else
                    {
                        ShowInformationModal();
                    }

                    break;
                case PriceCurrency.Gems:
                    var totalGems = _userData.Gems;
                    var gemsPrice = platform.Price;

                    if (totalGems >= gemsPrice)
                    {
                        _userData.Gems -= gemsPrice;
                        PurchasePlatform(platform);
                    }
                    else
                    {
                        ShowInformationModal();
                    }

                    break;
                case PriceCurrency.Achievement:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PurchasePlatform(Platform platform)
        {
            _buyButton.onClick.RemoveAllListeners();
            _selectButton.onClick.RemoveAllListeners();

            platform.IsPurchased = true;
            _selectButton.onClick.AddListener(() => SelectPlatform(platform.Id));

            PlatformsPageEventSystem.instance.SuccessfulPlatformPurchase();
        }

        private void ShowInformationModal()
        {
            informationModal.SetActive(true);
        }

        private void SelectPlatform(int id)
        {
            _userData.SelectedPlatform.IsSelected = false;
            _userData.Platforms.First(platform => platform.Id == id).IsSelected = true;
            PlatformsPageEventSystem.instance.ChangeSelectedPlatform();
        }

        private void SetBuyPanelActive()
        {
            buyPanel.SetActive(true);
            selectPanel.SetActive(false);
            selectedPanel.SetActive(false);
        }

        private void SetSelectPanelActive()
        {
            buyPanel.SetActive(false);
            selectPanel.SetActive(true);
            selectedPanel.SetActive(false);
        }

        private void SetSelectedPanelActive()
        {
            buyPanel.SetActive(false);
            selectPanel.SetActive(false);
            selectedPanel.SetActive(true);
        }
    }
}