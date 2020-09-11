using System.Collections;
using Loader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MapSelection : MonoBehaviour
    {
        private SceneSystem _sceneSystem;
        private Map _previousMap;
        private Map _selectedMap;

        private void Start()
        {
            _sceneSystem = SceneSystem.instance;
            _selectedMap = _sceneSystem.map;
        }

        public void SelectNextMap()
        {
            _previousMap = _selectedMap;
            StartCoroutine(LoadMap(_sceneSystem.map = ++_selectedMap));
        }

        public void SelectPreviousMap()
        {
            _previousMap = _selectedMap;
            StartCoroutine(LoadMap(_sceneSystem.map = --_selectedMap));
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
                    SceneManager.UnloadSceneAsync((int) _previousMap);
                    load.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}