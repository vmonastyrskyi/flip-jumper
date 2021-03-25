using System;
using System.Collections;
using Game.EventSystems;
using UnityEngine;

namespace Game.Systems
{
    public class CoinSystem : MonoBehaviour
    {
        [SerializeField] private GameObject coinPrefab;

        private GameObject _systems;
        private PlatformSystem _platformSystem;

        private void Awake()
        {
            _systems = GameObject.FindWithTag("Systems");
            _platformSystem = _systems.GetComponent<PlatformSystem>();
        }

        private IEnumerator Start()
        {
            yield return null;

            GameEventSystem.Instance.OnCoinGenerated += GenerateCoin;
        }

        private void GenerateCoin(JumpDirection direction)
        {
            var lastPlatform = _platformSystem.LastPlatform();
            var preLastPlatform = _platformSystem.PreLastPlatform();

            var lastPlatformPosition = lastPlatform.transform.position;
            var preLastPlatformPosition = preLastPlatform.transform.position;

            var positionX = preLastPlatformPosition.x + (lastPlatformPosition.x - preLastPlatformPosition.x) / 2;
            float positionY;
            var positionZ = preLastPlatformPosition.z + (lastPlatformPosition.z - preLastPlatformPosition.z) / 2;
            float rotationY;

            switch (direction)
            {
                case JumpDirection.Left:
                    positionY = (lastPlatformPosition.z - preLastPlatformPosition.z) / 2;
                    rotationY = 0;
                    break;
                case JumpDirection.Right:
                    positionY = (lastPlatformPosition.x - preLastPlatformPosition.x) / 2;
                    rotationY = 90;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Instantiate(
                coinPrefab,
                new Vector3(positionX, positionY, positionZ),
                Quaternion.Euler(0, rotationY, 0)
            );
        }
    }
}