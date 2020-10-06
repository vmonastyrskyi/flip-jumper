using Game.Platform;
using Scriptable_Objects;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        private const float StartPosition = 16;
        private const float InitialDownVelocity = 9.81f;
        private const int ScoreToCoin = 5;

        [SerializeField] private GameObject pit;

        private Camera _mainCamera;
        private SpawnDirection _currentSpawnDirection;
        private GameObject _player;
        private int _score;
        private int _coins;

        private readonly Random _random = new Random();
        
        private void Awake()
        {
            Time.timeScale = 1;
        }

        private void Start()
        {
            _currentSpawnDirection = SpawnDirection.Right;
            
            SpawnPlayer();
            SetCameraTarget();
            SetPitTarget();

            PlatformEventSystem.instance.OnVisited += () =>
            {
                _currentSpawnDirection = (SpawnDirection) _random.Next(0, 2);

                GameEventSystem.instance.MoveCamera(_currentSpawnDirection);
                GameEventSystem.instance.CreatePlatform(_currentSpawnDirection);
                GameEventSystem.instance.ChangeDirection(_currentSpawnDirection);
                GameEventSystem.instance.UpdateScore(++_score);

                if (_score % ScoreToCoin == 0)
                    GameEventSystem.instance.UpdateCoins(++_coins);

                TempDataManager.instance.EarnedScore = _score;
                TempDataManager.instance.EarnedCoins = _coins;
            };
        }

        private void SpawnPlayer()
        {
            _player = Instantiate(
                DataManager.instance.UserData.SelectedCharacter.Prefab,
                Vector3.up * StartPosition,
                Quaternion.identity);
            // _player.GetComponent<Rigidbody>().velocity = Vector3.down * InitialDownVelocity;
        }

        private void SetCameraTarget()
        {
            _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            if (_mainCamera != null)
                _mainCamera.GetComponent<CameraMoving>().Target = _player.transform;
        }

        private void SetPitTarget()
        {
            pit.GetComponent<PlayerFollowing>().Target = _player.transform;
        }
    }
}