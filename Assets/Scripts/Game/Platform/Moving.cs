using System;
using DG.Tweening;
using Game.Controllers;
using Game.Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Platform
{
    public class Moving : MonoBehaviour
    {
        private JumpDirection _jumpDirection;
        private Vector3 _finalPosition;
        private float _movementTime;
        private float _range;

        private void Start()
        {
            _jumpDirection = GetComponent<PlatformController>().Direction;
            _movementTime = Random.Range(1.5f, 3f);
            _range = Random.Range(4f, 8f);
            
            
            var position = transform.position;
            switch (_jumpDirection)
            {
                case JumpDirection.Left:
                    _finalPosition = new Vector3(position.x - _range / 2, position.y, position.z);
                    break;
                case JumpDirection.Right:
                    _finalPosition = new Vector3(position.x, position.y, position.z - _range / 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transform.DOMove(_finalPosition, _movementTime / 2)
                .OnComplete(Move)
                .SetEase(Ease.Linear);
        }

        private void Move()
        {
            switch (_jumpDirection)
            {
                case JumpDirection.Left:
                    _finalPosition = new Vector3(_finalPosition.x + _range, _finalPosition.y, _finalPosition.z);
                    break;
                case JumpDirection.Right:
                    _finalPosition = new Vector3(_finalPosition.x, _finalPosition.y, _finalPosition.z + _range);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _range *= -1;

            transform.DOMove(_finalPosition, _movementTime)
                .OnComplete(Move)
                .SetEase(Ease.Linear);
        }
    }
}