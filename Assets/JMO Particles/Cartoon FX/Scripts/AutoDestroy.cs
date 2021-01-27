using System.Collections;
using UnityEngine;

namespace Game.Player.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDestroy : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine("CheckIfAlive");
        }

        IEnumerator CheckIfAlive()
        {
            var ps = GetComponent<ParticleSystem>();

            while (ps != null)
            {
                yield return new WaitForSeconds(0.5f);
                if (!ps.IsAlive(true))
                {
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}