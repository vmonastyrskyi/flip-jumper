using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Controllers;
using Game.EventSystems;
using Loader;
using Scriptable_Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Systems
{
    public class PlatformSystem : MonoBehaviour
    {
        private readonly List<GameObject> _platforms = new List<GameObject>();

        private GameData _gameData;

        private void Awake()
        {
            _gameData = DataManager.Instance.GameData;
        }

        private IEnumerator Start()
        {
            SpawnPlatforms();

            yield return null;

            GameEventSystem.Instance.OnPlatformGenerate += GeneratePlatformGenerate;
        }

        private void SpawnPlatforms()
        {
            var platform = CreatePlatform(JumpDirection.Right, false);
            var platformController = platform.GetComponent<PlatformController>();
            platformController.Visited = true;
            _platforms.Add(platform);

            _platforms.Add(CreatePlatform(JumpDirection.Right, false));
        }

        private void GeneratePlatformGenerate(JumpDirection direction, bool isMoving)
        {
            _platforms.Add(CreatePlatform(direction, isMoving));

            if (_platforms.Count > 4)
            {
                var firstPlatform = _platforms.First();
                firstPlatform.transform.DOKill();
                Destroy(firstPlatform);
                _platforms.Remove(firstPlatform);
            }
        }

        private GameObject CreatePlatform(JumpDirection direction, bool isMoving)
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
                position = lastPlatform.GetComponent<PlatformController>().InitialPosition;
                platformSize = Vector3.Scale(lastPlatform.transform.localScale,
                    lastPlatform.GetComponent<BoxCollider>().bounds.size);
            }

            GameObject platform;
            var a = (platformSize.x + platformSize.z) / 2;
            var platformDistance = Random.Range(a, a * 2) + a / 1.5f;
            var platformsData = _gameData.Platforms;
            var activePlatformsData = platformsData.Where(p => p.IsActive).ToArray();
            var platformData = activePlatformsData[Random.Range(0, activePlatformsData.Length)];

            switch (direction)
            {
                case JumpDirection.Left:
                    platform = Instantiate(
                        platformData.Prefab,
                        new Vector3(position.x, position.y, position.z + platformDistance),
                        Quaternion.identity
                    );
                    break;
                case JumpDirection.Right:
                    platform = Instantiate(
                        platformData.Prefab,
                        new Vector3(position.x + platformDistance, position.y, position.z),
                        Quaternion.identity
                    );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var platformController = platform.GetComponent<PlatformController>();
            platformController.Direction = direction;
            platformController.IsMoving = isMoving;

            return platform;
        }

        public GameObject LastPlatform()
        {
            return _platforms[_platforms.Count - 1];
        }

        public GameObject PreLastPlatform()
        {
            return _platforms[_platforms.Count - 2];
        }
    }
}