using System.Collections;
using Game.EventSystems;
using UnityEngine;

namespace Game.Player.Effects
{
    public class SmokePoofEffect : Effect
    {
        [SerializeField] private GameObject smokePoofEffectPrefab = null;

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
                var smokePoofEffect = Instantiate(smokePoofEffectPrefab,
                    new Vector3(position.x, position.y - 1, position.z),
                    Quaternion.Euler(0, 0, 0));
                smokePoofEffect.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}