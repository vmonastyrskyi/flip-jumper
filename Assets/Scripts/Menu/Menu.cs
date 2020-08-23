using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        public GameObject platformWithPlayer;

        private void Update()
        {
            platformWithPlayer.transform.Rotate(0, -(10f * Time.deltaTime), 0);
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}