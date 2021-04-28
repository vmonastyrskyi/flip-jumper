using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class ButtonGroup : MonoBehaviour
    {
        private readonly List<Button> _buttons = new List<Button>();

        public void Subscribe(Button button)
        {
            _buttons.Add(button);
        }

        public void OnButtonSelected(Button button)
        {
            ResetButtons();

            button.Select();
        }

        private void ResetButtons()
        {
            foreach (var button in _buttons)
                button.Deselect();
        }
    }
}