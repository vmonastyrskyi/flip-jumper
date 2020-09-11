using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Platform
{
    public class Moving : MonoBehaviour
    {
        private const float MovementTime = 2f;
        
        private SpawnDirection _spawnDirection;
        private Vector3 _finalPosition;
        private float _distance = 4f;

        private void Start()
        {
            _spawnDirection = GetComponent<PlatformManager>().Direction;

            var position = transform.position;
            switch (_spawnDirection)
            {
                case SpawnDirection.Left:
                    _finalPosition = new Vector3(position.x - _distance / 2, position.y, position.z);
                    break;
                case SpawnDirection.Right:
                    _finalPosition = new Vector3(position.x, position.y, position.z - _distance / 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transform.DOMove(_finalPosition, MovementTime / 2)
                .OnComplete(Move)
                .SetEase(Ease.Linear);
        }

        private void Move()
        {
            switch (_spawnDirection)
            {
                case SpawnDirection.Left:
                    _finalPosition = new Vector3(_finalPosition.x + _distance, _finalPosition.y, _finalPosition.z);
                    break;
                case SpawnDirection.Right:
                    _finalPosition = new Vector3(_finalPosition.x, _finalPosition.y, _finalPosition.z + _distance);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _distance *= -1;

            transform.DOMove(_finalPosition, MovementTime)
                .OnComplete(Move)
                .SetEase(Ease.Linear);
        }
    }
}