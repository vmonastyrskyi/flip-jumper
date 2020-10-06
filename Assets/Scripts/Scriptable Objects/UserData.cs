using System.Linq;
using UnityEngine;

namespace Scriptable_Objects
{
    public class UserData : ScriptableObject
    {
        [SerializeField] private CharacterData[] characters;
        [SerializeField] private PlatformData[] platforms;
        [SerializeField] private MapData[] maps;
        [SerializeField] private int coins;
        [SerializeField] private int gems;

        public CharacterData[] Characters => characters;

        public CharacterData SelectedCharacter => characters.First(character => character.IsSelected);

        public PlatformData[] Platforms => platforms;

        public PlatformData SelectedPlatform => platforms.First(platform => platform.IsSelected);

        public MapData[] Maps => maps;

        public MapData SelectedMap => maps.First(map => map.IsSelected);

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