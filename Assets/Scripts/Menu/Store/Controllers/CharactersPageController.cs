using System.Collections;
using System.Linq;
using DG.Tweening;
using Game.Player.Effects;
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
    public class CharactersPageController : MonoBehaviour
    {
        private const int CharactersPerRow = 3;

        [SerializeField] private GameObject platform;
        [SerializeField] private GameObject emptyItem;
        [SerializeField] private GameObject selectedItem;
        [SerializeField] private GridLayoutGroup charactersItems;
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private TextMeshProUGUI selectMessage;
        [SerializeField] private TextMeshProUGUI failureMessage;
        [SerializeField] private Toggle effectToggle;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button selectButton;
        [SerializeField] private Sprite coinIcon;

        private GameData _gameData;
        private Character _currentCharacter;
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

            AttachCharacters();

            CharactersPageEventSystem.Instance.OnCharacterSelected += SetSelectedCharacter;
        }

        private void AttachCharacters()
        {
            foreach (var character in _gameData.Characters)
                Instantiate(character.UiItemPrefab, charactersItems.transform);

            var charactersAmount = _gameData.Characters.Length;
            for (var i = 0; i < CharactersPerRow - (charactersAmount % CharactersPerRow); i++)
                Instantiate(emptyItem, charactersItems.transform);
        }

        private void OnEnable()
        {
            _currentCharacter = null;

            characterName.SetText("");
            failureMessage.SetText("");

            selectMessage.gameObject.SetActive(true);
            failureMessage.gameObject.SetActive(false);
            effectToggle.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(false);

            effectToggle.onValueChanged.RemoveAllListeners();
            buyButton.onClick.RemoveAllListeners();
            selectButton.onClick.RemoveAllListeners();

            foreach (Transform child in selectedItem.transform)
                Destroy(child.gameObject);
        }

        private void SetSelectedCharacter(Character character)
        {
            if (_currentCharacter == character)
                return;

            _currentCharacter = character;

            effectToggle.onValueChanged.RemoveAllListeners();
            buyButton.onClick.RemoveAllListeners();
            selectButton.onClick.RemoveAllListeners();

            foreach (Transform child in selectedItem.transform)
                Destroy(child.gameObject);

            Instantiate(character.Ui3DPrefab, selectedItem.transform);

            characterName.text = character.Name.GetLocalizedString().Result;
            selectMessage.gameObject.SetActive(false);
            failureMessage.gameObject.SetActive(false);
            effectToggle.gameObject.SetActive(character.IsPurchased);
            buyButton.gameObject.SetActive(!character.IsPurchased);
            selectButton.gameObject.SetActive(character.IsPurchased && !character.IsSelected);

            if (!character.IsPurchased)
            {
                _price.SetText(character.Price.ToString());
                _currencyIcon.sprite = coinIcon;

                buyButton.onClick.AddListener(ValidatePurchase);
            }
            else
            {
                effectToggle.isOn = character.IsEffectEnabled;
                effectToggle.onValueChanged.AddListener(ToggleCharacterEffect);
                selectButton.onClick.AddListener(SelectCharacter);
            }
        }

        private void ValidatePurchase()
        {
            var totalCoins = _gameData.Coins;
            var characterPrice = _currentCharacter.Price;

            if (totalCoins >= characterPrice)
            {
                var data = SaveSystem.Load();

                data.Coins -= characterPrice;
                _gameData.Coins -= characterPrice;

                data.Characters.First(c => c.Id == _currentCharacter.Id).IsPurchased = true;
                _currentCharacter.IsPurchased = true;

                SaveSystem.Save(data);

                effectToggle.onValueChanged.RemoveAllListeners();
                buyButton.onClick.RemoveAllListeners();
                selectButton.onClick.RemoveAllListeners();

                failureMessage.gameObject.SetActive(false);
                effectToggle.gameObject.SetActive(true);
                buyButton.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);

                effectToggle.isOn = _currentCharacter.IsEffectEnabled;
                effectToggle.onValueChanged.AddListener(ToggleCharacterEffect);
                selectButton.onClick.AddListener(SelectCharacter);

                CharactersPageEventSystem.Instance.PurchaseCharacter(_currentCharacter);
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

        private void ToggleCharacterEffect(bool value)
        {
            var data = SaveSystem.Load();

            data.Characters.First(c => c.Id == _currentCharacter.Id).IsEffectEnabled = value;
            _currentCharacter.IsEffectEnabled = value;

            SaveSystem.Save(data);

            _currentCharacter.Prefab.GetComponent<Effect>().enabled = value;
            platform.transform.GetChild(0).GetComponent<Effect>().enabled = value;
        }

        private void SelectCharacter()
        {
            effectToggle.onValueChanged.RemoveAllListeners();
            selectButton.onClick.RemoveAllListeners();

            failureMessage.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(false);

            var data = SaveSystem.Load();

            data.Characters.First(c => c.Id == _gameData.SelectedCharacter.Id).IsSelected = false;
            _gameData.SelectedCharacter.IsSelected = false;
            data.Characters.First(c => c.Id == _currentCharacter.Id).IsSelected = true;
            _currentCharacter.IsSelected = true;

            SaveSystem.Save(data);

            effectToggle.onValueChanged.AddListener(ToggleCharacterEffect);

            CharactersPageEventSystem.Instance.ChangeSelectedCharacter(_currentCharacter);
        }
    }
}