using System;
using Save;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Loader
{
    public class LoaderController : MonoBehaviour
    {
        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 999;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            var dataManager = DataManager.instance;
            var sceneManager = SceneSystem.instance;

            var data = SaveSystem.Load();

            if (data != null)
            {
                dataManager.selectedCharacterIndex = data.SelectedCharacter;
                sceneManager.map = data.SelectedMap;
            }
            else
            {
                const int defaultCharacterIndex = 0;
                const Map defaultMap = Map.Sky;

                dataManager.selectedCharacterIndex = defaultCharacterIndex;
                sceneManager.map = defaultMap;

                SaveSystem.Save(new ApplicationData(
                    defaultMap,
                    defaultCharacterIndex,
                    0,
                    0
                ));
            }

            sceneManager.LoadMenu();
        }
    }
}