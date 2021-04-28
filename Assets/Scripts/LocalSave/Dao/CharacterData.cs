using System;
using UnityEngine;

namespace LocalSave.Dao
{
    [Serializable]
    public class CharacterData
    {
        [SerializeField] private string id;
        [SerializeField] private bool isPurchased;
        [SerializeField] private bool isSelected;
        [SerializeField] private bool isEffectEnabled;

        public string Id => id;

        public bool IsPurchased
        {
            get => isPurchased;
            set => isPurchased = value;
        }

        public bool IsSelected
        {
            get => isSelected;
            set => isSelected = value;
        }

        public bool IsEffectEnabled
        {
            get => isEffectEnabled;
            set => isEffectEnabled = value;
        }

        public CharacterData(CharacterData characterData)
        {
            id = characterData.id;
            isPurchased = characterData.isPurchased;
            isSelected = characterData.isSelected;
            isEffectEnabled = characterData.isEffectEnabled;
        }

        public CharacterData(string id, bool isPurchased, bool isSelected, bool isEffectEnabled)
        {
            this.id = id;
            this.isPurchased = isPurchased;
            this.isSelected = isSelected;
            this.isEffectEnabled = isEffectEnabled;
        }
    }
}