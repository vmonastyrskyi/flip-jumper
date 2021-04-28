using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Loader;
using LocalSave;
using LocalSave.Dao;
using Menu.Settings;
using PlayGames;
using PlayGames.Dao;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Util;
using Button = UnityEngine.UI.Button;

namespace Menu.Controllers
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private GameObject platformWithPlayer;

        [Header("Buttons")]
        [SerializeField] private Button closeButton;

        [Header("Panels")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject loaderPanel;
        [SerializeField] private GameObject noInternetPanel;

        [Header("Audio")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundsSlider;

        [Header("Graphics")]
        [SerializeField] private Toggle antiAliasingToggle;
        [SerializeField] private Toggle shadowsToggle;

        [Header("Language")]
        [SerializeField] private Button englishLocaleButton;
        [SerializeField] private Button russianLocaleButton;

        [Header("Signed Panel")]
        [SerializeField] private GameObject signedPanel;
        [SerializeField] private TextMeshProUGUI userName;
        [SerializeField] private RawImage avatar;

        [Header("Unsigned Panel")]
        [SerializeField] private GameObject unsignedPanel;
        [SerializeField] private Button signInButton;

        [Header("Sync Panel")]
        [SerializeField] private GameObject syncPanel;
        [SerializeField] private Button syncButton;
        [SerializeField] private Button syncLocalButton;
        [SerializeField] private Button syncCloudButton;

        private Dictionary<string, GameObject> _accountPanels;

        private enum AntiAliasing
        {
            Disabled = 0,
            Enabled = 8
        }

        private enum Locale
        {
            English = 0,
            Russian = 1
        }

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(CloseSettings);
            if (musicSlider != null)
                musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            if (soundsSlider != null)
                soundsSlider.onValueChanged.AddListener(ChangeSoundsVolume);
            if (antiAliasingToggle != null)
                antiAliasingToggle.onValueChanged.AddListener(ToggleAntialiasing);
            if (shadowsToggle != null)
                shadowsToggle.onValueChanged.AddListener(ToggleShadows);
            if (shadowsToggle != null)
                shadowsToggle.onValueChanged.AddListener(ToggleShadows);
            if (englishLocaleButton != null)
                englishLocaleButton.onClick.AddListener(() => ChangeLocale(Locale.English));
            if (russianLocaleButton != null)
                russianLocaleButton.onClick.AddListener(() => ChangeLocale(Locale.Russian));
            if (signInButton != null)
                signInButton.onClick.AddListener(Authenticate);
            if (syncButton != null)
                syncButton.onClick.AddListener(Synchronize);
            if (syncLocalButton != null)
                syncLocalButton.onClick.AddListener(SyncLocalWithCloud);
            if (syncCloudButton != null)
                syncCloudButton.onClick.AddListener(SyncCloudWithLocal);

            _accountPanels = new Dictionary<string, GameObject>
            {
                {"loader", loaderPanel},
                {"signed", signedPanel},
                {"unsigned", unsignedPanel},
                {"sync", syncPanel},
                {"noInternet", noInternetPanel}
            };

            ChangeAccountState(PlayGamesServices.IsAuthenticated);
        }

        private IEnumerator Start()
        {
            yield return null;

            var localData = LocalSaveSystem.LoadLocalData();

            musicSlider.value = localData.Settings.MusicVolume;
            soundsSlider.value = localData.Settings.SoundsVolume;
            antiAliasingToggle.isOn = localData.Settings.Antialiasing;
            shadowsToggle.isOn = localData.Settings.Shadows;

            ChangeMusicVolume(localData.Settings.MusicVolume);
            ChangeSoundsVolume(localData.Settings.SoundsVolume);
            QualitySettings.antiAliasing =
                localData.Settings.Antialiasing ? (int) AntiAliasing.Enabled : (int) AntiAliasing.Disabled;
            QualitySettings.shadows =
                localData.Settings.Shadows ? ShadowQuality.HardOnly : ShadowQuality.Disable;

            switch (LocalizationSettings.SelectedLocale.Identifier.Code)
            {
                case "en":
                    englishLocaleButton.GetComponent<Util.Button>().SelectOnStart();
                    break;
                case "ru":
                    russianLocaleButton.GetComponent<Util.Button>().SelectOnStart();
                    break;
            }
        }

        private void Authenticate()
        {
            ActivateAccountPanels(new[] {"loader"});

            PlayGamesServices.Authenticate(ChangeAccountState);
        }

        private void ChangeAccountState(bool authenticated)
        {
            var localData = LocalSaveSystem.LoadLocalData();

            if (InternetConnection.Available())
            {
                if (authenticated)
                {
                    StartCoroutine(LoadUser());
                    PlayGamesServicesEventSystem.Instance.UserAuthenticated(true);
                }

                if (authenticated && localData.Synchronized)
                    ActivateAccountPanels(new[] {"signed"});
                else if (authenticated && !localData.Synchronized)
                    ActivateAccountPanels(new[] {"sync"});
                else if (!authenticated)
                    ActivateAccountPanels(new[] {"unsigned"});
            }
            else
                ActivateAccountPanels(new[] {"noInternet"});
        }

        private void ActivateAccountPanels(string[] panels)
        {
            foreach (var pair in _accountPanels)
                pair.Value.SetActive(Array.Exists(panels, p => p == pair.Key));
        }

        private IEnumerator LoadUser()
        {
            ActivateAccountPanels(new[] {"loader"});

            PlayGamesServices.LoadLocalUser();

            while (PlayGamesServices.LocalUser.image == null)
                yield return null;

            userName.SetText(PlayGamesServices.LocalUser.userName);
            avatar.texture = PlayGamesServices.LocalUser.image;
        }

        public void SaveMusicVolumeChanges()
        {
            var localData = LocalSaveSystem.LoadLocalData();
            localData.Settings.MusicVolume = musicSlider.value;
            LocalSaveSystem.SaveLocalData(localData);
        }

        public void SaveSoundsVolumeChanges()
        {
            var localData = LocalSaveSystem.LoadLocalData();
            localData.Settings.SoundsVolume = soundsSlider.value;
            LocalSaveSystem.SaveLocalData(localData);
        }

        private void CloseSettings()
        {
            var localData = LocalSaveSystem.LoadLocalData();

            platformWithPlayer.SetActive(true);
            settingsPanel.SetActive(false);
            menuPanel.SetActive(true);

            if (InternetConnection.Available())
            {
                if (PlayGamesServices.IsAuthenticated && localData.Synchronized)
                    ActivateAccountPanels(new[] {"signed"});
                else if (PlayGamesServices.IsAuthenticated && !localData.Synchronized)
                    ActivateAccountPanels(new[] {"sync"});
                else if (!PlayGamesServices.IsAuthenticated)
                    ActivateAccountPanels(new[] {"unsigned"});
            }
            else
            {
                ActivateAccountPanels(new[] {"noInternet"});
            }
        }

        private void ChangeMusicVolume(float value)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        }

        private void ChangeSoundsVolume(float value)
        {
            audioMixer.SetFloat("SoundsVolume", Mathf.Log10(value) * 20);
        }

        private void ToggleAntialiasing(bool value)
        {
            var localData = LocalSaveSystem.LoadLocalData();
            localData.Settings.Antialiasing = value;
            QualitySettings.antiAliasing = value ? (int) AntiAliasing.Enabled : (int) AntiAliasing.Disabled;
            LocalSaveSystem.SaveLocalData(localData);
        }

        private void ToggleShadows(bool value)
        {
            var localData = LocalSaveSystem.LoadLocalData();
            localData.Settings.Shadows = value;
            QualitySettings.shadows = value ? ShadowQuality.HardOnly : ShadowQuality.Disable;
            LocalSaveSystem.SaveLocalData(localData);
        }

        private void ChangeLocale(Locale locale)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int) locale];
        }

        private void Synchronize()
        {
            ActivateAccountPanels(new[] {"sync"});
        }

        private void SyncLocalWithCloud()
        {
            var localData = LocalSaveSystem.LoadLocalData();
            localData.Synchronized = true;
            LocalSaveSystem.SaveLocalData(localData);

            var defaultCloudData = CloudData.FromLocalData(localData);
            PlayGamesServices.SaveCloudData(defaultCloudData);

            PlayGamesServices.ReportScore(Gps.LeaderboardHighScore, localData.HighScore);

            ActivateAccountPanels(new[] {"signed"});
        }

        private void SyncCloudWithLocal()
        {
            ActivateAccountPanels(new[] {"loader"});

            PlayGamesServices.LoadCloudData(cloudData =>
            {
                var localData = LocalSaveSystem.LoadLocalData();

                if (localData.SaveTime != cloudData.saveTime)
                {
                    localData.Synchronized = true;
                    localData.Characters = cloudData.characters;
                    localData.Platforms = cloudData.platforms;
                    localData.Environments = cloudData.environments;
                    localData.SaveTime = cloudData.saveTime;
                    localData.HighScore = cloudData.highScore;
                    localData.Coins = cloudData.coins;

                    UpdateGameData(localData);
                }

                ActivateAccountPanels(new[] {"signed"});
            });
        }

        private static void UpdateGameData(LocalData data)
        {
            LocalSaveSystem.SaveLocalData(data);

            var charactersData = data.Characters;
            var platformsData = data.Platforms;
            var gameData = DataManager.Instance.GameData;

            foreach (var characterData in charactersData)
            {
                var character = gameData.Characters.FirstOrDefault(c => c.Id == characterData.Id);
                if (character != null)
                {
                    character.IsPurchased = characterData.IsPurchased;
                    character.IsSelected = characterData.IsSelected;
                    character.IsEffectEnabled = characterData.IsEffectEnabled;
                }
            }

            foreach (var platformData in platformsData)
            {
                var platform = gameData.Platforms.FirstOrDefault(p => p.Id == platformData.Id);
                if (platform != null)
                {
                    platform.IsPurchased = platformData.IsPurchased;
                    platform.IsActive = platformData.IsActive;
                }
            }

            gameData.HighScore = data.HighScore;
            gameData.Coins = data.Coins;

            SettingsEventSystem.Instance.GameDataUpdated();
        }
    }
}