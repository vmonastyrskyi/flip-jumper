using System.Collections;
using Game.EventSystems;
using Loader;
using Save;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game.Controllers
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private Animator sceneTransitionAnimator;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundsSlider;
        [SerializeField] private AudioMixer audioMixer;
        
        private static readonly int FadeIn = Animator.StringToHash("Fade_In");

        private void Awake()
        {
            if (homeButton != null)
                homeButton.onClick.AddListener(() => StartCoroutine(LoadMenu()));
            if (playAgainButton != null)
                playAgainButton.onClick.AddListener(() => StartCoroutine(ReloadGame()));
            if (resumeButton != null)
                resumeButton.onClick.AddListener(ResumeGame);

            if (musicSlider != null)
                musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            if (soundsSlider != null)
                soundsSlider.onValueChanged.AddListener(ChangeSoundsVolume);
        }

        private IEnumerator Start()
        {
            yield return null;

            var data = SaveSystem.Load();

            musicSlider.value = data.Settings.MusicVolume;
            soundsSlider.value = data.Settings.SoundsVolume;

            ChangeMusicVolume(data.Settings.MusicVolume);
            ChangeSoundsVolume(data.Settings.SoundsVolume);
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