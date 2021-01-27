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
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject storePanel;
        [SerializeField] private GameObject rankingPanel;
        [SerializeField] private GameObject settingsPanel;
        
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button storeButton;
        [SerializeField] private Button rankingButton;
        [SerializeField] private Button playButton;
        
        [SerializeField] private GameObject coinsPanel;
        [SerializeField] private GameObject gemsPanel;

        private UserData _userData;
        private TextMeshProUGUI _coinsLabel;
        private TextMeshProUGUI _gemsLabel;

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
                playButton.onClick.AddListener(Play);

            _coinsLabel = coinsPanel.GetComponentInChildren<TextMeshProUGUI>();
            _gemsLabel = gemsPanel.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OpenSettings()
        {
            menuPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        private void OpenStore()
        {
            menuPanel.SetActive(false);
            storePanel.SetActive(true);
        }

        private void OpenRanking()
        {
            menuPanel.SetActive(false);
            rankingPanel.SetActive(true);
        }

        private void Play()
        {
            SceneSystem.instance.LoadGame();
        }

        private void Start()
        {
            _userData = DataManager.instance.UserData;

            SetCashInformation();

            CharactersPageEventSystem.instance.OnSuccessfulCharacterPurchase += SetCashInformation;
        }

        private void SetCashInformation()
        {
            _coinsLabel.SetText(_userData.Coins.ToString());
            _gemsLabel.SetText(_userData.Gems.ToString());
        }
    }
}