using System.Collections;
using Game.EventSystems;
using Loader;
using UnityEngine;

namespace Game.Systems
{
    public enum JumpDirection
    {
        Left,
        Right
    }

    public class GameSystem : MonoBehaviour
    {
        private const float StartPositionY = 16;
        private const float InitialDownVelocity = 9.81f;
        private const float ChanceToMovingPlatform = 0;
        private const int StepsToCoin = 10;
        private const int CenterStepsToCoin = 5;

        [SerializeField] private GameObject scorePopupPrefab;
        [SerializeField] private GameObject centeredStepPrefab;

        private JumpDirection _currentJumpDirection;
        private GameObject _player;
        private bool _isAvailableMoving;
        private int _steps;
        private int _centerSteps;
        private int _score;
        private int _coins;

        private void Awake()
        {
            Time.timeScale = 1;
        }

        private IEnumerator Start()
        {
            _currentJumpDirection = JumpDirection.Right;
            _isAvailableMoving = true;

            SpawnPlayer();
            SetCameraTarget();

            yield return null;

            PlatformEventSystem.Instance.OnVisited += centered =>
            {
                var isMoving = false;
                var nextJumpDirection = (JumpDirection) Random.Range(0, 2);

                if (_isAvailableMoving && _currentJumpDirection == nextJumpDirection)
                {
                    isMoving = Random.value <= ChanceToMovingPlatform;
                    _isAvailableMoving = !isMoving;
                }
                else if (!_isAvailableMoving)
                {
                    nextJumpDirection = _currentJumpDirection;
                    _isAvailableMoving = true;
                }

                GameEventSystem.Instance.MoveCamera(nextJumpDirection);
                GameEventSystem.Instance.GeneratePlatform(nextJumpDirection, isMoving);
                GameEventSystem.Instance.ChangeDirection(nextJumpDirection);

                var scorePopup = Instantiate(scorePopupPrefab, _player.transform).GetComponent<ScorePopup>();
                int earnedScore;
                
                if (centered)
                {
                    earnedScore = 5;
                    _score += earnedScore;
                    _centerSteps += 1;
                    Instantiate(centeredStepPrefab, _player.transform.parent);
                }
                else
                {
                    earnedScore = 1;
                    _score += earnedScore;
                    _centerSteps = 0;
                }
                _steps += 1;

                scorePopup.SetText("+" + earnedScore);
                GameEventSystem.Instance.UpdateScore(_score);

                if (_steps % StepsToCoin == 0 || _centerSteps == CenterStepsToCoin)
                {
                    GameEventSystem.Instance.GenerateCoin(nextJumpDirection);
                    _centerSteps = 0;
                } 

                _currentJumpDirection = nextJumpDirection;
            };

            GameEventSystem.Instance.OnCoinPickuped += () =>
            {
                GameEventSystem.Instance.UpdateCoins(++_coins);
            };
        }

        private void SpawnPlayer()
        {
            _player = Instantiate(
                DataManager.Instance.GameData.SelectedCharacter.Prefab,
                Vector3.up * StartPositionY,
                Quaternion.identity);
            _player.GetComponent<Rigidbody>().velocity = Vector3.down * InitialDownVelocity;
        }

        private void SetCameraTarget()
        {
            var mainCamera = Camera.main;

            if (mainCamera != null)
                mainCamera.GetComponent<CameraMoving>().Target = _player.transform;
        }
    }
}