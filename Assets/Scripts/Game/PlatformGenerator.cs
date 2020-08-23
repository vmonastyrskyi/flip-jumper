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

    internal enum PlatformType
    {
        Default,
        Moving,
        FadeOut
    }

    public class PlatformGenerator : MonoBehaviour
    {
        public GameObject[] platformPrefabs;

        public Material defaultPlatformMaterial;
        public Material movingPlatformMaterial;
        public Material fadeOutPlatformMaterial;

        private readonly List<GameObject> _platforms = new List<GameObject>();
        private readonly Random _random = new Random();

        private void Start()
        {
            SpawnPlatforms();

            GameEventSystem.current.OnCreatePlatform += GeneratePlatform;
        }

        private void SpawnPlatforms()
        {
            for (var i = 0; i < 2; i++)
                _platforms.Add(CreatePlatform(SpawnDirection.Right));
        }

        private void GeneratePlatform()
        {
            var platform = CreatePlatform((SpawnDirection) _random.Next(0, 2));
            var mesh = platform.GetComponentInChildren<MeshRenderer>();
            
            switch ((PlatformType) _random.Next(0, 3))
            {
                case PlatformType.Default:
                    mesh.material = defaultPlatformMaterial;
                    break;
                case PlatformType.Moving:
                    mesh.material = movingPlatformMaterial;
                    platform.AddComponent<Moving>();
                    break;
                case PlatformType.FadeOut:
                    mesh.material = fadeOutPlatformMaterial;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _platforms.Add(platform);

            if (_platforms.Count > 4)
            {
                var firstPlatform = _platforms.First();
                firstPlatform.transform.DOKill();
                Destroy(firstPlatform);
                _platforms.Remove(firstPlatform);
            }
        }

        private GameObject CreatePlatform(SpawnDirection spawnDirection)
        {
            Vector3 position;
            Vector3 platformSize;
            if (_platforms.Count == 0)
            {
                position = Vector3.zero;
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
            var platformDistance = _random.Next((int) platformSize.x, (int) (platformSize.x * 2f)) +
                                   (int) platformSize.x / 1.5f;
            switch (spawnDirection)
            {
                case SpawnDirection.Left:
                    GameEventSystem.current.ChangeSpawnDirection(SpawnDirection.Left);
                    platform = Instantiate(
                        platformPrefabs[randomPlatformPrefabIndex],
                        new Vector3(position.x, position.y, position.z + platformDistance),
                        Quaternion.identity);
                    platform.GetComponent<PlatformManager>().Direction = SpawnDirection.Left;
                    break;
                case SpawnDirection.Right:
                    GameEventSystem.current.ChangeSpawnDirection(SpawnDirection.Right);
                    platform = Instantiate(
                        platformPrefabs[randomPlatformPrefabIndex],
                        new Vector3(position.x + platformDistance, position.y, position.z),
                        Quaternion.identity);
                    platform.GetComponent<PlatformManager>().Direction = SpawnDirection.Right;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return platform;
        }
    }
}