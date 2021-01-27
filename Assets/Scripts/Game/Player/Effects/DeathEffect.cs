using DG.Tweening;
using UnityEngine;

namespace Game.Player.Effects
{
    public class DeathEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem lavaDeathEffectParticle;
        [SerializeField] private ParticleSystem waterDeathEffectParticle;

        private void OnTriggerEnter(Collider other)
        {
            ParticleSystem deathEffect;
            var position = transform.position;
            switch (other.tag)
            {
                case "Lava":
                    transform.DOMoveY(-4, 4);
                    deathEffect = Instantiate(lavaDeathEffectParticle,
                        new Vector3(position.x, other.transform.position.y, position.z),
                        Quaternion.identity);
                    deathEffect.Play();
                    break;
                case "Water":
                    deathEffect = Instantiate(waterDeathEffectParticle,
                        new Vector3(position.x, other.transform.position.y + 0.01f, position.z),
                        Quaternion.Euler(90, 0, 0));
                    deathEffect.Play();
                    break;
            }
        }

        private void OnDisable()
        {
            transform.DOKill();
        }
    }
}