using System;
using UnityEngine;

namespace Game
{
    public class PlatformEventSystem : MonoBehaviour
    {
        public static PlatformEventSystem current;

        private void Awake()
        {
            current = this;
        }

        public event Action<string, bool> OnPlayerStepped;

        public void SetIsPlayerStepped(string guid, bool isStepped)
        {
            OnPlayerStepped?.Invoke(guid, isStepped);
        }
    }
}
