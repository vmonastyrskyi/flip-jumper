using System.Collections;
using Ads;
using Loader;
using Menu.Settings;
using Menu.Store.EventSystems;
using PlayGames;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using Util;
using Button = UnityEngine.UI.Button;

namespace Menu.Controllers
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Animator sceneTransitionAnimator;
        [SerializeField] private GameObject platformWithPlayer;

        [Header("Panels")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject rankingPanel;
        [SerializeField] private GameObject storePanel;

        [Header("Coins Panel")]
        [SerializeField] private TextMeshProUGUI coinsLabel;

        [Header("Buttons")]
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button storeButton;
        [SerializeField] private Button rankingButton;
        [SerializeField] private Button playButton;

        private GameData _gameData;

        private static readonly int FadeIn = Animator.StringToHash("Fade_In");

        private void Awake()
        {
            Time.timeScale = 1;

            if (settingsButton != null)
                settingsButton.onClick.AddListener(OpenSettings);
            if (storeButton != null)
                storeButton.onClick.AddListener(OpenStore);
            if (rankingButton != null)
                rankingButton.onClick.AddListener(OpenRanking);
            if (playButton != null)
                playButton.onClick.AddListener(() => StartCoroutine(Play()));

            ToggleRankingButton(PlayGamesServices.IsAuthenticated && InternetConnection.Available());

            PlayGamesServicesEventSystem.Instance.OnUserAuthenticated += ToggleRankingButton;

            SettingsEventSystem.Instance.OnGameDataUpdated += UpdateCoinsLabel;

            StoreEventSystem.Instance.OnSuccessfulPurchase += UpdateCoinsLabel;
        }
        
        private void OpenSettings()
        {
            platformWithPlayer.SetActive(false);
            menuPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        private void OpenStore()
        {
            platformWithPlayer.SetActive(false);
            menuPanel.SetActive(false);
            storePanel.SetActive(true);
        }

        private void OpenRanking()
        {
            platformWithPlayer.SetActive(false);
            menuPanel.SetActive(false);
            rankingPanel.SetActive(true);
        }

        private IEnumerator Play()
        {
            sceneTransitionAnimator.SetTrigger(FadeIn);

            yield return new WaitForSeconds(0.25f);

            SceneManager.Instance.LoadGame();
        }

        private IEnumerator Start()
        {
            _gameData = DataManager.Instance.GameData;

            yield return null;

            UpdateCoinsLabel();

            AdsManager.ShowBanner();
        }

        private void UpdateCoinsLabel()
        {
            coinsLabel.SetText(_gameData.Coins.ToString());
        }

        private void ToggleRankingButton(bool active)
        {
            rankingButton.interactable = active;
        }
    }
}