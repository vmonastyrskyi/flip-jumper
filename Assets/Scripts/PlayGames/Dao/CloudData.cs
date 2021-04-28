using System;
using LocalSave.Dao;
using UnityEngine;

namespace PlayGames.Dao
{
    [Serializable]
    public class CloudData
    {
        public CharacterData[] characters;
        public PlatformData[] platforms;
        public EnvironmentData[] environments;
        public long saveTime;
        public int highScore;
        public int coins;

        public static CloudData FromLocalData(LocalData data)
        {
            return new CloudData
            {
                characters = data.Characters,
                platforms = data.Platforms,
                environments = data.Environments,
                saveTime = data.SaveTime,
                highScore = data.HighScore,
                coins = data.Coins
            };
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}