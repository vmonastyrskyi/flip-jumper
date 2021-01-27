using Scriptable_Objects;
using UnityEngine;

namespace Menu.Store
{
    public class PlatformItem : MonoBehaviour
    {
        [SerializeField] private Platform platform;

        public Platform Platform => platform;
        public GameObject PlatformPrefab { get; private set; }

        private void Awake()
        {
            PlatformPrefab = transform.GetChild(0).gameObject;
        }
    }
}