using System;
using UnityEngine;

namespace Game
{
    public class GameEventSystem : MonoBehaviour
    {
        public static GameEventSystem current;

        private void Awake()
        {
            current = this;
        }

        public event Action<SpawnDirection> OnSpawnDirectionChanged;

        public void ChangeSpawnDirection(SpawnDirection spawnDirection)
        {
            OnSpawnDirectionChanged?.Invoke(spawnDirection);
        }

        public event Action OnCreatePlatform;

        public void CreatePlatform()
        {
            OnCreatePlatform?.Invoke();
        }
        
        public event Action<int> OnIncreaseScore;

        public void IncreaseScore(int points)
        {
            OnIncreaseScore?.Invoke(points);
        }
    }
}