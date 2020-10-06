using UnityEngine;
using UnityEngine.SceneManagement;

public class AttachToCamera : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var mainCamera = Camera.main;
        
        if (mainCamera != null)
            transform.parent = mainCamera.transform;
    }
}