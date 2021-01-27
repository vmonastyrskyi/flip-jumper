using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.EventSystems;
using Game.Platform;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Systems
{
    public class PlatformSystem : MonoBehaviour
    {
        [SerializeField] private GameObject coinPrefab;

        private readonly List<GameObject> _platforms = new List<GameObject>();

        private IEnumerator Start()
        {
            SpawnPlatforms();

            yield return null;

            GameEventSystem.instance.OnGeneratingPlatform += GeneratePlatform;
            GameEventSystem.instance.OnCoinGenerated += GenerateCoin;
        }

        private void SpawnPlatforms()
        {
            var platform = CreatePlatform(SpawnDirection.Right);
            platform.GetComponent<PlatformManager>().Visited = true;
            _platforms.Add(platform);

            _platforms.Add(CreatePlatform(SpawnDirection.Right));
        }

        private void GeneratePlatform(SpawnDirection direction)
        {
            _platforms.Add(CreatePlatform(direction));

            if (_platforms.Count > 4)
            {
                var firstPlatform = _platforms.First();
                firstPlatform.transform.DOKill();
                Destroy(firstPlatform);
                _platforms.Remove(firstPlatform);
            }
        }

        private GameObject CreatePlatform(SpawnDirection direction)
        {
            Vector3 position;
            Vector3 platformSize;
            if (_platforms.Count == 0)
            {
                position = new Vector3(0, 0.25f, 0);
                platformSize = Vector3.zero;
            }
            else
            {
                var lastPlatform = _platforms.Last();
                position = lastPlatform.transform.position;
                platformSize = Vector3.Scale(lastPlatform.transform.localScale,
                    lastPlatform.GetComponent<BoxCollider>().bounds.size);
            }

            GameObject platform;
            var platformDistance = Random.Range(platformSize.x, platformSize.x * 2) + platformSize.x / 1.5f;
            switch (direction)
            {
                case SpawnDirection.Left:
                    platform = Instantiate(
                        DataManager.instance.UserData.SelectedPlatform.Prefab,
                        new Vector3(position.x, position.y, position.z + platformDistance),
                        Quaternion.identity
                    );
                    break;
                case SpawnDirection.Right:
                    platform = Instantiate(
                        DataManager.instance.UserData.SelectedPlatform.Prefab,
                        new Vector3(position.x + platformDistance, position.y, position.z),
                        Quaternion.identity
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            platform.GetComponent<PlatformManager>().Direction = direction;

            return platform;
        }

        private void GenerateCoin()
        {
            var lastPlatform = _platforms[_platforms.Count - 1];
            var preLastPlatform = _platforms[_platforms.Count - 2];

            var lastPlatformPosition = lastPlatform.transform.position;
            var preLastPlatformPosition = preLastPlatform.transform.position;
            
            Debug.Log(lastPlatform, preLastPlatform);
            Instantiate(
                coinPrefab,
                new Vector3(preLastPlatformPosition.x + (lastPlatformPosition.x - preLastPlatformPosition.x) / 2, 
                    5, 
                    preLastPlatformPosition.z + (lastPlatformPosition.z - preLastPlatformPosition.z) / 2),
                Quaternion.identity
            );
        }
    }
}