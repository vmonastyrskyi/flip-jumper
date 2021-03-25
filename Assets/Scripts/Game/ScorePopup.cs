using TMPro;
using UnityEngine;

namespace Game
{
    public class ScorePopup : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            if (Camera.main != null)
            {
                _mainCamera = Camera.main;
            }
        }

        private void Update()
        {
            var cameraRotation = _mainCamera.transform.rotation;

            transform.LookAt(transform.position + cameraRotation * Vector3.forward,
                cameraRotation * Vector3.up);
        }

        public void SetText(string text)
        {
            GetComponent<TextMeshPro>().text = text;
        }
    }
}