using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using Menu.Store.EventSystems;

namespace Ads
{
    public class FreeCoinsForAdListener : INonSkippableVideoAdListener
    {
        public void onNonSkippableVideoLoaded(bool isPrecache)
        {
        }

        public void onNonSkippableVideoFailedToLoad()
        {
        }

        public void onNonSkippableVideoShowFailed()
        {
        }

        public void onNonSkippableVideoShown()
        {
        }

        public void onNonSkippableVideoFinished()
        {
            CurrencyPageEventSystem.Instance.RewardForVideo();
        }

        public void onNonSkippableVideoClosed(bool finished)
        {
        }

        public void onNonSkippableVideoExpired()
        {
        }
    }
}