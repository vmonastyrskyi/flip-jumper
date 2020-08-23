using UnityEngine;

namespace Game
{
    public class PlatformManager : MonoBehaviour
    {
        public string Guid { get; private set; }
        public bool IsPlayerSteppedOnce { get; private set; }
        
        public SpawnDirection Direction { get; set; }

        private void Start()
        {
            Guid = System.Guid.NewGuid().ToString();

            PlatformEventSystem.current.OnPlayerStepped += (guid, isStepped) =>
            {
                if (Guid == guid && !IsPlayerSteppedOnce)
                {
                    IsPlayerSteppedOnce = isStepped;
                    GameEventSystem.current.CreatePlatform();
                    GameEventSystem.current.IncreaseScore(1);
                }
            };
        }
    }
}