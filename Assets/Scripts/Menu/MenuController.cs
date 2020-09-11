using System;
using Loader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject menuOrPlayPanel;
        [SerializeField] private GameObject shopPanel;

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
            switch (UiManager.instance.uiPage)
            {
                case UiPage.Home:
                    menuPanel.SetActive(true);
                    menuOrPlayPanel.SetActive(false);
                    shopPanel.SetActive(false);
                    break;
                case UiPage.HomeOrPlay:
                    menuPanel.SetActive(false);
                    menuOrPlayPanel.SetActive(true);
                    shopPanel.SetActive(false);
                    break;
                case UiPage.Shop:
                    menuPanel.SetActive(false);
                    menuOrPlayPanel.SetActive(false);
                    shopPanel.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Play()
        {
            SceneSystem.instance.LoadGame();
        }

        public void ReturnToHome()
        {
            UiManager.instance.uiPage = UiPage.Home;
            
            menuPanel.SetActive(true);
            menuOrPlayPanel.SetActive(false);
            shopPanel.SetActive(false);
        }

        public void OpenShop()
        {
            menuPanel.SetActive(false);
            menuOrPlayPanel.SetActive(false);
            shopPanel.SetActive(true);
        }
    }
}