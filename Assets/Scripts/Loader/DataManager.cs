using UnityEngine;

namespace Loader
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager instance;

        public GameObject[] characters;
        public int selectedCharacterIndex;

        public GameObject SelectedCharacter => characters[selectedCharacterIndex];

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
    }
}