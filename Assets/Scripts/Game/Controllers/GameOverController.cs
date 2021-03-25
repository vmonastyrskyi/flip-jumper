using System.Collections;
using Ads;
using Game.EventSystems;
using Loader;
using Save;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class GameOverController : MonoBehaviour
    {
        private const int CoinsMultiplayer = 2;

        [SerializeField] private Animator sceneTransitionAnimator;
        [SerializeField] private TextMeshProUGUI earnedScoreLabel;
        [SerializeField] private TextMeshProUGUI newRecordLabel;
        [SerializeField] private TextMeshProUGUI earnedCoinsLabel;
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
            
            if (_earnedCoins > 0)
                multiplyCoinsButton.gameObject.SetActive(true);

            var data = SaveSystem.Load();

            data.Coins += _earnedCoins;
            _gameData.Coins += _earnedCoins;

            var highScore = data.HighScore;
            if (highScore < _earnedScore)
            {
                data.HighScore = _earnedScore;
                _gameData.HighScore = _earnedScore;
                newRecordLabel.gameObject.SetActive(true);
            }

            SaveSystem.Save(data);
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

        private void MultiplyCoins()
        {
            var multipliedCoins = _earnedCoins * CoinsMultiplayer;
            earnedCoinsLabel.text = multipliedCoins.ToString();
            
            var data = SaveSystem.Load();

            data.Coins += multipliedCoins - _earnedCoins;
            _gameData.Coins += multipliedCoins - _earnedCoins;

            SaveSystem.Save(data);

            multiplyCoinsButton.onClick.RemoveAllListeners();
            multiplyCoinsButton.gameObject.SetActive(false);
        }

        private IEnumerator ReloadGame()
        {
            sceneTransitionAnimator.SetTrigger(FadeIn);

            yield return new WaitForSeconds(0.25f);

            SceneManager.Instance.LoadGame();
        }
    }
}