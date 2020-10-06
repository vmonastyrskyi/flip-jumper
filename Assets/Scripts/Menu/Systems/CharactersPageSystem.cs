using System;
using System.Collections;
using System.Linq;
using Menu.Store;
using Menu.Store.EventSystems;
using Menu.Store.Modal;
using Scriptable_Objects;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Systems
{
    public class CharactersPageSystem : MonoBehaviour
    {
        [SerializeField] private ScrollSnap charactersScrollView;
        [SerializeField] private TextMeshProUGUI characterName;
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

            AttachCharacters();

            charactersScrollView.onPanelChanged.AddListener(ChangeCharacter);

            CharactersPageEventSystem.instance.OnSuccessfulCharacterBuy += SetSelectPanelActive;
            CharactersPageEventSystem.instance.OnSelectedCharacterChange += SetSelectedPanelActive;
        }

        private void AttachCharacters()
        {
            foreach (var character in _userData.Characters)
                charactersScrollView.AddToBack(character.UiPrefab);

            for (var i = 0; i < _userData.Characters.Length; i++)
            {
                var character = _userData.Characters[i];
                var characterItem = charactersScrollView.Panels[i].GetComponent<CharacterItem>();

                if (character.IsSelected)
                {
                    charactersScrollView.GoToPanel(i);
                    characterName.text = character.Name;
                    characterItem.Character.GetComponent<Rotate>().enabled = true;
                    SetSelectedPanelActive();
                    break;
                }
            }
        }

        private void ChangeCharacter()
        {
            _buyButton.onClick.RemoveAllListeners();
            _selectButton.onClick.RemoveAllListeners();

            var characterPanels = charactersScrollView.Panels;
            var changedPanelIndex = charactersScrollView.CurrentPanel;
            var changedPanel = characterPanels[changedPanelIndex];
            var characterItem = changedPanel.GetComponent<CharacterItem>();
            var character = characterItem.CharacterData;

            characterName.text = character.Name;
            if (character.IsPurchased)
            {
                if (character.IsSelected)
                {
                    SetSelectedPanelActive();
                }
                else
                {
                    _selectButton.onClick.AddListener(() => SelectCharacter(character.Id));
                    SetSelectPanelActive();
                }
            }
            else
            {
                _price.SetText(character.Price.ToString());
                switch (character.Currency)
                {
                    case CharacterData.PriceCurrency.Coins:
                        _currencyIcon.sprite = coinsSprite;
                        break;
                    case CharacterData.PriceCurrency.Gems:
                        _currencyIcon.sprite = gemsSprite;
                        break;
                    case CharacterData.PriceCurrency.Achievement:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _buyButton.onClick.AddListener(() => ValidatePurchase(character));
                SetBuyPanelActive();
            }

            foreach (var panel in characterPanels)
            {
                if (panel == changedPanel)
                    continue;

                var item = panel.GetComponent<CharacterItem>();
                item.Character.transform.rotation = Quaternion.Euler(0, -90, -30);
                item.Character.GetComponent<Rotate>().enabled = false;
            }

            characterItem.Character.GetComponent<Rotate>().enabled = true;
        }

        private void ValidatePurchase(CharacterData character)
        {
            switch (character.Currency)
            {
                case CharacterData.PriceCurrency.Coins:
                    var totalCoins = _userData.Coins;
                    var coinsPrice = character.Price;

                    if (totalCoins >= coinsPrice)
                    {
                        _userData.Coins -= coinsPrice;
                        PurchaseCharacter(character);
                    }
                    else
                    {
                        ShowInformationModal();
                    }

                    break;
                case CharacterData.PriceCurrency.Gems:
                    var totalGems = _userData.Gems;
                    var gemsPrice = character.Price;

                    if (totalGems >= gemsPrice)
                    {
                        _userData.Gems -= gemsPrice;
                        PurchaseCharacter(character);
                    }
                    else
                    {
                        ShowInformationModal();
                    }

                    break;
                case CharacterData.PriceCurrency.Achievement:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PurchaseCharacter(CharacterData character)
        {
            _buyButton.onClick.RemoveAllListeners();
            _selectButton.onClick.RemoveAllListeners();

            character.IsPurchased = true;
            _selectButton.onClick.AddListener(() => SelectCharacter(character.Id));

            CharactersPageEventSystem.instance.SuccessfulCharacterBuy();
        }
        
        private void ShowInformationModal()
        {
            informationModal.SetActive(true);
        }

        private void SelectCharacter(int id)
        {
            _userData.SelectedCharacter.IsSelected = false;
            _userData.Characters.First(character => character.Id == id).IsSelected = true;
            CharactersPageEventSystem.instance.ChangeSelectedCharacter();
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