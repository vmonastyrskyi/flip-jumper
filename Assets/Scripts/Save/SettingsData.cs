using System;

namespace Save
{
    [Serializable]
    public class SettingsData
    {
        private float _musicVolume;
        private float _soundsVolume;
        private bool _antialiasing;
        private bool _shadows;

        public float MusicVolume
        {
            get => _musicVolume;
            set => _musicVolume = value;
        }

        public float SoundsVolume
        {
            get => _soundsVolume;
            set => _soundsVolume = value;
        }

        public bool Antialiasing
        {
            get => _antialiasing;
            set => _antialiasing = value;
        }

        public bool Shadows
        {
            get => _shadows;
            set => _shadows = value;
        }

        public SettingsData(SettingsData settingsData)
        {
            _musicVolume = settingsData._musicVolume;
            _soundsVolume = settingsData._soundsVolume;
            _antialiasing = settingsData._antialiasing;
            _shadows = settingsData._shadows;
        }

        public SettingsData(
            float musicVolume,
            float soundsVolume,
            bool antialiasing,
            bool shadows
        )
        {
            _musicVolume = musicVolume;
            _soundsVolume = soundsVolume;
            _antialiasing = antialiasing;
            _shadows = shadows;
        }
    }
}