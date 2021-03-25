using System.Collections;
using Ads;
using Loader;
using Menu.Store.EventSystems;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Controllers
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Animator sceneTransitionAnimator;
        [SerializeField] private GameObject platformWithPlayer;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject rankingPanel;
        [SerializeField] private GameObject storePanel;
        [SerializeField] private GameObject coinsPanel;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button storeButton;
        [SerializeField] private Button rankingButton;
        [SerializeField] private Button playButton;

        private GameData _gameData;
        private TextMeshProUGUI _coinsLabel;
        
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

            _coinsLabel = coinsPanel.GetComponentInChildren<TextMeshProUGUI>();
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

            StoreEventSystem.Instance.OnSuccessfulPurchase += UpdateCoinsLabel;

            AdsManager.ShowBanner();
        }

        private void UpdateCoinsLabel()
        {
            _coinsLabel.SetText(_gameData.Coins.ToString());
        }
    }
}