using System;
using Loader;

namespace Save
{
    [Serializable]
    public class ApplicationData
    {
        private Map _selectedMap;
        private int _selectedCharacter;
        private int _score;
        private int _coins;

        public ApplicationData(ApplicationData applicationData)
        {
            _selectedMap = applicationData._selectedMap;
            _selectedCharacter = applicationData._selectedCharacter;
            _score = applicationData._score;
            _coins = applicationData._coins;
        }

        public ApplicationData(Map selectedMap, int selectedCharacter, int score, int coins)
        {
            _selectedMap = selectedMap;
            _selectedCharacter = selectedCharacter;
            _score = score;
            _coins = coins;
        }

        public Map SelectedMap
        {
            get => _selectedMap;
            set => _selectedMap = value;
        }

        public int SelectedCharacter
        {
            get => _selectedCharacter;
            set => _selectedCharacter = value;
        }

        public int Score
        {
            get => _score;
            set => _score = value;
        }

        public int Coins
        {
            get => _coins;
            set => _coins = value;
        }
    }
}