using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu.Store
{
    [RequireComponent(typeof(SVGImage))]
    public class TabButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TabGroup tabGroup;
        [SerializeField] private GameObject page;
        [SerializeField] private Sprite selectedButtonSprite;
        [SerializeField] private Sprite deselectedButtonSprite;
        [SerializeField] private bool selectOnStart;

        private SVGImage _image;

        private void Start()
        {
            _image = GetComponent<SVGImage>();

            if (selectOnStart)
            {
                Select();
            }

            tabGroup.Subscribe(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void Select()
        {
            _image.sprite = selectedButtonSprite;
            page.SetActive(true);
        }

        public void Deselect()
        {
            _image.sprite = deselectedButtonSprite;
            page.SetActive(false);
        }
    }
}