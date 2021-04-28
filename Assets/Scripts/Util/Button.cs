using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Util
{
    [RequireComponent(typeof(SVGImage))]
    public class Button : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ButtonGroup buttonGroup;
        [SerializeField] private Sprite selectedButtonSprite;
        [SerializeField] private Sprite deselectedButtonSprite;

        private SVGImage _image;
        private bool _selectOnStart;

        private void Awake()
        {
            _image = GetComponent<SVGImage>();
        }

        private void Start()
        {
            if (_selectOnStart)
                Select();
            
            buttonGroup.Subscribe(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            buttonGroup.OnButtonSelected(this);
        }

        public void Select()
        {
            _image.sprite = selectedButtonSprite;
        }

        public void Deselect()
        {
            _image.sprite = deselectedButtonSprite;
        }

        public void SelectOnStart()
        {
            _selectOnStart = true;
        }
    }
}