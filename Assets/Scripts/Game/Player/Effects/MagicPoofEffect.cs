using System.Collections;
using Game.Controllers;
using Game.EventSystems;
using UnityEngine;

namespace Game.Player.Effects
{
    public class MagicPoofEffect : Effect
    {
        [SerializeField] private ParticleSystem magicPoofEffectParticle;

        private IEnumerator Start()
        {
            yield return null;

            PlayerEventSystem.instance.OnStateChanged += PlayEffect;
        }

        private void PlayEffect(PlayerState state)
        {
            if (state == PlayerState.Stepped)
            {
                var position = transform.position;
                var magicPoofEffect = Instantiate(magicPoofEffectParticle,
                    new Vector3(position.x, position.y - .75f, position.z),
                    Quaternion.Euler(90, 0, 0));
                magicPoofEffect.Play();
            }
        }
    }
}