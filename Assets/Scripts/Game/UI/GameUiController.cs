using System.Collections;
using Game.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameUiController : MonoBehaviour
    {
        private const float GameOverDelay = 2;
        
        [SerializeField] private TextMeshProUGUI coinsLabel;
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI hintLabel;
        [SerializeField] private Button pauseButton;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;

        private Animator _scoreAnimator;
        private Animator _hintAnimator;
        
        private void Awake()
        {
            if (pauseButton != null)
                pauseButton.onClick.AddListener(PauseGame);

            _scoreAnimator = scoreLabel.GetComponent<Animator>();
            _hintAnimator = hintLabel.GetComponent<Animator>();
        }

        private void Start()
        {
            GameEventSystem.instance.OnScoreUpdate += UpdateScore;
            GameEventSystem.instance.OnCoinsUpdate += UpdateCoins;
            PlayerEventSystem.instance.OnStateChanged += HideHint;
            PlayerEventSystem.instance.OnStateChanged += GameOver;
        }
        
        private void UpdateScore(int value)
        {
            scoreLabel.text = value.ToString();

            if (value % 10 == 0)
                _scoreAnimator.Play("Increased");
        }
        
        private void UpdateCoins(int value)
        {
            coinsLabel.text = value.ToString();
        }
        
        private void HideHint(PlayerState state)
        {
            if (state != PlayerState.Start)
                _hintAnimator.Play("Hide");
        }

        private void GameOver(PlayerState state)
        {
            if (state == PlayerState.Dead)
                StartCoroutine(ShowGameOverPanel());
        }

        private IEnumerator ShowGameOverPanel()
        {
            Debug.Log("Game Over");
            yield return new WaitForSeconds(GameOverDelay);
            gameOverPanel.SetActive(true);
        }

        private void PauseGame()
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}