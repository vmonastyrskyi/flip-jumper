using System;
using UnityEngine;

namespace Util
{
    public class DestroyAfter : MonoBehaviour
    {
        [SerializeField] private float timeToDestroy;

        private void Start()
        {
            Destroy(gameObject, timeToDestroy);
        }
    }
}