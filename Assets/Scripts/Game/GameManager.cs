using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private const float StartPosition = 10f;

        public GameObject playerPrefab;

        private GameObject _scoreUiElement;
        private TextMeshProUGUI _scoreText;
        private Animator _scoreAnimator;
        
        private GameObject _hintUiElement;
        private Animator _hintAnimator;
        
        private int _score;

        private void Start()
        {
            _scoreUiElement = GameObject.FindWithTag("ScoreUIElement");
            _scoreText = _scoreUiElement.GetComponent<TextMeshProUGUI>();
            _scoreAnimator = _scoreUiElement.GetComponent<Animator>();
            
            _hintUiElement = GameObject.FindWithTag("HintUIElement");
            _hintAnimator = _hintUiElement.GetComponent<Animator>();

            GameEventSystem.current.OnIncreaseScore += IncreaseScore;

            PlayerEventSystem.current.OnStateChanged += HideHint;
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

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var player = Instantiate(playerPrefab, Vector3.up * StartPosition, Quaternion.identity);

            var mainCamera = Camera.main;
            if (mainCamera != null)
                mainCamera.GetComponent<CameraMoving>().target = player.transform;

            GetComponent<CloudGenerator>().Target = player.transform;
        }
    }
}