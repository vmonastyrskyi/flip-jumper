using Loader;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject pausePanel;

        [SerializeField] private Button homeButton;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button resumeButton;

        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundsSlider;

        [SerializeField] private AudioMixer audioMixer;

        private void Awake()
        {
            if (homeButton != null)
                homeButton.onClick.AddListener(LoadMenu);
            if (playAgainButton != null)
                playAgainButton.onClick.AddListener(ReloadGame);
            if (resumeButton != null)
                resumeButton.onClick.AddListener(ResumeGame);

            if (musicSlider != null)
                musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            if (soundsSlider != null)
                soundsSlider.onValueChanged.AddListener(ChangeSoundsVolume);
        }

        private static void LoadMenu()
        {
            SceneSystem.instance.LoadMenu();
        }

        private static void ReloadGame()
        {
            SceneSystem.instance.LoadGame();
        }

        private void ResumeGame()
        {
            gamePanel.SetActive(true);
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }

        private void ChangeMusicVolume(float value)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 0, value));
        }

        private void ChangeSoundsVolume(float value)
        {
            audioMixer.SetFloat("SoundsVolume", Mathf.Lerp(-80, 0, value));
        }
    }
}