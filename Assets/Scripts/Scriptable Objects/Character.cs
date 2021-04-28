using UnityEngine;
using UnityEngine.Localization;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Character Data", order = 52)]
    public class Character : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject ui3DPrefab;
        [SerializeField] private GameObject uiItemPrefab;
        [SerializeField] private LocalizedString name;
        [SerializeField] private bool isDefault;
        [SerializeField] private bool isPurchased;
        [SerializeField] private bool isSelected;
        [SerializeField] private bool isEffectEnabled;
        [SerializeField] private int price;

        public string Id => id;

        public GameObject Prefab => prefab;

        public GameObject Ui3DPrefab => ui3DPrefab;

        public GameObject UiItemPrefab => uiItemPrefab;

        public LocalizedString Name => name;

        public bool IsDefault => isDefault;

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

        public int Price => price;
    }
}