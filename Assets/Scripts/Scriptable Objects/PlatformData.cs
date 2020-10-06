using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Platform Item", order = 53)]
    public class PlatformData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject uiPrefab;
        [SerializeField] private string name;
        [SerializeField] private bool isPurchased;
        [SerializeField] private bool isSelected;
        [SerializeField] private int price;

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

        public int Price => price;
    }
}