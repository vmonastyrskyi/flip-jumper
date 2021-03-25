using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using SceneManager = Loader.SceneManager;

namespace Menu
{
    public class MapSelection : MonoBehaviour
    {
        private Camera _mainCamera;
        private SceneManager _sceneManager;
        private Environment _previousEnvironment;
        private Environment _selectedEnvironment;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private IEnumerator LoadMap(Environment environment)
        {
            var load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync((int) environment, LoadSceneMode.Additive);
            load.completed += operation =>
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(
                    UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex((int) environment));
            load.allowSceneActivation = false;
            while (!load.isDone)
            {
                if (load.progress >= 0.9f)
                {
                    var childCount = _mainCamera.transform.childCount;
                    for (var i = childCount - 1; i >= 0; i--)
                        Destroy(_mainCamera.transform.GetChild(i).gameObject);
                    load.allowSceneActivation = true;
                }

                yield return null;
            }

            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync((int) _previousEnvironment);
        }
    }
}