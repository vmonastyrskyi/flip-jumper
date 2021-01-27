using Game.Controllers;
using Game.EventSystems;
using Game.Player;
using Game.Player.Effects;
using Menu.Store.EventSystems;
using Scriptable_Objects;
using UnityEngine;

namespace Menu
{
    public class CharacterSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject spawn;

        private UserData _userData;
        private GameObject _selectedCharacter;

        private void Start()
        {
            _userData = DataManager.instance.UserData;

            SpawnCharacter();

            CharactersPageEventSystem.instance.OnSelectedCharacterChange += SpawnCharacter;
        }

        private void SpawnCharacter()
        {
            Destroy(_selectedCharacter);

            var character = Instantiate(_userData.SelectedCharacter.Prefab,
                spawn.transform.position + Vector3.up,
                Quaternion.identity);

            character.transform.parent = spawn.transform;
            character.transform.rotation = spawn.transform.parent.rotation;

            character.GetComponent<LineRenderer>().enabled = false;
            character.GetComponent<Rigidbody>().useGravity = false;
            character.GetComponent<BoxCollider>().enabled = false;
            character.GetComponent<DeathEffect>().enabled = false;
            character.GetComponent<PlayerController>().enabled = false;
            character.GetComponent<PlayerEventSystem>().enabled = false;

            _selectedCharacter = character;
        }
    }
}