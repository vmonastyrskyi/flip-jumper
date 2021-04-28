using Game.Controllers;
using Game.EventSystems;
using Game.Player.Effects;
using Loader;
using Menu.Settings;
using Menu.Store.EventSystems;
using Scriptable_Objects;
using UnityEngine;

namespace Menu.Systems
{
    public class CharacterSpawnSystem : MonoBehaviour
    {
        [SerializeField] private GameObject platformWithPlayer;

        private GameData _gameData;
        private GameObject _selectedCharacter;

        private void Start()
        {
            _gameData = DataManager.Instance.GameData;

            SpawnCharacter(_gameData.SelectedCharacter);

            CharactersPageEventSystem.Instance.OnSelectedCharacterChanged += SpawnCharacter;
            
            SettingsEventSystem.Instance.OnGameDataUpdated += () => SpawnCharacter(_gameData.SelectedCharacter);
        }

        private void SpawnCharacter(Character character)
        {
            Destroy(_selectedCharacter);

            var menuCharacter = Instantiate(character.Prefab,
                platformWithPlayer.transform.position + new Vector3(0, 1.85f, 0),
                Quaternion.identity);

            menuCharacter.transform.parent = platformWithPlayer.transform;
            menuCharacter.transform.rotation = platformWithPlayer.transform.rotation;
            menuCharacter.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);

            menuCharacter.GetComponent<Effect>().enabled = character.IsEffectEnabled;

            Destroy(menuCharacter.GetComponent<PlayerEventSystem>());
            Destroy(menuCharacter.GetComponent<PlayerController>());
            Destroy(menuCharacter.GetComponent<DeathEffect>());
            Destroy(menuCharacter.GetComponent<BoxCollider>());
            Destroy(menuCharacter.GetComponent<Rigidbody>());

            _selectedCharacter = menuCharacter;
        }
    }
}