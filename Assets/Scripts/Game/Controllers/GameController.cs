using System.Collections;
using Ads;
using Game.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        private const float GameOverDelay = 3;
        private const float TimeToHideHint = 0.5f;

        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button pauseButton;
        [SerializeField] private TextMeshProUGUI coinsLabel;
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI hintLabel;

        private Animator _hintAnimator;

        private void Awake()
        {
            if (pauseButton != null)
                pauseButton.onClick.AddListener(PauseGame);

            _hintAnimator = hintLabel.GetComponent<Animator>();
        }

        private IEnumerator Start()
        {
            yield return null;

            GameEventSystem.Instance.OnScoreUpdated += UpdateScore;
            GameEventSystem.Instance.OnCoinsUpdated += UpdateCoins;
            GameEventSystem.Instance.OnGameStarted += () => StartCoroutine(HideHint());
            GameEventSystem.Instance.OnGameOver += () => StartCoroutine(ShowGameOverPanel());

            AdsManager.ShowBanner();
        }

        private void UpdateScore(int value)
        {
            scoreLabel.text = value.ToString();
        }

        private void UpdateCoins(int value)
        {
            coinsLabel.text = value.ToString();
        }

        private IEnumerator HideHint()
        {
            _hintAnimator.Play("Hide");

            yield return new WaitForSeconds(TimeToHideHint);

            hintLabel.gameObject.SetActive(false);
        }

        private IEnumerator ShowGameOverPanel()
        {
            yield return new WaitForSeconds(GameOverDelay);

            gamePanel.SetActive(false);
            gameOverPanel.SetActive(true);
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
            gamePanel.SetActive(false);
            pausePanel.SetActive(true);
            GameEventSystem.Instance.PauseGame();
        }
    }
}