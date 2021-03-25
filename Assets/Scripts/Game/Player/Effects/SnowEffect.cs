using System;
using UnityEngine;

namespace Game.Player.Effects
{
    public class SnowEffect : Effect
    {
        [SerializeField] private GameObject snowEffectPrefab;

        private GameObject _snowEffect;
        
        private void OnEnable()
        {
            PlayEffect();
        }

        private void PlayEffect()
        {
            var playerTransform = transform;
            var position = playerTransform.position;
            _snowEffect = Instantiate(snowEffectPrefab,
                new Vector3(position.x + 6, position.y + 12, position.z),
                Quaternion.identity,
                playerTransform);
            _snowEffect.GetComponent<ParticleSystem>().Play();
        }
        
        private void OnDisable()
        {
            Destroy(_snowEffect);
        }
    }
}