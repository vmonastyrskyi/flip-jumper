using System.Collections;
using Save;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Menu.Controllers
{
    public class SettingsController : MonoBehaviour
    {
        private const string EnglishLocale = "English (en)";
        private const string RussianLocale = "Russian (ru)";

        [SerializeField] private GameObject platformWithPlayer;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Button closeButton;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundsSlider;
        [SerializeField] private Toggle antiAliasingToggle;
        [SerializeField] private Toggle shadowsToggle;
        [SerializeField] private Button englishLocaleButton;
        [SerializeField] private Button russianLocaleButton;
        [SerializeField] private AudioMixer audioMixer;

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
        }

        private IEnumerator Start()
        {
            yield return null;

            var data = SaveSystem.Load();

            musicSlider.value = data.Settings.MusicVolume;
            soundsSlider.value = data.Settings.SoundsVolume;
            antiAliasingToggle.isOn = data.Settings.Antialiasing;
            shadowsToggle.isOn = data.Settings.Shadows;

            ChangeMusicVolume(data.Settings.MusicVolume);
            ChangeSoundsVolume(data.Settings.SoundsVolume);
            QualitySettings.antiAliasing =
                data.Settings.Antialiasing ? (int) AntiAliasing.Enabled : (int) AntiAliasing.Disabled;
            QualitySettings.shadows =
                data.Settings.Shadows ? ShadowQuality.HardOnly : ShadowQuality.Disable;

            Debug.Log(russianLocaleButton);
            Debug.Log(russianLocaleButton.GetComponent<Button>());
            Debug.Log(russianLocaleButton.GetComponent<Util.Button>());
            
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

        public void SaveMusicVolumeChanges()
        {
            var data = SaveSystem.Load();
            data.Settings.MusicVolume = musicSlider.value;
            SaveSystem.Save(data);
        }

        public void SaveSoundsVolumeChanges()
        {
            var data = SaveSystem.Load();
            data.Settings.SoundsVolume = soundsSlider.value;
            SaveSystem.Save(data);
        }

        private void CloseSettings()
        {
            platformWithPlayer.SetActive(true);
            settingsPanel.SetActive(false);
            menuPanel.SetActive(true);
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
            var data = SaveSystem.Load();

            data.Settings.Antialiasing = value;
            QualitySettings.antiAliasing = value ? (int) AntiAliasing.Enabled : (int) AntiAliasing.Disabled;

            SaveSystem.Save(data);
        }

        private void ToggleShadows(bool value)
        {
            var data = SaveSystem.Load();

            data.Settings.Shadows = value;
            QualitySettings.shadows = value ? ShadowQuality.HardOnly : ShadowQuality.Disable;

            SaveSystem.Save(data);
        }

        private void ChangeLocale(Locale locale)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int) locale];
        }
    }
}