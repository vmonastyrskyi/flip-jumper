using System;
using UnityEngine;

namespace LocalSave.Dao
{
    [Serializable]
    public class EnvironmentData
    {
        [SerializeField] private string id;
        [SerializeField] private bool isPurchased;
        [SerializeField] private bool isSelected;

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

        public EnvironmentData(EnvironmentData environmentData)
        {
            id = environmentData.id;
            isPurchased = environmentData.isPurchased;
            isSelected = environmentData.isSelected;
        }

        public EnvironmentData(string id, bool isPurchased, bool isSelected)
        {
            this.id = id;
            this.isPurchased = isPurchased;
            this.isSelected = isSelected;
        }
    }
}