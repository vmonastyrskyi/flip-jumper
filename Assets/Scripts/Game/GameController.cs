using System.Collections;
using Game.Player;
using Loader;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        private const float StartPosition = 16;
        private const float StartMenuDelay = 2;
        private const float InitialDownVelocity = 9.81f;

        public static GameController instance;

        [SerializeField] private GameObject pit;

        private Camera _mainCamera;
        private int _score;

        // UI
        private GameObject _scoreUiElement;
        private TextMeshProUGUI _scoreText;
        private Animator _scoreAnimator;
        private GameObject _hintUiElement;
        private Animator _hintAnimator;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _scoreUiElement = GameObject.FindWithTag("ScoreUIElement");
            _scoreText = _scoreUiElement.GetComponent<TextMeshProUGUI>();
            _scoreAnimator = _scoreUiElement.GetComponent<Animator>();

            _hintUiElement = GameObject.FindWithTag("HintUIElement");
            _hintAnimator = _hintUiElement.GetComponent<Animator>();

            GameEventSystem.instance.OnIncreaseScore += IncreaseScore;

            PlayerEventSystem.instance.OnStateChanged += HideHint;
            
            PlayerEventSystem.instance.OnStateChanged += GameOver;
        }

        private void IncreaseScore(int points)
        {
            _scoreText.SetText((_score += points).ToString());
            _scoreAnimator.Play("Increased");
        }

        private void HideHint(PlayerState state)
        {
            if (state != PlayerState.Start)
                _hintAnimator.Play("Hide");
        }
        
        private void GameOver(PlayerState state)
        {
            if (state == PlayerState.Dead)
            {
                Debug.Log("Game Over");
                StartCoroutine(LoadMenu());
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
            {
                var player = Instantiate(
                    DataManager.instance.SelectedCharacter,
                    Vector3.up * StartPosition,
                    Quaternion.identity);
                player.GetComponent<Rigidbody>().velocity = Vector3.down * InitialDownVelocity;

                _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
                if (_mainCamera != null)
                    _mainCamera.GetComponent<CameraMoving>().Target = player.transform;
                // var terrain = GameObject.FindGameObjectWithTag("Terrain");
                // var terrainPosition = terrain.transform.position;
                // terrain.transform.position =
                // new Vector3(terrainPosition.x + 4, terrainPosition.y, terrainPosition.y + 2);
                // terrain.transform.parent = _mainCamera.transform;
                pit.GetComponent<PlayerFollowing>().Target = player.transform;
            }
        }

        private static IEnumerator LoadMenu()
        {
            yield return new WaitForSeconds(StartMenuDelay);
            UiManager.instance.uiPage = UiPage.HomeOrPlay;
            SceneSystem.instance.LoadMenu();
        }
    }
}