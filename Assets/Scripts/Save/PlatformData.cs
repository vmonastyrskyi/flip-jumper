using System;

namespace Save
{
    [Serializable]
    public class PlatformData
    {
        private int _id;
        private bool _isPurchased;
        private bool _isActive;

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

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        public PlatformData(PlatformData platformData)
        {
            _id = platformData._id;
            _isPurchased = platformData._isPurchased;
            _isActive = platformData._isActive;
        }

        public PlatformData(int id, bool isPurchased, bool isActive)
        {
            _id = id;
            _isPurchased = isPurchased;
            _isActive = isActive;
        }
    }
}