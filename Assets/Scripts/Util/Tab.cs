using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Util
{
    [RequireComponent(typeof(SVGImage))]
    public class Tab : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TabGroup tabGroup;
        [SerializeField] private GameObject page;
        [SerializeField] private Sprite selectedTabSprite;
        [SerializeField] private Sprite deselectedTabSprite;
        [SerializeField] private bool selectOnStart;

        private SVGImage _image;

        private void Awake()
        {
            _image = GetComponent<SVGImage>();
        }

        private void Start()
        {
            if (selectOnStart)
                Select();

            tabGroup.Subscribe(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void Select()
        {
            _image.sprite = selectedTabSprite;
            page.SetActive(true);
        }

        public void Deselect()
        {
            _image.sprite = deselectedTabSprite;
            page.SetActive(false);
        }
    }
}