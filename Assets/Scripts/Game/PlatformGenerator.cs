using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Platform;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public enum SpawnDirection
    {
        Left,
        Right
    }

    public class PlatformGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject[] platformPrefabs;

        private readonly List<GameObject> _platforms = new List<GameObject>();
        private readonly Random _random = new Random();

        private void Start()
        {
            SpawnPlatforms();

            GameEventSystem.instance.OnPlatformCreate += Generate;
        }

        private void SpawnPlatforms()
        {
            var platform = CreatePlatform(SpawnDirection.Right);
            platform.GetComponent<PlatformManager>().Visited = true;
            _platforms.Add(platform);
            
            _platforms.Add(CreatePlatform(SpawnDirection.Right));
        }

        private void Generate(SpawnDirection direction)
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
                    lastPlatform.GetComponentInChildren<MeshRenderer>().bounds.size);
            }

            GameObject platform;
            var randomPlatformPrefabIndex = _random.Next(0, platformPrefabs.Length - 1);
            var platformDistance = _random.Next((int) platformSize.x,
                (int) (platformSize.x * 2f)) + (int) platformSize.x / 1.5f;
            switch (direction)
            {
                case SpawnDirection.Left:
                    // GameEventSystem.instance.ChangeSpawnDirection(SpawnDirection.Left);
                    platform = Instantiate(
                        platformPrefabs[randomPlatformPrefabIndex],
                        new Vector3(position.x, position.y, position.z + platformDistance),
                        Quaternion.identity);
                    break;
                case SpawnDirection.Right:
                    // GameEventSystem.instance.ChangeSpawnDirection(SpawnDirection.Right);
                    platform = Instantiate(
                        platformPrefabs[randomPlatformPrefabIndex],
                        new Vector3(position.x + platformDistance, position.y, position.z),
                        Quaternion.identity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            platform.GetComponent<PlatformManager>().Direction = direction;

            return platform;
        }
    }
}