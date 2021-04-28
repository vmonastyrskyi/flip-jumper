using System;
using UnityEngine;

namespace Game.EventSystems
{
    public class PlatformEventSystem : MonoBehaviour
    {
        public static PlatformEventSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event Action<string, bool> OnPlayerStepped;

        public void PlayerStepped(string guid, bool centered)
        {
            OnPlayerStepped?.Invoke(guid, centered);
        }

        public event Action<bool> OnVisited;

        public void Visited(bool centered)
        {
            OnVisited?.Invoke(centered);
        }
    }
}
