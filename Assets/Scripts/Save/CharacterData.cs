using System;

namespace Save
{
    [Serializable]
    public class CharacterData
    {
        private int _id;
        private bool _isPurchased;
        private bool _isSelected;
        private bool _isEffectEnabled;

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public bool IsPurchased
        {
            get => _isPurchased;
            set => _isPurchased = value;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }

        public bool IsEffectEnabled
        {
            get => _isEffectEnabled;
            set => _isEffectEnabled = value;
        }

        public CharacterData(CharacterData characterData)
        {
            _id = characterData._id;
            _isPurchased = characterData._isPurchased;
            _isSelected = characterData._isSelected;
            _isEffectEnabled = characterData._isEffectEnabled;
        }

        public CharacterData(int id, bool isPurchased, bool isSelected, bool isEffectEnabled)
        {
            _id = id;
            _isPurchased = isPurchased;
            _isSelected = isSelected;
            _isEffectEnabled = isEffectEnabled;
        }
    }
}