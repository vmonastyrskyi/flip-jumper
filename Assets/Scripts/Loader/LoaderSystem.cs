using System.Linq;
using Save;
using UnityEngine;

namespace Loader
{
    public class LoaderSystem : MonoBehaviour
    {
        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            SaveSystem.Initialize();

            LoadData();
        }

        private static void LoadData()
        {
            var applicationData = SaveSystem.Load();
            var charactersData = applicationData.Characters;
            var platformsData = applicationData.Platforms;

            var gameData = DataManager.Instance.GameData;
            
            foreach (var characterData in charactersData)
            {
                var character = gameData.Characters.First(c => c.Id == characterData.Id);
                character.IsPurchased = characterData.IsPurchased;
                character.IsSelected = characterData.IsSelected;
                character.IsEffectEnabled = characterData.IsEffectEnabled;
            }

            foreach (var platformData in platformsData)
            {
                var platform = gameData.Platforms.First(p => p.Id == platformData.Id);
                platform.IsPurchased = platformData.IsPurchased;
                platform.IsActive = platformData.IsActive;
            }

            gameData.HighScore = applicationData.HighScore;
            gameData.Coins = applicationData.Coins;
        }
    }
}