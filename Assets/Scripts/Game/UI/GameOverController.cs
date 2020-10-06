using Loader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI earnedScoreLabel;
        [SerializeField] private TextMeshProUGUI earnedCoinsLabel;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button multiplyCoinsButton;
        [SerializeField] private Button playAgainButton;

        private void Awake()
        {
            if (homeButton != null)
                homeButton.onClick.AddListener(GoHome);
            if (multiplyCoinsButton != null)
                multiplyCoinsButton.onClick.AddListener(MultiplyCoins);
            if (playAgainButton != null)
                playAgainButton.onClick.AddListener(ReloadGame);
        }

        private void Start()
        {
            var earnedScore = TempDataManager.instance.EarnedScore;
            var earnedCoins = TempDataManager.instance.EarnedCoins;

            earnedScoreLabel.text = earnedScore.ToString();
            earnedCoinsLabel.text = earnedCoins.ToString();

            if (earnedCoins > 0)
                multiplyCoinsButton.gameObject.SetActive(true);

            DataManager.instance.UserData.Coins += earnedCoins;
        }

        private static void GoHome()
        {
            SceneSystem.instance.LoadMenu();
        }

        private static void MultiplyCoins()
        {
        }

        private void ReloadGame()
        {
            SceneSystem.instance.LoadGame();
        }
    }
}