using System.Collections;
using Game.EventSystems;
using Game.Platform;
using Game.Systems;
using UnityEngine;

namespace Game.Controllers
{
    public class PlatformController : MonoBehaviour
    {
        public JumpDirection Direction { get; set; }
        public string Guid { get; private set; }
        public bool Visited { private get; set; }
        public bool IsMoving
        {
            set => GetComponent<Moving>().enabled = value;
        }
        public Vector3 InitialPosition { get; private set; }

        private void Awake()
        {
            Guid = System.Guid.NewGuid().ToString();
            InitialPosition = transform.position;
        }

        private IEnumerator Start()
        {
            yield return null;

            PlatformEventSystem.Instance.OnPlayerStepped += (guid, centered) =>
            {
                if (Guid == guid && !Visited)
                {
                    PlatformEventSystem.Instance.Visited(centered);
                    Visited = true;
                }
            };
        }
    }
}