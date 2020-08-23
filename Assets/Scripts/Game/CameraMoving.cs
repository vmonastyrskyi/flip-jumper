﻿using System;
using UnityEngine;

namespace Game
{
    public class CameraMoving : MonoBehaviour
    {
        private const float CameraMovingTime = 4f;

        public Transform target;
        public Vector3 offset;

        private Vector3 _newPosition;
        private PlayerState _playerState;

        private void Start()
        {
            _newPosition = transform.position;

            GameEventSystem.current.OnSpawnDirectionChanged += CalculateNewPosition;

            PlayerEventSystem.current.OnStateChanged += state => { _playerState = state; };
        }

        private void CalculateNewPosition(SpawnDirection direction)
        {
            var position = target.position;
            switch (direction)
            {
                case SpawnDirection.Left:
                    _newPosition = new Vector3(
                        position.x + offset.x / .75f,
                        offset.y,
                        position.z + offset.z / 1.5f);
                    break;
                case SpawnDirection.Right:
                    _newPosition = new Vector3(
                        position.x + offset.x,
                        offset.y,
                        position.z + offset.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (_playerState == PlayerState.Idle)
                transform.position = Vector3.Lerp(transform.position, _newPosition, CameraMovingTime * Time.deltaTime);
        }
    }
}