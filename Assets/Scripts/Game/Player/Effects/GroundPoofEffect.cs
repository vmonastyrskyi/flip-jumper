using System.Collections;
using Game.EventSystems;
using UnityEngine;

namespace Game.Player.Effects
{
    public class GroundPoofEffect : Effect
    {
        [SerializeField] private GameObject groundPoofEffectPrefab = null;

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
                var magicPoofEffect = Instantiate(groundPoofEffectPrefab,
                    new Vector3(position.x, position.y - .95f, position.z),
                    Quaternion.Euler(-90, 0, 0));
                magicPoofEffect.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}