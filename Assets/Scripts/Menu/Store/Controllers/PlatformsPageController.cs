using System.Collections;
using System.Linq;
using DG.Tweening;
using Loader;
using Menu.Store.EventSystems;
using Save;
using Scriptable_Objects;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Store.Controllers
{
    public class PlatformsPageController : MonoBehaviour
    {
        private const int PlatformsPerRow = 3;

        [SerializeField] private GameObject emptyItem;
        [SerializeField] private GameObject selectedItem;
        [SerializeField] private GridLayoutGroup platformsItems;
        [SerializeField] private TextMeshProUGUI platformName;
        [SerializeField] private TextMeshProUGUI selectMessage;
        [SerializeField] private TextMeshProUGUI failureMessage;
        [SerializeField] private Toggle activeToggle;
        [SerializeField] private Button buyButton;
        [SerializeField] private Sprite coinIcon;

        private GameData _gameData;
        private Platform _currentPlatform;
        private SVGImage _currencyIcon;
        private TextMeshProUGUI _price;

        private void Awake()
        {
            _currencyIcon = buyButton.transform.GetChild(1).GetChild(0).GetComponent<SVGImage>();
            _price = buyButton.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        private IEnumerator Start()
        {
            _gameData = DataManager.Instance.GameData;

            yield return null;

            AttachPlatforms();

            PlatformsPageEventSystem.Instance.OnPlatformSelected += SetSelectedPlatform;
        }

        private void AttachPlatforms()
        {
            foreach (var platform in _gameData.Platforms)
            {
                if (!platform.IsDefault)
                    Instantiate(platform.UiItemPrefab, platformsItems.transform);
            }

            var platformsAmount = _gameData.Platforms.Length - 1;
            for (var i = 0; i < PlatformsPerRow - (platformsAmount % PlatformsPerRow); i++)
                Instantiate(emptyItem, platformsItems.transform);
        }

        private void OnEnable()
        {
            _currentPlatform = null;

            platformName.SetText("");
            failureMessage.SetText("");

            selectMessage.gameObject.SetActive(true);
            failureMessage.gameObject.SetActive(false);
            activeToggle.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);

            buyButton.onClick.RemoveAllListeners();
            activeToggle.onValueChanged.RemoveAllListeners();

            foreach (Transform child in selectedItem.transform)
                Destroy(child.gameObject);
        }

        private void SetSelectedPlatform(Platform platform)
        {
            if (_currentPlatform == platform)
                return;

            _currentPlatform = platform;

            buyButton.onClick.RemoveAllListeners();
            activeToggle.onValueChanged.RemoveAllListeners();

            foreach (Transform child in selectedItem.transform)
                Destroy(child.gameObject);

            Instantiate(platform.Ui3DPrefab, selectedItem.transform);

            platformName.text = platform.Name.GetLocalizedString().Result;
            selectMessage.gameObject.SetActive(false);
            failureMessage.gameObject.SetActive(false);
            activeToggle.gameObject.SetActive(platform.IsPurchased);
            buyButton.gameObject.SetActive(!platform.IsPurchased);

            if (!platform.IsPurchased)
            {
                _price.SetText(platform.Price.ToString());
                _currencyIcon.sprite = coinIcon;

                buyButton.onClick.AddListener(ValidatePurchase);
            }
            else
            {
                activeToggle.isOn = platform.IsActive;
                activeToggle.onValueChanged.AddListener(TogglePlatformActive);
            }
        }

        private void ValidatePurchase()
        {
            var totalCoins = _gameData.Coins;
            var platformPrice = _currentPlatform.Price;

            if (totalCoins >= platformPrice)
            {
                var data = SaveSystem.Load();

                data.Coins -= platformPrice;
                _gameData.Coins -= platformPrice;

                data.Platforms.First(p => p.Id == _currentPlatform.Id).IsPurchased = true;
                _currentPlatform.IsPurchased = true;

                SaveSystem.Save(data);

                activeToggle.onValueChanged.RemoveAllListeners();
                buyButton.onClick.RemoveAllListeners();

                failureMessage.gameObject.SetActive(false);
                activeToggle.gameObject.SetActive(true);
                buyButton.gameObject.SetActive(false);

                activeToggle.isOn = _currentPlatform.IsActive;
                activeToggle.onValueChanged.AddListener(TogglePlatformActive);

                PlatformsPageEventSystem.Instance.PurchasePlatform(_currentPlatform);
                StoreEventSystem.Instance.SuccessfulPurchase();
            }
            else
            {
                var buyButtonTransform = buyButton.GetComponent<RectTransform>();
                buyButtonTransform.DOKill(true);
                buyButtonTransform.DOPunchPosition(new Vector3(15, 0, 0), 0.5f);

                failureMessage.gameObject.SetActive(true);

                var failureMessageTransform = failureMessage.GetComponent<RectTransform>();
                failureMessageTransform.DOKill(true);
                failureMessageTransform.GetComponent<RectTransform>().DOPunchScale(new Vector3(0.125f, 0, 0), 0.5f, 2);
            }
        }

        private void TogglePlatformActive(bool value)
        {
            var data = SaveSystem.Load();

            data.Platforms.First(p => p.Id == _currentPlatform.Id).IsActive = value;
            _currentPlatform.IsActive = value;

            SaveSystem.Save(data);
            
            PlatformsPageEventSystem.Instance.ActivatePlatform(_currentPlatform);
        }
    }
}