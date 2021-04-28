using System;
using UnityEngine;

namespace PlayGames
{
    public class PlayGamesServicesEventSystem : MonoBehaviour
    {
        public static PlayGamesServicesEventSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event Action<bool> OnUserAuthenticated;

        public void UserAuthenticated(bool authenticated)
        {
            OnUserAuthenticated?.Invoke(authenticated);
        }
    }
}