using Scriptable_Objects;
using UnityEngine;

namespace Menu.Store
{
    public class CharacterItem : MonoBehaviour
    {
        [SerializeField] private CharacterData characterData;

        public CharacterData CharacterData => characterData;
        public GameObject Character { get; private set; }

        private void Awake()
        {
            Character = transform.GetChild(0).gameObject;
        }
    }
}