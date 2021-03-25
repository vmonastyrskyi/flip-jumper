using System.Collections;
using Game.EventSystems;
using UnityEngine;

namespace Game.Player.Effects
{
    public class MagicPoofEffect : Effect
    {
        [SerializeField] private GameObject magicPoofEffectPrefab;

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
                var magicPoofEffect = Instantiate(magicPoofEffectPrefab,
                    new Vector3(position.x, position.y - .75f, position.z),
                    Quaternion.Euler(90, 0, 0));
                magicPoofEffect.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}