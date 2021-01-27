using System.Collections;
using Game.EventSystems;
using Game.Systems;
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

        private IEnumerator Start()
        {
            yield return null;
            
            PlatformEventSystem.instance.OnPlayerStepped += guid =>
            {
                if (Guid == guid && !Visited)
                {
                    PlatformEventSystem.instance.Visited();
                    Visited = true;
                }
            };
        }
    }
}