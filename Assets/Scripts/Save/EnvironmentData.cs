using System;

namespace Save
{
    [Serializable]
    public class EnvironmentData
    {
        private int _id;
        private bool _isPurchased;
        private bool _isSelected;

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

        public EnvironmentData(EnvironmentData environmentData)
        {
            _id = environmentData._id;
            _isPurchased = environmentData._isPurchased;
            _isSelected = environmentData._isSelected;
        }

        public EnvironmentData(int id, bool isPurchased, bool isSelected)
        {
            _id = id;
            _isPurchased = isPurchased;
            _isSelected = isSelected;
        }
    }
}