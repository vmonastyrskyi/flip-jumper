using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Save
{
    public static class SaveSystem
    {
        private static readonly string Path = Application.persistentDataPath + "/data.fun";
        private static readonly BinaryFormatter Formatter;

        static SaveSystem()
        {
            Formatter = new BinaryFormatter();
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
                    var data = Formatter.Deserialize(stream) as ApplicationData;
                    Debug.Log(data.SelectedMap);
                    Debug.Log(data.SelectedCharacter);
                    Debug.Log(data.Coins);
                    Debug.Log(data.Score);
                    return data;
                }
                finally
                {
                    stream?.Close();
                }
            }

            Debug.Log("No Data");
            return null;
        }
    }
}