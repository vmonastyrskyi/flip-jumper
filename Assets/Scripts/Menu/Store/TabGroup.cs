using System.Collections.Generic;
using UnityEngine;

namespace Menu.Store
{
    public class TabGroup : MonoBehaviour
    {
        private readonly List<TabButton> _tabButtons = new List<TabButton>();

        public void Subscribe(TabButton button)
        {
            _tabButtons.Add(button);
        }

        public void OnTabSelected(TabButton button)
        {
            ResetTabs();
            button.Select();
        }

        private void ResetTabs()
        {
            foreach (var button in _tabButtons)
                button.Deselect();
        }
    }
}