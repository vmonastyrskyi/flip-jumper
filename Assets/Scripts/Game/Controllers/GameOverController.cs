using System;
using System.Collections;
using Ads;
using Game.EventSystems;
using Loader;
using LocalSave;
using PlayGames;
using PlayGames.Dao;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using Util;
using Button = UnityEngine.UI.Button;

namespace Game.Controllers
{
    public class GameOverController : MonoBehaviour
    {
        private const int CoinsMultiplayer = 2;

        [SerializeField] private Animator sceneTransitionAnimator;

        [Header("Score Panel")]
        [SerializeField] private TextMeshProUGUI earnedScoreLabel;
        [SerializeField] private TextMeshProUGUI newRecordLabel;

        [Header("Coins Panel")]
        [SerializeField] private TextMeshProUGUI earnedCoinsLabel;

        [Header("Buttons")]
        [SerializeField] private Button homeButton;
        [SerializeField] private Button multiplyCoinsButton;
        [SerializeField] private Button playAgainButton;

        private GameData _gameData;
        private int _earnedScore;
        private int _earnedCoins;

        private static readonly int FadeIn = Animator.StringToHash("Fade_In");

        private void Awake()
        {
            if (homeButton != null)
                homeButton.onClick.AddListener(() => StartCoroutine(LoadMenu()));
            if (multiplyCoinsButton != null)
                multiplyCoinsButton.onClick.AddListener(ShowNonSkippableVideo);
            if (playAgainButton != null)
                playAgainButton.onClick.AddListener(() => StartCoroutine(ReloadGame()));
        }

        private IEnumerator LoadMenu()
        {
            sceneTransitionAnimator.SetTrigger(FadeIn);

            yield return new WaitForSeconds(0.25f);

            SceneManager.Instance.LoadMenu();
        }

        private void ShowNonSkippableVideo()
        {
            AdsManager.ShowNonSkippableVideo(AdsManager.Placement.MultiplyCoins);
        }

        private IEnumerator ReloadGame()
        {
            sceneTransitionAnimator.SetTrigger(FadeIn);

            yield return new WaitForSeconds(0.25f);

            SceneManager.Instance.LoadGame();
        }

        private IEnumerator Start()
        {
            _gameData = DataManager.Instance.GameData;

            yield return null;

            GameEventSystem.Instance.OnScoreUpdated += score => _earnedScore = score;
            GameEventSystem.Instance.OnCoinsUpdated += coins => _earnedCoins = coins;
            GameEventSystem.Instance.OnRewardedForVideo += MultiplyCoins;
            GameEventSystem.Instance.OnGameOver += SaveProgress;
        }

        private void SaveProgress()
        {
            earnedScoreLabel.text = _earnedScore.ToString();
            earnedCoinsLabel.text = _earnedCoins.ToString();

            if (_earnedCoins > 0 && InternetConnection.Available())
                multiplyCoinsButton.gameObject.SetActive(true);

            var localData = LocalSaveSystem.LoadLocalData();

            var highScore = _gameData.HighScore;
            if (highScore < _earnedScore)
            {
                localData.HighScore = _earnedScore;
                _gameData.HighScore = _earnedScore;
                newRecordLabel.gameObject.SetActive(true);

                if (PlayGamesServices.IsAuthenticated && InternetConnection.Available())
                    PlayGamesServices.ReportScore(Gps.LeaderboardHighScore, _earnedScore);
            }

            localData.SaveTime = DateTime.Now.Ticks;
            localData.Coins += _earnedCoins;
            _gameData.Coins += _earnedCoins;

            if (PlayGamesServices.IsAuthenticated && InternetConnection.Available())
                PlayGamesServices.SaveCloudData(CloudData.FromLocalData(localData));
            LocalSaveSystem.SaveLocalData(localData);
        }

        private void MultiplyCoins()
        {
            var multipliedCoins = _earnedCoins * CoinsMultiplayer;
            earnedCoinsLabel.text = multipliedCoins.ToString();

            var localData = LocalSaveSystem.LoadLocalData();

            localData.SaveTime = DateTime.Now.Ticks;
            localData.Coins += multipliedCoins - _earnedCoins;
            _gameData.Coins += multipliedCoins - _earnedCoins;

            if (PlayGamesServices.IsAuthenticated && InternetConnection.Available())
                PlayGamesServices.SaveCloudData(CloudData.FromLocalData(localData));
            LocalSaveSystem.SaveLocalData(localData);

            multiplyCoinsButton.onClick.RemoveAllListeners();
            multiplyCoinsButton.gameObject.SetActive(false);
        }
    }
}