using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using LocalSave;
using LocalSave.Dao;
using PlayGames;
using PlayGames.Dao;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;
using Environment = Util.Environment;

namespace Loader
{
    public class LoaderSystem : MonoBehaviour
    {
        [SerializeField] private Animator sceneTransitionAnimator;

        [Header("Skyboxes")]
        [SerializeField] private Material skySkybox;

        [Header("Progress Bar")]
        [SerializeField] private Slider progressBar;
        [SerializeField] private TextMeshProUGUI progressLabel;

        private static readonly int FadeIn = Animator.StringToHash("Fade_In");

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            StartCoroutine(LoadEnvironment());
        }

        private IEnumerator Start()
        {
            progressLabel.SetText("Initializing Local Data...");
            var start = Time.realtimeSinceStartup;
            yield return InitializeLocalData();
            var finish = Time.realtimeSinceStartup;
            progressBar.DOKill();
            progressBar.DOValue(0.25f, finish - start);

            var authenticated = false;
            CloudData loadedCloudData = null;

            if (InternetConnection.Available())
            {
                progressLabel.SetText("Authenticating...");
                start = Time.realtimeSinceStartup;
                yield return Authenticate(success => authenticated = success);
                finish = Time.realtimeSinceStartup;
                progressBar.DOKill();
                progressBar.DOValue(0.5f, finish - start);

                if (authenticated)
                {
                    progressLabel.SetText("Loading Cloud Data..");
                    start = Time.realtimeSinceStartup;
                    yield return LoadCloudData(data => loadedCloudData = data);
                    finish = Time.realtimeSinceStartup;
                    progressBar.DOKill();
                    progressBar.DOValue(0.75f, finish - start);

                    progressLabel.SetText("Synchronizing...");
                    start = Time.realtimeSinceStartup;
                    yield return Synchronize(loadedCloudData);
                    finish = Time.realtimeSinceStartup;
                    progressBar.DOKill();
                    progressBar.DOValue(0.95f, finish - start);
                }
                else
                {
                    progressLabel.SetText("Loading Local Data...");
                    start = Time.realtimeSinceStartup;
                    yield return LoadLocalData();
                    finish = Time.realtimeSinceStartup;
                    progressBar.DOKill();
                    progressBar.DOValue(0.95f, finish - start);
                }
            }
            else
            {
                progressLabel.SetText("Loading Local Data...");
                start = Time.realtimeSinceStartup;
                yield return LoadLocalData();
                finish = Time.realtimeSinceStartup;
                progressBar.DOKill();
                progressBar.DOValue(0.95f, finish - start);
            }

            progressLabel.SetText("Initialize Localization...");
            start = Time.realtimeSinceStartup;
            yield return InitializeLocalization();
            finish = Time.realtimeSinceStartup;
            progressBar.DOKill();
            progressBar.DOValue(1, finish - start);

            progressLabel.SetText("Loading Menu...");
            yield return LoadMenu();
        }

        private static IEnumerator InitializeLocalData()
        {
            yield return null;

            LocalSaveSystem.InitializeDefaultData();
        }

        private static IEnumerator Authenticate(Action<bool> callback)
        {
            var authenticated = false;
            PlayGamesServices.Authenticate(success =>
            {
                authenticated = true;
                callback.Invoke(success);
            });

            yield return new WaitUntil(() => authenticated);
        }

        private static IEnumerator LoadCloudData(Action<CloudData> callback)
        {
            var loadedCloudData = false;
            PlayGamesServices.LoadCloudData(data =>
            {
                loadedCloudData = true;
                callback.Invoke(data);
            });

            yield return new WaitUntil(() => loadedCloudData);
        }

        private static IEnumerator Synchronize(CloudData cloudData)
        {
            bool synchronized;

            var localData = LocalSaveSystem.LoadLocalData();

            if (!localData.Synchronized)
            {
                localData.Synchronized = true;

                if (cloudData == null)
                {
                    var defaultCloudData = CloudData.FromLocalData(localData);
                    PlayGamesServices.SaveCloudData(defaultCloudData);
                }
                else
                {
                    localData.Characters = cloudData.characters;
                    localData.Platforms = cloudData.platforms;
                    localData.Environments = cloudData.environments;
                    localData.SaveTime = cloudData.saveTime;
                    localData.HighScore = cloudData.highScore;
                    localData.Coins = cloudData.coins;
                }

                synchronized = true;
            }
            else
            {
                if (localData.SaveTime > cloudData.saveTime)
                {
                    var newCloudData = CloudData.FromLocalData(localData);
                    PlayGamesServices.SaveCloudData(newCloudData);

                    PlayGamesServices.ReportScore(Gps.LeaderboardHighScore, localData.HighScore);
                }
                else if (localData.SaveTime < cloudData.saveTime)
                {
                    localData.Characters = cloudData.characters;
                    localData.Platforms = cloudData.platforms;
                    localData.Environments = cloudData.environments;
                    localData.SaveTime = cloudData.saveTime;
                    localData.HighScore = cloudData.highScore;
                    localData.Coins = cloudData.coins;
                }
                
                synchronized = true;
            }

            LoadGameData(localData);

            yield return new WaitUntil(() => synchronized);
        }

        private static IEnumerator LoadLocalData()
        {
            yield return null;

            LoadGameData(LocalSaveSystem.LoadLocalData());
        }

        private static void LoadGameData(LocalData data)
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
        }

        private IEnumerator LoadEnvironment()
        {
            var environmentLoading =
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int) Environment.Sky,
                    LoadSceneMode.Additive);
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

        private IEnumerator InitializeLocalization()
        {
            while (!LocalizationSettings.InitializationOperation.IsDone)
                yield return LocalizationSettings.InitializationOperation;
        }

        private IEnumerator LoadMenu()
        {
            sceneTransitionAnimator.SetTrigger(FadeIn);

            yield return new WaitForSeconds(0.25f);

            SceneManager.Instance.LoadMenu();
        }
    }
}