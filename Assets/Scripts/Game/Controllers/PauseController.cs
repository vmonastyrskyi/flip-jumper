using System.Collections;
using Game.EventSystems;
using Loader;
using LocalSave;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private Animator sceneTransitionAnimator;
        [Header("Panels")]
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject pausePanel;

        [Header("Audio")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundsSlider;

        [Header("Buttons")]
        [SerializeField] private Button homeButton;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button resumeButton;

        private static readonly int FadeIn = Animator.StringToHash("Fade_In");

        private void Awake()
        {
            if (musicSlider != null)
                musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            if (soundsSlider != null)
                soundsSlider.onValueChanged.AddListener(ChangeSoundsVolume);
            if (homeButton != null)
                homeButton.onClick.AddListener(() => StartCoroutine(LoadMenu()));
            if (playAgainButton != null)
                playAgainButton.onClick.AddListener(() => StartCoroutine(ReloadGame()));
            if (resumeButton != null)
                resumeButton.onClick.AddListener(ResumeGame);
        }

        private IEnumerator Start()
        {
            yield return null;

            var data = LocalSaveSystem.LoadLocalData();

            musicSlider.value = data.Settings.MusicVolume;
            soundsSlider.value = data.Settings.SoundsVolume;

            ChangeMusicVolume(data.Settings.MusicVolume);
            ChangeSoundsVolume(data.Settings.SoundsVolume);
        }

        public void SaveMusicVolumeChanges()
        {
            var data = LocalSaveSystem.LoadLocalData();
            data.Settings.MusicVolume = musicSlider.value;
            LocalSaveSystem.SaveLocalData(data);
        }

        public void SaveSoundsVolumeChanges()
        {
            var data = LocalSaveSystem.LoadLocalData();
            data.Settings.SoundsVolume = soundsSlider.value;
            LocalSaveSystem.SaveLocalData(data);
        }

        private IEnumerator LoadMenu()
        {
            sceneTransitionAnimator.SetTrigger(FadeIn);

            yield return new WaitForSecondsRealtime(0.25f);

            SceneManager.Instance.LoadMenu();
        }

        private IEnumerator ReloadGame()
        {
            sceneTransitionAnimator.SetTrigger(FadeIn);

            yield return new WaitForSecondsRealtime(0.25f);

            SceneManager.Instance.LoadGame();
        }

        private void ResumeGame()
        {
            Time.timeScale = 1;
            gamePanel.SetActive(true);
            pausePanel.SetActive(false);
            GameEventSystem.Instance.ResumeGame();
        }

        private void ChangeMusicVolume(float value)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        }

        private void ChangeSoundsVolume(float value)
        {
            audioMixer.SetFloat("SoundsVolume", Mathf.Log10(value) * 20);
        }
    }
}