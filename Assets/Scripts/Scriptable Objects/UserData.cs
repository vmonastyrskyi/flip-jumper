using System.Linq;
using UnityEngine;

namespace Scriptable_Objects
{
    public class UserData : ScriptableObject
    {
        [SerializeField] private Character[] characters;
        [SerializeField] private Platform[] platforms;
        [SerializeField] private Map[] maps;
        [SerializeField] private int coins;
        [SerializeField] private int gems;

        public Character[] Characters => characters;

        public Character SelectedCharacter => characters.First(character => character.IsSelected);

        public Platform[] Platforms => platforms;

        public Platform SelectedPlatform => platforms.First(platform => platform.IsSelected);

        public Map[] Maps => maps;

        public Map SelectedMap => maps.First(map => map.IsSelected);

        public int Coins
        {
            get => coins;
            set => coins = value;
        }

        public int Gems
        {
            get => gems;
            set => gems = value;
        }
    }
}