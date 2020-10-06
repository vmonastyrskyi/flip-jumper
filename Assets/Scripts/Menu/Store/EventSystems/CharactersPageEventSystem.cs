using System;
using UnityEngine;

namespace Menu.Store.EventSystems
{
    public class CharactersPageEventSystem : MonoBehaviour
    {
        public static CharactersPageEventSystem instance;

        private void Awake()
        {
            instance = this;
        }

        public event Action OnSelectedCharacterChange;

        public void ChangeSelectedCharacter()
        {
            OnSelectedCharacterChange?.Invoke();
        }

        public event Action OnSuccessfulCharacterBuy;

        public void SuccessfulCharacterBuy()
        {
            OnSuccessfulCharacterBuy?.Invoke();
        }
    }
}