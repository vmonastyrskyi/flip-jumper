using System;
using UnityEngine;

namespace LocalSave.Dao
{
    [Serializable]
    public class PlatformData
    {
        [SerializeField] private string id;
        [SerializeField] private bool isPurchased;
        [SerializeField] private bool isActive;

        public string Id => id;

        public bool IsPurchased
        {
            get => isPurchased;
            set => isPurchased = value;
        }

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public PlatformData(PlatformData platformData)
        {
            id = platformData.id;
            isPurchased = platformData.isPurchased;
            isActive = platformData.isActive;
        }

        public PlatformData(string id, bool isPurchased, bool isActive)
        {
            this.id = id;
            this.isPurchased = isPurchased;
            this.isActive = isActive;
        }
    }
}