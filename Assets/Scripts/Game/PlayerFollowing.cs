using UnityEngine;

namespace Game
{
    public class PlayerFollowing : MonoBehaviour
    {
        private Transform _target;

        public Transform Target
        {
            set => _target = value;
        }

        private void Update()
        {
            var targetPosition = _target.position;
            transform.position = new Vector3(targetPosition.x, 0, targetPosition.z);
        }
    }
}
