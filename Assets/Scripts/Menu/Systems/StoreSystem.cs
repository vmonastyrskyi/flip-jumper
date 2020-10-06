using Menu.Store.EventSystems;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Systems
{
    public class StoreSystem : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject coinsPanel;
        [SerializeField] private GameObject gemsPanel;

        private UserData _userData;
        private TextMeshProUGUI _coinsLabel;
        private TextMeshProUGUI _gemsLabel;

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(CloseShop);

            _coinsLabel = coinsPanel.GetComponentInChildren<TextMeshProUGUI>();
            _gemsLabel = gemsPanel.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            _userData = DataManager.instance.UserData;
            
            SetCashInformation();

            CharactersPageEventSystem.instance.OnSuccessfulCharacterBuy += SetCashInformation;
        }
        
        private void SetCashInformation()
        {
            _coinsLabel.SetText(_userData.Coins.ToString());
            _gemsLabel.SetText(_userData.Gems.ToString());
        }

        private void CloseShop()
        {
            gameObject.SetActive(false);
        }
    }
}