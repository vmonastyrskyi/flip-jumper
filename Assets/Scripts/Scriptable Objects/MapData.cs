using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(menuName = "Map Item", order = 54)]
    public class MapData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private int sceneIndex;
        [SerializeField] private Texture image;
        [SerializeField] private string name;
        [SerializeField] private bool isPurchased;
        [SerializeField] private bool isSelected;
        [SerializeField] private int price;

        public int Id => id;

        public int SceneIndex => sceneIndex;

        public Texture Image => image;

        public string Name => name;

        public bool IsPurchased { get => isPurchased; set => isPurchased = value; }

        public bool IsSelected { get => isSelected; set => isSelected = value; }

        public int Price => price;
    }
}