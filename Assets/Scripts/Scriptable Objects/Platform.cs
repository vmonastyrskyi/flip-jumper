using UnityEngine;
using UnityEngine.Localization;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Platform Data", order = 53)]
    public class Platform : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject ui3DPrefab;
        [SerializeField] private GameObject uiItemPrefab;
        [SerializeField] private LocalizedString name;
        [SerializeField] private bool isDefault;
        [SerializeField] private bool isPurchased;
        [SerializeField] private bool isActive;
        [SerializeField] private int price;

        public int Id => id;

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

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public int Price => price;
    }
}