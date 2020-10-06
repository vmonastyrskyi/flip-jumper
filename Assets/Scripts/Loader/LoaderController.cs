using Scriptable_Objects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Loader
{
    public class LoaderController : MonoBehaviour
    {
        private UserData _userData;
        
        private void Start()
        {
            _userData = DataManager.instance.UserData;
            
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 999;
            
            var sceneManager = SceneSystem.instance;
            sceneManager.map = (Map) _userData.SelectedMap.SceneIndex;
            sceneManager.LoadMenu();
        }

        // private void OnEnable()
        // {
        //     SceneManager.sceneLoaded += OnSceneLoaded;
        // }
        //
        // private void OnDisable()
        // {
        //     SceneManager.sceneLoaded -= OnSceneLoaded;
        // }

        // private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        // {
        //     var sceneManager = SceneSystem.instance;
        //
        //     sceneManager.map = (Map) _userData.SelectedMap.SceneIndex;
        //
        //     // var data = SaveSystem.Load();
        //     //
        //     // if (data != null)
        //     // {
        //     //     sceneManager.map = data.SelectedMap;
        //     // }
        //     // else
        //     // {
        //     //     const int defaultCharacterIndex = 0;
        //     //     const Map defaultMap = Map.Sky;
        //     //
        //     //     sceneManager.map = defaultMap;
        //     //
        //     //     SaveSystem.Save(new ApplicationData(
        //     //         defaultMap,
        //     //         defaultCharacterIndex,
        //     //         0,
        //     //         0
        //     //     ));
        //     // }
        //
        //     sceneManager.LoadMenu();
        // }
    }
}