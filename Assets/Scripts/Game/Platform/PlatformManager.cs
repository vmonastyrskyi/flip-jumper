using UnityEngine;

namespace Game.Platform
{
    public class PlatformManager : MonoBehaviour
    {
        private bool _isPlayerSteppedOnce;
        
        public SpawnDirection Direction { get; set; }

        public string Guid { get; private set; }

        private void Start()
        {
            Guid = System.Guid.NewGuid().ToString();

            PlatformEventSystem.instance.OnPlayerStepped += (guid, isStepped) =>
            {
                if (Guid == guid && !_isPlayerSteppedOnce)
                {
                    GameEventSystem.instance.CreatePlatform();
                    GameEventSystem.instance.IncreaseScore(1);
                    _isPlayerSteppedOnce = isStepped;
                }
            };
        }
    }
}