using Loader;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private Button homeButton;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button resumeButton;

        private void Awake()
        {
            if (homeButton != null)
                homeButton.onClick.AddListener(LoadMenu);
            if (playAgainButton != null)
                playAgainButton.onClick.AddListener(ReloadGame);
            if (resumeButton != null)
                resumeButton.onClick.AddListener(ResumeGame);
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
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}