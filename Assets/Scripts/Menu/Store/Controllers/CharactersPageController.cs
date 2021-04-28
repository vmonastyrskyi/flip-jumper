using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Game.Player.Effects;
using Loader;
using LocalSave;
using Menu.Settings;
using Menu.Store.EventSystems;
using PlayGames;
using PlayGames.Dao;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Button = UnityEngine.UI.Button;

namespace Menu.Store.Controllers
{
    public class CharactersPageController : MonoBehaviour
    {
        private const int CharactersPerRow = 3;

        [SerializeField] private GameObject platformWithPlayer;

        [Header("Character Information")]
        [SerializeField] private GameObject selectedItem;
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private TextMeshProUGUI priceLabel;

        [Header("Messages")]
        [SerializeField] private TextMeshProUGUI selectMessage;
        [SerializeField] private TextMeshProUGUI failureMessage;

        [Header("Characters List")]
        [SerializeField] private GridLayoutGroup charactersItems;
        [SerializeField] private GameObject emptyItem;

        [Header("Buttons")]
        [SerializeField] private Toggle effectToggle;
        [SerializeField] private Button buyButton;
        [SerializeField] private Button selectButton;

        private GameData _gameData;
        private Character _currentCharacter;

        private IEnumerator Start()
        {
            _gameData = DataManager.Instance.GameData;

            yield return null;
            
            AttachCharacters();

            CharactersPageEventSystem.Instance.OnCharacterSelected += SetSelectedCharacter;

            SettingsEventSystem.Instance.OnGameDataUpdated += () =>
            {
                foreach (Transform child in charactersItems.transform)
                    Destroy(child.gameObject); 
                
                AttachCharacters();
            };
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

            characterName.SetText(string.Empty);
            failureMessage.SetText(string.Empty);

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
                priceLabel.SetText(character.Price.ToString());

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
                var localData = LocalSaveSystem.LoadLocalData();

                localData.SaveTime = DateTime.Now.Ticks;
                localData.Coins -= characterPrice;
                _gameData.Coins -= characterPrice;

                localData.Characters.First(c => c.Id == _currentCharacter.Id).IsPurchased = true;
                _currentCharacter.IsPurchased = true;

                if (PlayGamesServices.IsAuthenticated && InternetConnection.Available())
                    PlayGamesServices.SaveCloudData(CloudData.FromLocalData(localData));
                LocalSaveSystem.SaveLocalData(localData);

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
                failureMessageTransform.GetComponent<RectTransform>().DOPunchScale(new Vector3(0.125f, 0, 0), 0.25f, 2);
            }
        }

        private void ToggleCharacterEffect(bool value)
        {
            var localData = LocalSaveSystem.LoadLocalData();

            localData.Characters.First(c => c.Id == _currentCharacter.Id).IsEffectEnabled = value;
            _currentCharacter.IsEffectEnabled = value;

            LocalSaveSystem.SaveLocalData(localData);

            _currentCharacter.Prefab.GetComponent<Effect>().enabled = value;
            platformWithPlayer.transform.GetChild(0).GetComponent<Effect>().enabled = value;
        }

        private void SelectCharacter()
        {
            effectToggle.onValueChanged.RemoveAllListeners();
            selectButton.onClick.RemoveAllListeners();

            failureMessage.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(false);

            var localData = LocalSaveSystem.LoadLocalData();

            localData.Characters.First(c => c.Id == _gameData.SelectedCharacter.Id).IsSelected = false;
            _gameData.SelectedCharacter.IsSelected = false;
            localData.Characters.First(c => c.Id == _currentCharacter.Id).IsSelected = true;
            _currentCharacter.IsSelected = true;

            LocalSaveSystem.SaveLocalData(localData);

            effectToggle.onValueChanged.AddListener(ToggleCharacterEffect);

            CharactersPageEventSystem.Instance.ChangeSelectedCharacter(_currentCharacter);
        }
    }
}