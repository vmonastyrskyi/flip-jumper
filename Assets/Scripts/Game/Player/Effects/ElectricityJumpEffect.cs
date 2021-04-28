using System.Collections;
using Game.EventSystems;
using UnityEngine;

namespace Game.Player.Effects
{
    public class ElectricityJumpEffect : Effect
    {
        [SerializeField] private GameObject electricityJumpEffectPrefab = null;

        private IEnumerator Start()
        {
            yield return null;

            if (PlatformEventSystem.Instance != null)
                PlatformEventSystem.Instance.OnPlayerStepped += PlayEffect;
        }

        private void PlayEffect(string guid, bool centered)
        {
            if (!centered)
            {
                var position = transform.position;
                var electricityJumpEffect = Instantiate(electricityJumpEffectPrefab,
                    new Vector3(position.x, position.y - .95f, position.z),
                    Quaternion.Euler(-90, 0, 0));
                electricityJumpEffect.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}