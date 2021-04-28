using AppodealAds.Unity.Common;
using Game.EventSystems;

namespace Ads
{
    public class MultiplyCoinsForAdListener : INonSkippableVideoAdListener
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
            GameEventSystem.Instance.RewardForVideo();
        }

        public void onNonSkippableVideoClosed(bool finished)
        {
        }

        public void onNonSkippableVideoExpired()
        {
        }
    }
}