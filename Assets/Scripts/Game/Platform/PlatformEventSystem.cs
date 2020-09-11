using System;
using UnityEngine;

namespace Game.Platform
{
    public class PlatformEventSystem : MonoBehaviour
    {
        public static PlatformEventSystem instance;

        private void Awake()
        {
            instance = this;
        }

        public event Action<string, bool> OnPlayerStepped;

        public void SetIsPlayerStepped(string guid, bool isStepped)
        {
            OnPlayerStepped?.Invoke(guid, isStepped);
        }
    }
}
