using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Menu.Controllers
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject settingsPanel;
        
        [SerializeField] private Button closeButton;
        
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundsSlider;
        
        [SerializeField] private Toggle antiAliasingToggle;
        [SerializeField] private Toggle shadowsToggle;
        
        [SerializeField] private AudioMixer audioMixer;

        private enum AntiAliasing
        {
            Disabled = 0,
            Enabled = 8
        }

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(ClosePanel);
            if (musicSlider != null)
                musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            if (soundsSlider != null)
                soundsSlider.onValueChanged.AddListener(ChangeSoundsVolume);
            if (antiAliasingToggle != null)
                antiAliasingToggle.onValueChanged.AddListener(ToggleAntialiasing);
            if (shadowsToggle != null)
                shadowsToggle.onValueChanged.AddListener(ToggleShadows);
        }

        private void ClosePanel()
        {
            settingsPanel.SetActive(false);
            menuPanel.SetActive(true);
        }

        private void ChangeMusicVolume(float value)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 0, value));
        }

        private void ChangeSoundsVolume(float value)
        {
            audioMixer.SetFloat("SoundsVolume", Mathf.Lerp(-80, 0, value));
        }

        private void ToggleAntialiasing(bool value)
        {
            QualitySettings.antiAliasing = value ? (int) AntiAliasing.Enabled : (int) AntiAliasing.Disabled;
        }

        private void ToggleShadows(bool value)
        {
            QualitySettings.shadows = value ? ShadowQuality.HardOnly : ShadowQuality.Disable;
        }
    }
}