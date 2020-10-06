using System;
using UnityEngine;

namespace Game
{
    public class GameEventSystem : MonoBehaviour
    {
        public static GameEventSystem instance;

        private void Awake()
        {
            instance = this;
        }

        public event Action<SpawnDirection> OnDirectionChange;

        public void ChangeDirection(SpawnDirection spawnDirection)
        {
            OnDirectionChange?.Invoke(spawnDirection);
        }

        public event Action<SpawnDirection> OnPlatformCreate;

        public void CreatePlatform(SpawnDirection spawnDirection)
        {
            OnPlatformCreate?.Invoke(spawnDirection);
        }
        
        public event Action<SpawnDirection> OnCameraMove;

        public void MoveCamera(SpawnDirection spawnDirection)
        {
            OnCameraMove?.Invoke(spawnDirection);
        }
        
        public event Action<int> OnScoreUpdate;

        public void UpdateScore(int value)
        {
            OnScoreUpdate?.Invoke(value);
        }
        
        public event Action<int> OnCoinsUpdate;

        public void UpdateCoins(int value)
        {
            OnCoinsUpdate?.Invoke(value);
        }
    }
}