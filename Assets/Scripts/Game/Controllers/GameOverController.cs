using System.Collections;
using Game.EventSystems;
using Loader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class GameOverController : MonoBehaviour
    {
        private const int CoinsMultiplayer = 2;

        [SerializeField] private TextMeshProUGUI earnedScoreLabel;
        [SerializeField] private TextMeshProUGUI earnedCoinsLabel;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button multiplyCoinsButton;
        [SerializeField] private Button playAgainButton;

        private int _earnedScore;
        private int _earnedCoins;

        private void Awake()
        {
            if (homeButton != null)
                homeButton.onClick.AddListener(LoadMenu);
            if (multiplyCoinsButton != null)
                multiplyCoinsButton.onClick.AddListener(MultiplyCoins);
            if (playAgainButton != null)
                playAgainButton.onClick.AddListener(ReloadGame);
        }

        private IEnumerator Start()
        {
            yield return null;

            GameEventSystem.instance.OnScoreUpdated += score => _earnedScore = score;
            GameEventSystem.instance.OnCoinsUpdated += coins => _earnedCoins = coins;
            GameEventSystem.instance.OnGameOver += UpdateProgress;
        }

        private void UpdateProgress()
        {
            earnedScoreLabel.text = _earnedScore.ToString();
            earnedCoinsLabel.text = _earnedCoins.ToString();

            if (_earnedCoins > 0)
                multiplyCoinsButton.gameObject.SetActive(true);

            DataManager.instance.UserData.Coins += _earnedCoins;
        }

        private static void LoadMenu()
        {
            SceneSystem.instance.LoadMenu();
        }

        private void MultiplyCoins()
        {
            var multipliedCoins = _earnedCoins * CoinsMultiplayer;
            earnedCoinsLabel.text = multipliedCoins.ToString();
            DataManager.instance.UserData.Coins += multipliedCoins - _earnedCoins;

            multiplyCoinsButton.onClick.RemoveListener(MultiplyCoins);
            multiplyCoinsButton.gameObject.SetActive(false);
        }

        private static void ReloadGame()
        {
            SceneSystem.instance.LoadGame();
        }
    }
}