using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LocalSave.Dao;
using UnityEngine;

namespace LocalSave
{
    public static class LocalSaveSystem
    {
        private static readonly string Path = Application.persistentDataPath + "/local_data.dat";
        private static readonly BinaryFormatter Formatter;

        static LocalSaveSystem()
        {
            Formatter = new BinaryFormatter();
        }

        public static void InitializeDefaultData()
        {
            if (!File.Exists(Path))
            {
                FileStream stream = null;
                try
                {
                    stream = new FileStream(Path, FileMode.Create);

                    var settings = new SettingsData(
                        0.5f,
                        0.5f,
                        true,
                        true
                    );

                    var characters = new[]
                    {
                        new CharacterData("Office Man", true, true, true),
                        new CharacterData("Builder", false, false, true),
                        new CharacterData("Santa", false, false, true),
                        new CharacterData("Zeus", false, false, true),
                        new CharacterData("Mage", false, false, true)
                    };

                    var platforms = new[]
                    {
                        new PlatformData("Ground", true, true),
                        new PlatformData("Box", false, false),
                        new PlatformData("Floppy Disk", false, false),
                        new PlatformData("Cookie", false, false),
                        new PlatformData("Rubiks Cube", false, false),
                    };

                    var data = new LocalData
                    {
                        Settings = settings,
                        Characters = characters,
                        Platforms = platforms,
                        Environments = new EnvironmentData[] { },
                        SaveTime = DateTime.Now.Ticks,
                        Synchronized = false,
                        HighScore = 0,
                        Coins = 0
                    };

                    Formatter.Serialize(stream, data);
                }
                finally
                {
                    stream?.Close();
                }
            }
        }

        public static void SaveLocalData(LocalData data)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(Path, FileMode.Create);
                Formatter.Serialize(stream, data);
            }
            finally
            {
                stream?.Close();
            }
        }

        public static LocalData LoadLocalData()
        {
            if (File.Exists(Path))
            {
                FileStream stream = null;
                try
                {
                    stream = new FileStream(Path, FileMode.Open);
                    return Formatter.Deserialize(stream) as LocalData;
                }
                finally
                {
                    stream?.Close();
                }
            }

            return null;
        }
    }
}