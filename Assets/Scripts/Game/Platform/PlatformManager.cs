using UnityEngine;

namespace Game.Platform
{
    public class PlatformManager : MonoBehaviour
    {
        public bool Visited { private get; set; }

        public SpawnDirection Direction { get; set; }

        public string Guid { get; private set; }

        private void Awake()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        private void Start()
        {
            PlatformEventSystem.instance.OnPlayerStepped += guid =>
            {
                if (Guid == guid && !Visited)
                {
                    PlatformEventSystem.instance.Visited();
                    // GameEventSystem.instance.CreatePlatform();
                    // GameEventSystem.instance.IncreaseScore(1);
                    Visited = true;
                }
            };
        }
    }
}