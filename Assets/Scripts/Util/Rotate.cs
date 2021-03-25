using System;
using System.Timers;
using UnityEngine;

namespace Util
{
    public class Rotate : MonoBehaviour
    {
        private enum RotationDirection
        {
            Clockwise,
            Counterclockwise
        }

        [SerializeField] private RotationDirection rotationDirection;
        [SerializeField, Min(0)] private int rotationSpeed;
        [SerializeField] private bool unscaledTime;

        private float _deltaTime;
        
        private void Update()
        {
            _deltaTime = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            
            switch (rotationDirection)
            {
                case RotationDirection.Clockwise:
                    transform.Rotate(0, rotationSpeed * _deltaTime, 0);
                    break;
                case RotationDirection.Counterclockwise:
                    transform.Rotate(0, -rotationSpeed * _deltaTime, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}