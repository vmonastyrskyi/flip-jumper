using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Player
{
    public class DeathEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem lavaDeathEffectParticle;
        [SerializeField] private ParticleSystem waterDeathEffectParticle;

        private PlayerState _playerState;

        private void Start()
        {
            PlayerEventSystem.instance.OnStateChanged += state => _playerState = state;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_playerState != PlayerState.Dead) return;

            ParticleSystem deathEffect;
            var position = transform.position;
            switch (other.tag)
            {
                case "Lava":
                    transform.DOMoveY(-4, 4);
                    deathEffect = Instantiate(lavaDeathEffectParticle,
                        new Vector3(position.x, other.transform.position.y + .1f, position.z),
                        Quaternion.identity);
                    deathEffect.Play();
                    break;
                case "Water":
                    deathEffect = Instantiate(waterDeathEffectParticle,
                        new Vector3(position.x, other.transform.position.y + .35f, position.z),
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