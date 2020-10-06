using System;
using UnityEngine;

namespace Menu
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

        private void Update()
        {
            switch (rotationDirection)
            {
                case RotationDirection.Clockwise:
                    transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
                    break;
                case RotationDirection.Counterclockwise:
                    transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}