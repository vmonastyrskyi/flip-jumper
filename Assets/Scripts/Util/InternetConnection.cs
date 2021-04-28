using UnityEngine;

namespace Util
{
    public class InternetConnection : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public static bool Available()
        {
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    return false;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    return true;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    return true;
                default:
                    return false;
            }
        }
    }
}