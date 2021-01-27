using System;
using UnityEngine;

namespace Game.EventSystems
{
    public class PlatformEventSystem : MonoBehaviour
    {
        public static PlatformEventSystem instance;

        private void Awake()
        {
            instance = this;
        }

        public event Action<string> OnPlayerStepped;

        public void PlayerStepped(string guid)
        {
            OnPlayerStepped?.Invoke(guid);
        }

        public event Action OnVisited;

        public void Visited()
        {
            OnVisited?.Invoke();
        }
    }
}
