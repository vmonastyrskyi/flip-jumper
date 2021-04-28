using System.Linq;
using UnityEngine;

namespace Scriptable_Objects
{
    public class GameData : ScriptableObject
    {
        [SerializeField] private Character[] characters;
        [SerializeField] private Platform[] platforms;
        [SerializeField] private int highScore;
        [SerializeField] private int coins;

        public Character[] Characters => characters;

        public Character DefaultCharacter => characters.First(character => character.IsDefault);

        public Character SelectedCharacter =>
            characters.First(character => character.IsPurchased && character.IsSelected);

        public Platform[] Platforms => platforms;

        public Platform DefaultPlatform => platforms.First(platform => platform.IsDefault);

        public int HighScore
        {
            get => highScore;
            set => highScore = value;
        }

        public int Coins
        {
            get => coins;
            set => coins = value;
        }
    }
}