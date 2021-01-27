using UnityEngine;
using Util;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Character Data", order = 52)]
    public class Character : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject uiPrefab;
        [SerializeField] private string name;
        [SerializeField] private bool isPurchased;
        [SerializeField] private bool isSelected;
        [SerializeField] private PriceCurrency currency;
        [SerializeField] private int price;
        [SerializeField] private bool isUpgradable;

        public int Id => id;

        public GameObject Prefab => prefab;

        public GameObject UiPrefab => uiPrefab;

        public string Name => name;

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

        public bool IsUpgradable => isUpgradable;
    }
}