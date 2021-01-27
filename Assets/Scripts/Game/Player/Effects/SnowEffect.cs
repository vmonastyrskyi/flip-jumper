using UnityEngine;

namespace Game.Player.Effects
{
    public class SnowEffect : Effect
    {
        [SerializeField] private ParticleSystem snowEffectParticle;

        private void OnEnable()
        {
            PlayEffect();
        }

        private void PlayEffect()
        {
            var playerTransform = transform;
            var position = playerTransform.position;
            var snowEffect = Instantiate(snowEffectParticle,
                new Vector3(position.x + 6, position.y + 12, position.z),
                Quaternion.identity,
                playerTransform);
            snowEffect.Play();
        }
    }
}