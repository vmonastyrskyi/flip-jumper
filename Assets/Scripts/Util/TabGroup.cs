using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class TabGroup : MonoBehaviour
    {
        private readonly List<Tab> _tabs = new List<Tab>();

        public void Subscribe(Tab tab)
        {
            _tabs.Add(tab);
        }

        public void OnTabSelected(Tab tab)
        {
            ResetTabs();

            tab.Select();
        }

        private void ResetTabs()
        {
            foreach (var tab in _tabs)
                tab.Deselect();
        }
    }
}