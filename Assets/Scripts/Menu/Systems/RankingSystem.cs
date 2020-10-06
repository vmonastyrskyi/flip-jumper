using UnityEngine;
using UnityEngine.UI;

namespace Menu.Systems
{
    public class RankingSystem : MonoBehaviour
    {
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(ClosePanel);
        }

        private void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}
