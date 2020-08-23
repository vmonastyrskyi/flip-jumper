using System;
using UnityEngine;

namespace Game
{
    public class PlayerEventSystem : MonoBehaviour
    {
        public static PlayerEventSystem current;

        private void Awake()
        {
            current = this;
        }
        
        public event Action<PlayerState> OnStateChanged;

        public void ChangeState(PlayerState playerState)
        {
            OnStateChanged?.Invoke(playerState);
        }
    }
}