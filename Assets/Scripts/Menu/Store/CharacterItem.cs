using Scriptable_Objects;
using UnityEngine;

namespace Menu.Store
{
    public class CharacterItem : MonoBehaviour
    {
        [SerializeField] private Character character;

        public Character Character => character;
        public GameObject CharacterPrefab { get; private set; }

        private void Awake()
        {
            CharacterPrefab = transform.GetChild(0).gameObject;
        }
    }
}