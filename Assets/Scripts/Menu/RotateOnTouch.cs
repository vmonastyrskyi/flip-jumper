using UnityEngine;
using Util;

namespace Menu
{
    public class RotateOnTouch : MonoBehaviour
    {
        private const float RotationSpeed = 0.25f;

        private Vector3 _currentPosition;
        private Vector3 _deltaPosition;
        private Vector3 _lastPosition;

        private Rotate _rotate;

        private void Start()
        {
            _rotate = GetComponent<Rotate>();
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (!PointerEventSystem.IsPointerOverGameObject(touch.position, new[] {"Button"}))
                    {
                        _rotate.enabled = false;
                    }
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    if (!PointerEventSystem.IsPointerOverGameObject(touch.position, new[] {"Button"}))
                    {
                        var rotationY = Quaternion.Euler(0, -touch.deltaPosition.x * RotationSpeed, 0);
                        transform.rotation *= rotationY;
                        _rotate.enabled = false;
                    }
                    else
                    {
                        _rotate.enabled = true;
                    }
                }
            }
            else
            {
                _rotate.enabled = true;
            }
        }
    }
}