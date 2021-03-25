using System;

namespace Save
{
    [Serializable]
    public class ApplicationData
    {
        private SettingsData _settingsData;
        private CharacterData[] _characters;
        private PlatformData[] _platforms;
        private EnvironmentData[] _environments;
        private int _highScore;
        private int _coins;

        public SettingsData Settings
        {
            get => _settingsData;
            set => _settingsData = value;
        }

        public CharacterData[] Characters
        {
            get => _characters;
            set => _characters = value;
        }

        public PlatformData[] Platforms
        {
            get => _platforms;
            set => _platforms = value;
        }

        public EnvironmentData[] Environments
        {
            get => _environments;
            set => _environments = value;
        }

        public int HighScore
        {
            get => _highScore;
            set => _highScore = value;
        }

        public int Coins
        {
            get => _coins;
            set => _coins = value;
        }

        public ApplicationData(ApplicationData applicationData)
        {
            _settingsData = applicationData._settingsData;
            _characters = applicationData._characters;
            _platforms = applicationData._platforms;
            _environments = applicationData._environments;
            _highScore = applicationData._highScore;
            _coins = applicationData._coins;
        }

        public ApplicationData(
            SettingsData settingsData,
            CharacterData[] characters,
            PlatformData[] platforms,
            EnvironmentData[] environments,
            int highScore,
            int coins
        )
        {
            _settingsData = settingsData;
            _characters = characters;
            _platforms = platforms;
            _environments = environments;
            _highScore = highScore;
            _coins = coins;
        }
    }
}