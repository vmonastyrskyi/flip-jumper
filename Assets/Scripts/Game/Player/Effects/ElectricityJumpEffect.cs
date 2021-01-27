using System.Collections;
using Game.Controllers;
using Game.EventSystems;
using UnityEngine;

namespace Game.Player.Effects
{
    public class ElectricityJumpEffect : Effect
    {
        [SerializeField] private ParticleSystem electricityEffectParticle;

        private IEnumerator Start()
        {
            yield return null;

            PlayerEventSystem.instance.OnStateChanged += PlayEffect;
        }

        private void PlayEffect(PlayerState state)
        {
            if (state == PlayerState.Jumping)
            {
                var position = transform.position;
                var electricityJumpEffect = Instantiate(electricityEffectParticle,
                    new Vector3(position.x, position.y - .95f, position.z),
                    Quaternion.Euler(-90, 0, 0));
                electricityJumpEffect.Play();
            }
        }
    }
}