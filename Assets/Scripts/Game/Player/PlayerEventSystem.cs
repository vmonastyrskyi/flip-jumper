using System;
using UnityEngine;

namespace Game.Player
{
    public class PlayerEventSystem : MonoBehaviour
    {
        public static PlayerEventSystem instance;

        private void Awake()
        {
            instance = this;
        }
        
        public event Action<PlayerState> OnStateChanged;

        public void ChangeState(PlayerState playerState)
        {
            OnStateChanged?.Invoke(playerState);
        }
    }
}