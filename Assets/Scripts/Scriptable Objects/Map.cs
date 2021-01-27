using UnityEngine;
using Util;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Map Data", order = 54)]
    public class Map : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private int sceneIndex;
        [SerializeField] private Texture image;
        [SerializeField] private string name;
        [SerializeField] private bool isPurchased;
        [SerializeField] private bool isSelected;
        [SerializeField] private PriceCurrency currency;
        [SerializeField] private int price;

        public int Id => id;

        public int SceneIndex => sceneIndex;

        public Texture Image => image;

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
    }
}