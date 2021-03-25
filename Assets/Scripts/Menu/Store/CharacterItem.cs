using Menu.Store.EventSystems;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Store
{
    public class CharacterItem : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private GameObject lockIcon;
        [SerializeField] private GameObject selectedIcon;

        private void Awake()
        {
            selectedIcon.SetActive(character.IsSelected);
            lockIcon.SetActive(!character.IsPurchased);
            
            GetComponent<Button>().onClick.AddListener(SelectItem);

            CharactersPageEventSystem.Instance.OnSelectedCharacterChanged += selectedCharacter =>
            {
                selectedIcon.SetActive(character == selectedCharacter);
            };

            CharactersPageEventSystem.Instance.OnCharacterPurchased += purchasedCharacter =>
            {
                if (character == purchasedCharacter)
                    lockIcon.SetActive(false);
            };
        }

        private void SelectItem()
        {
            CharactersPageEventSystem.Instance.SelectCharacter(character);
        }
    }
}