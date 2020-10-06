using System.Collections;
using Loader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MapSelection : MonoBehaviour
    {
        private Camera _mainCamera;
        private SceneSystem _sceneSystem;
        private Map _previousMap;
        private Map _selectedMap;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private IEnumerator LoadMap(Map map)
        {
            var load = SceneManager.LoadSceneAsync((int) map, LoadSceneMode.Additive);
            load.completed += operation => SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int) map));
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

            SceneManager.UnloadSceneAsync((int) _previousMap);
        }
    }
}