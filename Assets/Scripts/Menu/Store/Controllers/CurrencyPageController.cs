using Ads;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Store.Controllers
{
    public class CurrencyPageController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button freeCoinsButton;

        private void Awake()
        {
            if (freeCoinsButton != null)
                freeCoinsButton.onClick.AddListener(() =>
                    AdsManager.ShowNonSkippableVideo(AdsManager.Placement.FreeCoins)
                );
        }
    }
}