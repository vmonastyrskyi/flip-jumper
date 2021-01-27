using System.Collections;
using Game.EventSystems;
using UnityEngine;

namespace Game.Systems
{
    public enum SpawnDirection
    {
        Left,
        Right
    }

    public class GameSystem : MonoBehaviour
    {
        private const float StartPosition = 16;
        private const float InitialDownVelocity = 9.81f;
        private const int ScoreToCoin = 1; // TODO change

        [SerializeField] private GameObject pit;

        private SpawnDirection _currentSpawnDirection;
        private GameObject _player;
        private int _score;
        private int _coins;

        private void Awake()
        {
            Time.timeScale = 1;
        }

        private IEnumerator Start()
        {
            _currentSpawnDirection = SpawnDirection.Right;

            SpawnPlayer();
            SetCameraTarget();
            SetPitTarget();

            yield return null;

            PlatformEventSystem.instance.OnVisited += () =>
            {
                _currentSpawnDirection = (SpawnDirection) Random.Range(0, 2);

                GameEventSystem.instance.MoveCamera(_currentSpawnDirection);
                GameEventSystem.instance.GeneratePlatform(_currentSpawnDirection);
                GameEventSystem.instance.ChangeDirection(_currentSpawnDirection);
                GameEventSystem.instance.UpdateScore(++_score);

                if (_score % ScoreToCoin == 0)
                    GameEventSystem.instance.GenerateCoin();
            };
        }

        private void SpawnPlayer()
        {
            _player = Instantiate(
                DataManager.instance.UserData.SelectedCharacter.Prefab,
                Vector3.up * StartPosition,
                Quaternion.identity);
            _player.GetComponent<Rigidbody>().velocity = Vector3.down * InitialDownVelocity;
        }

        private void SetCameraTarget()
        {
            var mainCamera = Camera.main;
            if (mainCamera != null)
                mainCamera.GetComponent<CameraMoving>().Target = _player.transform;
        }

        private void SetPitTarget()
        {
            pit.GetComponent<PlayerFollowing>().Target = _player.transform;
        }
    }
}