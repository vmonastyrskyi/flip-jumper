using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Save
{
    public static class SaveSystem
    {
        private static readonly string Path = Application.persistentDataPath + "/data.dat";
        private static readonly BinaryFormatter Formatter;

        static SaveSystem()
        {
            Formatter = new BinaryFormatter();
        }

        public static void Initialize()
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
                        new CharacterData(1000, true, true, true),
                        new CharacterData(1001, false, false, true),
                        new CharacterData(1002, false, false, true),
                        new CharacterData(1003, false, false, true),
                        new CharacterData(1004, false, false, true)
                    };

                    var platforms = new[]
                    {
                        new PlatformData(2000, true, true),
                        new PlatformData(2001, false, false),
                        new PlatformData(2002, false, false),
                        new PlatformData(2003, false, false),
                        new PlatformData(2004, false, false),
                    };

                    var data = new ApplicationData(
                        settings,
                        characters,
                        platforms,
                        null,
                        0,
                        0
                    );

                    Formatter.Serialize(stream, data);
                }
                finally
                {
                    stream?.Close();
                }
            }
        }

        public static void Save(ApplicationData applicationData)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(Path, FileMode.Create);
                var data = new ApplicationData(applicationData);
                Formatter.Serialize(stream, data);
            }
            finally
            {
                stream?.Close();
            }
        }

        public static ApplicationData Load()
        {
            if (File.Exists(Path))
            {
                FileStream stream = null;
                try
                {
                    stream = new FileStream(Path, FileMode.Open);
                    return Formatter.Deserialize(stream) as ApplicationData;
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