using System;

namespace LocalSave.Dao
{
    [Serializable]
    public class LocalData
    {
        private SettingsData _settingsData;
        private CharacterData[] _characters;
        private PlatformData[] _platforms;
        private EnvironmentData[] _environments;
        private long _saveTime;
        private bool _synchronized;
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

        public long SaveTime
        {
            get => _saveTime;
            set => _saveTime = value;
        }

        public bool Synchronized
        {
            get => _synchronized;
            set => _synchronized = value;
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
    }
}