using UnityEngine;

namespace Game.Player.Effects
{
    public class DeathEffect : MonoBehaviour
    {
        [SerializeField] private GameObject lavaDeathEffectPrefab;
        [SerializeField] private GameObject waterDeathEffectPrefab;

        private void OnTriggerEnter(Collider other)
        {
            var position = transform.position;
            var rb = GetComponent<Rigidbody>();
            GameObject deathEffect;
            switch (other.tag)
            {
                case "Lava":
                    rb.velocity = new Vector3(0, -2, 0);
                    rb.useGravity = false;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;

                    deathEffect = Instantiate(lavaDeathEffectPrefab,
                        new Vector3(position.x, other.transform.position.y + 0.01f, position.z),
                        Quaternion.Euler(-90, 0, 0));
                    deathEffect.GetComponent<ParticleSystem>().Play();
                    break;
                case "Water":
                    rb.velocity = new Vector3(0, -4, 0);
                    
                    deathEffect = Instantiate(waterDeathEffectPrefab,
                        new Vector3(position.x, other.transform.position.y + 0.01f, position.z),
                        Quaternion.Euler(90, 0, 0));
                    deathEffect.GetComponent<ParticleSystem>().Play();
                    break;
            }
        }
    }
}