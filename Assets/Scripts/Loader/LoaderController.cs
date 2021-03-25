using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;
using Environment = Util.Environment;

namespace Loader
{
    public class LoaderController : MonoBehaviour
    {
        private const float LoadingTime = 0;

        [SerializeField] private Material skySkybox;
        [SerializeField] private Animator sceneTransitionAnimator;
        [SerializeField] private Slider progressBar;
        private static readonly int FadeIn = Animator.StringToHash("Fade_In");

        private void Awake()
        {
            StartCoroutine(LoadEnvironment());
            StartCoroutine(LoadMenu());
        }

        private IEnumerator LoadEnvironment()
        {
            var environmentLoading =
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int) Environment.Sky, LoadSceneMode.Additive);
            environmentLoading.allowSceneActivation = false;
            environmentLoading.completed += operation =>
            {
                UnityEngine.SceneManagement.SceneManager.MergeScenes(
                    UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex((int) Environment.Sky),
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

        private IEnumerator LoadMenu()
        {
            while (!LocalizationSettings.InitializationOperation.IsDone)
                yield return LocalizationSettings.InitializationOperation;

            yield return new WaitForSeconds(LoadingTime);

            sceneTransitionAnimator.SetTrigger(FadeIn);

            yield return new WaitForSeconds(0.25f);

            SceneManager.Instance.LoadMenu();
        }

        private void Update()
        {
            progressBar.value += Time.deltaTime / LoadingTime;
        }
    }
}