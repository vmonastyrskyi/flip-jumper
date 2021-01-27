using System;
using System.Collections;
using DG.Tweening;
using Game.EventSystems;
using Game.Systems;
using UnityEngine;

namespace Game
{
    public class CameraMoving : MonoBehaviour
    {
        private const float CameraMovingDuration = 1;

        [SerializeField] private Vector3 offset;

        public Transform Target { private get; set; }

        private IEnumerator Start()
        {
            yield return null;

            GameEventSystem.instance.OnCameraMoving += CalculateNewPosition;
        }

        private void CalculateNewPosition(SpawnDirection direction)
        {
            var targetPosition = Target.position;
            Vector3 newPosition;
            
            switch (direction)
            {
                case SpawnDirection.Left:
                    newPosition = new Vector3(
                        targetPosition.x + offset.x / .75f,
                        offset.y,
                        targetPosition.z + offset.z / 1.5f);
                    break;
                case SpawnDirection.Right:
                    newPosition = new Vector3(
                        targetPosition.x + offset.x,
                        offset.y,
                        targetPosition.z + offset.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transform.DOMove(newPosition, CameraMovingDuration).SetEase(Ease.OutQuint);
        }
    }
}