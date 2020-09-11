using Game;
using Game.Player;
using Loader;
using UnityEngine;

namespace Menu
{
    public class CharacterSelection : MonoBehaviour
    {
        [SerializeField] private GameObject spawn;

        private DataManager _dataManager;
        private GameObject[] _characters;
        private GameObject _selectedCharacter;
        private int _selectedCharacterIndex;

        private void Start()
        {
            _dataManager = DataManager.instance;
            _characters = _dataManager.characters;
            _selectedCharacterIndex = _dataManager.selectedCharacterIndex;

            SpawnCharacter(_selectedCharacterIndex);
        }

        private void Update()
        {
            spawn.transform.parent.Rotate(0, -(12f * Time.deltaTime), 0);
        }

        public void SelectNextCharacter()
        {
            SpawnCharacter(++_selectedCharacterIndex);
        }

        public void SelectPreviousCharacter()
        {
            SpawnCharacter(--_selectedCharacterIndex);
        }

        private void SpawnCharacter(int index)
        {
            if (index < 0)
                _selectedCharacterIndex = index = _characters.Length - 1;
            else if (index > _characters.Length - 1)
                _selectedCharacterIndex = index = 0;

            Destroy(_selectedCharacter);

            var character = Instantiate(_characters[index],
                spawn.transform.position + Vector3.up * 1.50f,
                Quaternion.Euler(0, 90, 0));

            character.transform.parent = spawn.transform;

            var platformScale = spawn.transform.parent.localScale;
            character.transform.localScale = new Vector3(
                1 / platformScale.x * 1.50f,
                1 / platformScale.y * 1.50f,
                1 / platformScale.z * 1.50f);

            character.GetComponent<LineRenderer>().enabled = false;
            character.GetComponent<Rigidbody>().useGravity = false;
            character.GetComponent<BoxCollider>().enabled = false;
            character.GetComponent<PlayerController>().enabled = false;
            character.GetComponent<PlayerEventSystem>().enabled = false;

            _dataManager.selectedCharacterIndex = index;
            _selectedCharacter = character;
        }
    }
}