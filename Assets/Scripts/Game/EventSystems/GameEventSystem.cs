using System;
using Game.Systems;
using UnityEngine;

namespace Game.EventSystems
{
    public class GameEventSystem : MonoBehaviour
    {
        public static GameEventSystem instance;

        private void Awake()
        {
            instance = this;
        }

        public event Action<SpawnDirection> OnDirectionChanged;

        public void ChangeDirection(SpawnDirection spawnDirection)
        {
            OnDirectionChanged?.Invoke(spawnDirection);
        }

        public event Action<SpawnDirection> OnGeneratingPlatform;

        public void GeneratePlatform(SpawnDirection spawnDirection)
        {
            OnGeneratingPlatform?.Invoke(spawnDirection);
        }

        public event Action<SpawnDirection> OnCameraMoving;

        public void MoveCamera(SpawnDirection spawnDirection)
        {
            OnCameraMoving?.Invoke(spawnDirection);
        }

        public event Action<int> OnScoreUpdated;

        public void UpdateScore(int value)
        {
            OnScoreUpdated?.Invoke(value);
        }

        public event Action<int> OnCoinsUpdated;

        public void UpdateCoins(int value)
        {
            OnCoinsUpdated?.Invoke(value);
        }
        
        public event Action OnCoinGenerated;

        public void GenerateCoin()
        {
            OnCoinGenerated?.Invoke();
        }
        
        public event Action OnGameStarted;

        public void StartGame()
        {
            OnGameStarted?.Invoke();
        }
        
        public event Action OnGameOver;

        public void GameOver()
        {
            OnGameOver?.Invoke();
        }
    }
}