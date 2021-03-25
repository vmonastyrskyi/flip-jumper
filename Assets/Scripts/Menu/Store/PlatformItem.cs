using Menu.Store.EventSystems;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Store
{
    public class PlatformItem : MonoBehaviour
    {
        [SerializeField] private Platform platform;
        [SerializeField] private GameObject lockIcon;
        [SerializeField] private GameObject selectedIcon;

        private void Awake()
        {
            lockIcon.SetActive(!platform.IsPurchased);
            selectedIcon.SetActive(platform.IsActive);
                
            GetComponent<Button>().onClick.AddListener(SelectItem);

            PlatformsPageEventSystem.Instance.OnPlatformPurchased += purchasedPlatform =>
            {
                if (platform == purchasedPlatform)
                    lockIcon.SetActive(false);
            };

            PlatformsPageEventSystem.Instance.OnPlatformActivated += activatedPlatform =>
            {
                if (platform == activatedPlatform)
                    selectedIcon.SetActive(platform.IsActive);
            };
        }

        private void SelectItem()
        {
            PlatformsPageEventSystem.Instance.SelectPlatform(platform);
        }
    }
}