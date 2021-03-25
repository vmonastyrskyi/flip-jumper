using System;
using Game.Systems;
using UnityEngine;

namespace Game.EventSystems
{
    public class GameEventSystem : MonoBehaviour
    {
        public static GameEventSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public event Action<JumpDirection> OnDirectionChanged;

        public void ChangeDirection(JumpDirection jumpDirection)
        {
            OnDirectionChanged?.Invoke(jumpDirection);
        }

        public event Action<JumpDirection, bool> OnPlatformGenerate;

        public void GeneratePlatform(JumpDirection jumpDirection, bool isMoving)
        {
            OnPlatformGenerate?.Invoke(jumpDirection, isMoving);
        }

        public event Action<JumpDirection> OnCameraMove;

        public void MoveCamera(JumpDirection jumpDirection)
        {
            OnCameraMove?.Invoke(jumpDirection);
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
        
        public event Action<JumpDirection> OnCoinGenerated;

        public void GenerateCoin(JumpDirection jumpDirection)
        {
            OnCoinGenerated?.Invoke(jumpDirection);
        }

        public event Action OnCoinPickuped;

        public void PickupCoin()
        {
            OnCoinPickuped?.Invoke();
        }
        
        public event Action OnRewardedForVideo;

        public void RewardForVideo()
        {
            OnRewardedForVideo?.Invoke();
        }

        public event Action OnGameStarted;

        public void StartGame()
        {
            OnGameStarted?.Invoke();
        }

        public event Action OnGamePaused;

        public void PauseGame()
        {
            OnGamePaused?.Invoke();
        }

        public event Action OnGameResumed;

        public void ResumeGame()
        {
            OnGameResumed?.Invoke();
        }

        public event Action OnGameOver;

        public void GameOver()
        {
            OnGameOver?.Invoke();
        }
    }
}