using System;
using PlayGames;
using PlayGames.Dao;
using UnityEngine;
using Util;

namespace Menu.Ranking
{
    public class LeaderboardEventSystem : MonoBehaviour
    {
        public static LeaderboardEventSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event Action<UserProfile> OnUserLoaded;

        public void UserLoaded(UserProfile user)
        {
            OnUserLoaded?.Invoke(user);
        }
    }
}