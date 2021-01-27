using UnityEngine;
using Util;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Platform Data", order = 53)]
    public class Platform : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject uiPrefab;
        [SerializeField] private bool isPurchased;
        [SerializeField] private bool isSelected;
        [SerializeField] private PriceCurrency currency;
        [SerializeField] private int price;

        public int Id => id;

        public GameObject Prefab => prefab;

        public GameObject UiPrefab => uiPrefab;

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

        public PriceCurrency Currency => currency;

        public int Price => price;
    }
}