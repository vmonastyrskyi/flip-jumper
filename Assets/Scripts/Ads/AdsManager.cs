using System;
using System.Collections;
using AppodealAds.Unity.Api;
using UnityEngine;

namespace Ads
{
    public class AdsManager : MonoBehaviour
    {
        public enum Placement
        {
            MultiplyCoins,
            FreeCoins
        }
        
        private const string AppKey = "553717c7800254047627ccddd8ddac196121afbfa36bc7f6";

        public static AdsManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private IEnumerator Start()
        {
            yield return null;

            Appodeal.setTesting(true);
            Appodeal.muteVideosIfCallsMuted(true);
            Appodeal.initialize(AppKey, Appodeal.NON_SKIPPABLE_VIDEO | Appodeal.BANNER);
        }

        public static void ShowNonSkippableVideo(Placement placement)
        {
            if (Appodeal.isLoaded(Appodeal.NON_SKIPPABLE_VIDEO))
            {
                Appodeal.show(Appodeal.NON_SKIPPABLE_VIDEO);
            }

            switch (placement)
            {
                case Placement.MultiplyCoins:
                    Appodeal.setNonSkippableVideoCallbacks(new MultiplyCoinsForAdListener());
                    break;
                case Placement.FreeCoins:
                    Appodeal.setNonSkippableVideoCallbacks(new FreeCoinsForAdListener());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(placement), placement, null);
            }
        }

        public static void ShowBanner()
        {
            if (Appodeal.isLoaded(Appodeal.BANNER))
            {
                Appodeal.show(Appodeal.BANNER_BOTTOM);
            }
        }
    }
}