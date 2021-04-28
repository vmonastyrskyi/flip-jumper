using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Scene = Util.Scene;

namespace Loader
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;

        [SerializeField] private Material skySkybox;

        private Environment Environment { get; set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Environment = Environment.Sky;
        }

        public void LoadGame()
        {
            LoadScene(Scene.Game);
        }

        public void LoadMenu()
        {
            LoadScene(Scene.Menu);
        }

        private void LoadScene(Scene scene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int) scene);
            StartCoroutine(LoadEnvironment());
        }

        private IEnumerator LoadEnvironment()
        {
            var environmentLoading =
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int) Environment, LoadSceneMode.Additive);
            environmentLoading.allowSceneActivation = false;
            environmentLoading.completed += operation =>
            {
                UnityEngine.SceneManagement.SceneManager.MergeScenes(
                    UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex((int) Environment),
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene()
                );

                RenderSettings.skybox = skySkybox;
                RenderSettings.sun = GameObject.FindWithTag("Directional Light").GetComponent<Light>();
            };


            while (!environmentLoading.isDone)
            {
                if (environmentLoading.progress >= 0.9f)
                    environmentLoading.allowSceneActivation = true;

                yield return null;
            }
        }
    }
}