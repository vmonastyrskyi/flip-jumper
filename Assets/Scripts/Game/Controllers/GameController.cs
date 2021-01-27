using System.Collections;
using Game.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        private const float GameOverDelay = 2;

        [SerializeField] private TextMeshProUGUI coinsLabel;
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI hintLabel;
        
        [SerializeField] private Button pauseButton;
        
        [SerializeField] private GameObject gamePanel;
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

        private IEnumerator Start()
        {
            yield return null;

            GameEventSystem.instance.OnScoreUpdated += UpdateScore;
            GameEventSystem.instance.OnCoinsUpdated += UpdateCoins;
            GameEventSystem.instance.OnGameStarted += () => StartCoroutine(HideHint());
            GameEventSystem.instance.OnGameOver += () => StartCoroutine(ShowGameOverPanel());
        }

        private void UpdateScore(int value)
        {
            scoreLabel.text = value.ToString();

            if (value % 5 == 0)
                _scoreAnimator.Play("Increased");
        }

        private void UpdateCoins(int value)
        {
            coinsLabel.text = value.ToString();
        }

        private IEnumerator HideHint()
        {
            _hintAnimator.Play("Hide");

            yield return new WaitForSeconds(.5f);

            hintLabel.gameObject.SetActive(false);
        }

        private IEnumerator ShowGameOverPanel()
        {
            Debug.Log("Game Over");
            yield return new WaitForSeconds(GameOverDelay);

            gamePanel.SetActive(false);
            gameOverPanel.SetActive(true);
        }

        private void PauseGame()
        {
            gamePanel.SetActive(false);
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}