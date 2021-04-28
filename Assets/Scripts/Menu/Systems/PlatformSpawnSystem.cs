using Loader;
using Scriptable_Objects;
using UnityEngine;

namespace Menu.Systems
{
    public class PlatformSpawnSystem : MonoBehaviour
    {
        [SerializeField] private GameObject platformWithPlayer;

        private GameData _gameData;

        private void Start()
        {
            _gameData = DataManager.Instance.GameData;

            SpawnPlatform(_gameData.DefaultPlatform);
        }

        private void SpawnPlatform(Platform platform)
        {
            var platformPrefab = platform.Prefab;
            platformWithPlayer.GetComponent<MeshFilter>().sharedMesh =
                platformPrefab.GetComponent<MeshFilter>().sharedMesh;
            platformWithPlayer.GetComponent<MeshRenderer>().sharedMaterials =
                platformPrefab.GetComponent<MeshRenderer>().sharedMaterials;
        }
    }
}