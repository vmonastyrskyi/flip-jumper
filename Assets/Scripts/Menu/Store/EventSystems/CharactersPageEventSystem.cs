using System;
using Scriptable_Objects;
using UnityEngine;

namespace Menu.Store.EventSystems
{
    public class CharactersPageEventSystem : MonoBehaviour
    {
        public static CharactersPageEventSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event Action<Character> OnCharacterSelected;

        public void SelectCharacter(Character character)
        {
            OnCharacterSelected?.Invoke(character);
        }

        public event Action<Character> OnCharacterPurchased;

        public void PurchaseCharacter(Character character)
        {
            OnCharacterPurchased?.Invoke(character);
        }

        public event Action<Character> OnSelectedCharacterChanged;

        public void ChangeSelectedCharacter(Character character)
        {
            OnSelectedCharacterChanged?.Invoke(character);
        }
    }
}