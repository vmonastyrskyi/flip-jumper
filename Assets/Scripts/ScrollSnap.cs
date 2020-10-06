using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler,
    IPointerUpHandler
{
    public MovementType movementType = MovementType.Fixed;
    public MovementAxis movementAxis = MovementAxis.Horizontal;
    public bool automaticallyLayout = true;
    public SizeControl sizeControl = SizeControl.Fit;
    public Vector2 size = new Vector2(400, 250);
    public float automaticLayoutSpacing = 0.25f;
    public float leftMargin;
    public float rightMargin;
    public float topMargin;
    public float bottomMargin;
    public bool infinitelyScroll = false;
    public float infiniteScrollingEndSpacing = 0f;
    public bool useOcclusionCulling = false;
    public int startingPanel = 0;
    public bool swipeGestures = true;
    public float minimumSwipeSpeed = 0f;
    public Button previousButton = null;
    public Button nextButton = null;
    public SnapTarget snapTarget = SnapTarget.Next;
    public float snappingSpeed = 10f;
    public float thresholdSnappingSpeed = -1f;
    public bool inertia = false;
    public bool useUnscaledTime = false;
    public UnityEvent onPanelChanged;
    public UnityEvent onPanelSelecting;
    public UnityEvent onPanelSelected;
    public UnityEvent onPanelChanging;
    public List<TransitionEffect> transitionEffects = new List<TransitionEffect>();

    private bool _dragging;
    private bool _pressing;
    private bool _selected = true;
    private float _releaseSpeed;
    private float _contentLength;
    private Direction _releaseDirection;
    private Graphic[] _graphics;
    private Canvas _canvas;
    private ScrollRect _scrollRect;
    private Vector2 _previousContentAnchoredPosition;
    private Vector2 _velocity;

    public RectTransform Content => _scrollRect.content;
    public RectTransform Viewport => _scrollRect.viewport;
    public int CurrentPanel { get; set; }
    public int TargetPanel { get; set; }
    public int NearestPanel { get; set; }
    private RectTransform[] PanelsRt { get; set; }
    public GameObject[] Panels { get; set; }
    public Toggle[] Toggles { get; set; }
    public int NumberOfPanels => Content.childCount;

    public enum MovementType
    {
        Fixed,
        Free
    }

    public enum MovementAxis
    {
        Horizontal,
        Vertical
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum SnapTarget
    {
        Nearest,
        Previous,
        Next
    }

    public enum SizeControl
    {
        Manual,
        Fit
    }

    private void Awake()
    {
        Initialize();
    }

    private IEnumerator Start()
    {
        yield return null;

        if (Validate())
            Setup();
        else
            throw new Exception("Invalid configuration.");
    }

    private void Update()
    {
        if (NumberOfPanels == 0) return;

        StartCoroutine(Processing());
    }

    private IEnumerator Processing()
    {
        yield return null;

        OnOcclusionCulling();
        OnSelectingAndSnapping();
        OnInfiniteScrolling();
        OnTransitionEffects();
        OnSwipeGestures();

        DetermineVelocity();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Initialize();
    }
#endif

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressing = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inertia)
            _scrollRect.inertia = true;

        _selected = false;
        _dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragging)
            onPanelSelecting.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragging = false;

        switch (movementAxis)
        {
            case MovementAxis.Horizontal:
                _releaseDirection = _scrollRect.velocity.x > 0 ? Direction.Right : Direction.Left;
                break;
            case MovementAxis.Vertical:
                _releaseDirection = _scrollRect.velocity.y > 0 ? Direction.Up : Direction.Down;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _releaseSpeed = _scrollRect.velocity.magnitude;
    }

    private void Initialize()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private bool Validate()
    {
        var valid = true;

        if (snappingSpeed < 0)
        {
            Debug.LogError("<b>[SimpleScrollSnap]</b> Snapping speed cannot be negative.", gameObject);
            valid = false;
        }

        return valid;
    }

    private void Setup()
    {
        if (NumberOfPanels == 0) return;

        // ScrollRect
        if (movementType == MovementType.Fixed)
        {
            _scrollRect.horizontal = (movementAxis == MovementAxis.Horizontal);
            _scrollRect.vertical = (movementAxis == MovementAxis.Vertical);
        }
        else
        {
            _scrollRect.horizontal = _scrollRect.vertical = true;
        }

        // Panels
        size = (sizeControl == SizeControl.Manual)
            ? size
            : new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height);

        Panels = new GameObject[NumberOfPanels];
        PanelsRt = new RectTransform[NumberOfPanels];

        for (var i = 0; i < NumberOfPanels; i++)
        {
            Panels[i] = Content.GetChild(i).gameObject;
            PanelsRt[i] = Panels[i].GetComponent<RectTransform>();

            if (movementType == MovementType.Fixed && automaticallyLayout)
            {
                PanelsRt[i].anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f,
                    movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                PanelsRt[i].anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f,
                    movementAxis == MovementAxis.Vertical ? 0f : 0.5f);

                var x = (rightMargin + leftMargin) / 2f - leftMargin;
                var y = (topMargin + bottomMargin) / 2f - bottomMargin;
                var marginOffset = new Vector2(x / size.x, y / size.y);
                PanelsRt[i].pivot = new Vector2(0.5f, 0.5f) + marginOffset;
                PanelsRt[i].sizeDelta = size - new Vector2(leftMargin + rightMargin, topMargin + bottomMargin);

                var panelPosX = (movementAxis == MovementAxis.Horizontal)
                    ? i * (automaticLayoutSpacing + 1f) * size.x + (size.x / 2f)
                    : 0f;
                var panelPosY = (movementAxis == MovementAxis.Vertical)
                    ? i * (automaticLayoutSpacing + 1f) * size.y + (size.y / 2f)
                    : 0f;
                PanelsRt[i].anchoredPosition = new Vector2(panelPosX, panelPosY);
            }
        }

        // Content
        if (movementType == MovementType.Fixed)
        {
            // Automatic Layout
            if (automaticallyLayout)
            {
                Content.anchorMin = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f,
                    movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                Content.anchorMax = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f,
                    movementAxis == MovementAxis.Vertical ? 0f : 0.5f);
                Content.pivot = new Vector2(movementAxis == MovementAxis.Horizontal ? 0f : 0.5f,
                    movementAxis == MovementAxis.Vertical ? 0f : 0.5f);

                var contentWidth = (movementAxis == MovementAxis.Horizontal)
                    ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.x) - (size.x * automaticLayoutSpacing)
                    : size.x;
                var contentHeight = (movementAxis == MovementAxis.Vertical)
                    ? (NumberOfPanels * (automaticLayoutSpacing + 1f) * size.y) - (size.y * automaticLayoutSpacing)
                    : size.y;
                Content.sizeDelta = new Vector2(contentWidth, contentHeight);
            }

            // Infinite Scrolling
            if (infinitelyScroll)
            {
                var contentRect = Content.rect;
                _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
                _contentLength = (movementAxis == MovementAxis.Horizontal)
                    ? (contentRect.width + size.x * infiniteScrollingEndSpacing)
                    : contentRect.height + size.y * infiniteScrollingEndSpacing;

                OnInfiniteScrolling(true);
            }

            // Occlusion Culling
            if (useOcclusionCulling)
            {
                OnOcclusionCulling(true);
            }
        }
        else
        {
            automaticallyLayout = infinitelyScroll = useOcclusionCulling = false;
        }

        // Starting Panel
        var viewportRect = Viewport.rect;

        var xOffset = (movementAxis == MovementAxis.Horizontal || movementType == MovementType.Free)
            ? viewportRect.width / 2f
            : 0f;
        var yOffset = (movementAxis == MovementAxis.Vertical || movementType == MovementType.Free)
            ? viewportRect.height / 2f
            : 0f;

        var offset = new Vector2(xOffset, yOffset);

        _previousContentAnchoredPosition =
            Content.anchoredPosition = -PanelsRt[startingPanel].anchoredPosition + offset;
        CurrentPanel = TargetPanel = NearestPanel = startingPanel;

        // Previous Button
        if (previousButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(GoToPreviousPanel);
        }

        // Next Button
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(GoToNextPanel);
        }
    }

    private Vector2 DisplacementFromCenter(int index)
    {
        var viewportRect = Viewport.rect;
        var contentAnchorMin = Content.anchorMin;

        return PanelsRt[index].anchoredPosition + Content.anchoredPosition - new Vector2(
            viewportRect.width * (0.5f - contentAnchorMin.x),
            viewportRect.height * (0.5f - contentAnchorMin.y));
    }

    private int DetermineNearestPanel()
    {
        var distances = new float[NumberOfPanels];

        for (var i = 0; i < Panels.Length; i++)
        {
            distances[i] = DisplacementFromCenter(i).magnitude;
        }

        return distances.ToList().IndexOf(distances.Min());
    }

    private void DetermineVelocity()
    {
        var contentAnchoredPosition = Content.anchoredPosition;
        var displacement = contentAnchoredPosition - _previousContentAnchoredPosition;
        var time = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        _velocity = displacement / time;
        _previousContentAnchoredPosition = contentAnchoredPosition;
    }

    private void SelectTargetPanel()
    {
        var displacementFromCenter = DisplacementFromCenter(NearestPanel = DetermineNearestPanel());

        if (snapTarget == SnapTarget.Nearest || _releaseSpeed <= minimumSwipeSpeed)
        {
            GoToPanel(NearestPanel);
        }
        else if (snapTarget == SnapTarget.Previous)
        {
            if ((_releaseDirection == Direction.Right && displacementFromCenter.x < 0f) ||
                (_releaseDirection == Direction.Up && displacementFromCenter.y < 0f))
            {
                GoToNextPanel();
            }
            else if ((_releaseDirection == Direction.Left && displacementFromCenter.x > 0f) ||
                     (_releaseDirection == Direction.Down && displacementFromCenter.y > 0f))
            {
                GoToPreviousPanel();
            }
            else
            {
                GoToPanel(NearestPanel);
            }
        }
        else if (snapTarget == SnapTarget.Next)
        {
            if ((_releaseDirection == Direction.Right && displacementFromCenter.x > 0f) ||
                (_releaseDirection == Direction.Up && displacementFromCenter.y > 0f))
            {
                GoToPreviousPanel();
            }
            else if ((_releaseDirection == Direction.Left && displacementFromCenter.x < 0f) ||
                     (_releaseDirection == Direction.Down && displacementFromCenter.y < 0f))
            {
                GoToNextPanel();
            }
            else
            {
                GoToPanel(NearestPanel);
            }
        }
    }

    private void SnapToTargetPanel()
    {
        var viewportRect = Viewport.rect;

        var xOffset = (movementAxis == MovementAxis.Horizontal || movementType == MovementType.Free)
            ? viewportRect.width / 2f
            : 0f;
        var yOffset = (movementAxis == MovementAxis.Vertical || movementType == MovementType.Free)
            ? viewportRect.height / 2f
            : 0f;
        var offset = new Vector2(xOffset, yOffset);

        var targetPosition = -PanelsRt[TargetPanel].anchoredPosition + offset;
        Content.anchoredPosition = Vector2.Lerp(Content.anchoredPosition, targetPosition,
            (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * snappingSpeed);

        if (CurrentPanel != TargetPanel)
        {
            if (DisplacementFromCenter(TargetPanel).magnitude < (Viewport.rect.width / 10f))
            {
                CurrentPanel = TargetPanel;
                onPanelChanged.Invoke();
            }
            else
            {
                onPanelChanging.Invoke();
            }
        }
    }

    private void OnSelectingAndSnapping()
    {
        if (_selected)
        {
            if (!((_dragging || _pressing) && swipeGestures))
            {
                SnapToTargetPanel();
            }
        }
        else if (!_dragging &&
                 (_scrollRect.velocity.magnitude <= thresholdSnappingSpeed || thresholdSnappingSpeed == -1f))
        {
            SelectTargetPanel();
        }
    }

    private void OnOcclusionCulling(bool forceUpdate = false)
    {
        if (useOcclusionCulling && (_velocity.magnitude > 0f || forceUpdate))
        {
            for (var i = 0; i < NumberOfPanels; i++)
            {
                if (movementAxis == MovementAxis.Horizontal)
                {
                    Panels[i].SetActive(Mathf.Abs(DisplacementFromCenter(i).x) <= Viewport.rect.width / 2f + size.x);
                }
                else if (movementAxis == MovementAxis.Vertical)
                {
                    Panels[i].SetActive(Mathf.Abs(DisplacementFromCenter(i).y) <= Viewport.rect.height / 2f + size.y);
                }
            }
        }
    }

    private void OnInfiniteScrolling(bool forceUpdate = false)
    {
        if (infinitelyScroll && (_velocity.magnitude > 0 || forceUpdate))
        {
            switch (movementAxis)
            {
                case MovementAxis.Horizontal:
                {
                    for (var i = 0; i < NumberOfPanels; i++)
                    {
                        if (DisplacementFromCenter(i).x > Content.rect.width / 2f)
                        {
                            PanelsRt[i].anchoredPosition -= new Vector2(_contentLength, 0);
                        }
                        else if (DisplacementFromCenter(i).x < Content.rect.width / -2f)
                        {
                            PanelsRt[i].anchoredPosition += new Vector2(_contentLength, 0);
                        }
                    }

                    break;
                }
                case MovementAxis.Vertical:
                {
                    for (var i = 0; i < NumberOfPanels; i++)
                    {
                        if (DisplacementFromCenter(i).y > Content.rect.height / 2f)
                        {
                            PanelsRt[i].anchoredPosition -= new Vector2(0, _contentLength);
                        }
                        else if (DisplacementFromCenter(i).y < Content.rect.height / -2f)
                        {
                            PanelsRt[i].anchoredPosition += new Vector2(0, _contentLength);
                        }
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void OnTransitionEffects()
    {
        if (transitionEffects.Count == 0) return;

        for (var i = 0; i < NumberOfPanels; i++)
        {
            foreach (var transitionEffect in transitionEffects)
            {
                // Displacement
                var displacement = 0f;
                if (movementType == MovementType.Fixed)
                {
                    if (movementAxis == MovementAxis.Horizontal)
                    {
                        displacement = DisplacementFromCenter(i).x;
                    }
                    else if (movementAxis == MovementAxis.Vertical)
                    {
                        displacement = DisplacementFromCenter(i).y;
                    }
                }
                else
                {
                    displacement = DisplacementFromCenter(i).magnitude;
                }

                // Value
                var panel = PanelsRt[i];
                var panelTransform = panel.transform;
                var panelPosition = panelTransform.localPosition;
                var panelScale = panelTransform.localScale;
                var panelRotation = panelTransform.localEulerAngles;
                switch (transitionEffect.Label)
                {
                    case "localPosition.z":
                        panel.transform.localPosition = new Vector3(panelPosition.x, panelPosition.y,
                            transitionEffect.GetValue(displacement));
                        break;
                    case "localScale.x":
                        panel.transform.localScale = new Vector2(transitionEffect.GetValue(displacement), panelScale.y);
                        break;
                    case "localScale.y":
                        panel.transform.localScale = new Vector2(panelScale.x, transitionEffect.GetValue(displacement));
                        break;
                    case "localRotation.x":
                        panel.transform.localRotation = Quaternion.Euler(new Vector3(
                            transitionEffect.GetValue(displacement), panelRotation.y, panelRotation.z));
                        break;
                    case "localRotation.y":
                        panel.transform.localRotation = Quaternion.Euler(new Vector3(
                            panelRotation.x, transitionEffect.GetValue(displacement), panelRotation.z));
                        break;
                    case "localRotation.z":
                        panel.transform.localRotation = Quaternion.Euler(new Vector3(
                            panelRotation.x, panelRotation.y, transitionEffect.GetValue(displacement)));
                        break;
                }
            }
        }
    }

    private void OnSwipeGestures()
    {
        if (swipeGestures)
        {
            _scrollRect.horizontal = movementAxis == MovementAxis.Horizontal || movementType == MovementType.Free;
            _scrollRect.vertical = movementAxis == MovementAxis.Vertical || movementType == MovementType.Free;
        }
        else
        {
            _scrollRect.horizontal = _scrollRect.vertical = !_dragging;
        }
    }

    public void GoToPanel(int panelNumber)
    {
        TargetPanel = panelNumber;

        _selected = true;
        onPanelSelected.Invoke();

        if (inertia)
            _scrollRect.inertia = false;
    }

    public void GoToPreviousPanel()
    {
        NearestPanel = DetermineNearestPanel();
        if (NearestPanel != 0)
        {
            GoToPanel(NearestPanel - 1);
        }
        else
        {
            if (infinitelyScroll)
            {
                GoToPanel(NumberOfPanels - 1);
            }
            else
            {
                GoToPanel(NearestPanel);
            }
        }
    }

    public void GoToNextPanel()
    {
        NearestPanel = DetermineNearestPanel();
        if (NearestPanel != (NumberOfPanels - 1))
        {
            GoToPanel(NearestPanel + 1);
        }
        else
        {
            if (infinitelyScroll)
                GoToPanel(0);
            else
                GoToPanel(NearestPanel);
        }
    }

    public void AddToFront(GameObject panel)
    {
        Add(panel, 0);
    }

    public void AddToBack(GameObject panel)
    {
        Add(panel, NumberOfPanels);
    }

    public void Add(GameObject panel, int index)
    {
        if (NumberOfPanels != 0 && (index < 0 || index > NumberOfPanels))
        {
            Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + NumberOfPanels + ".",
                gameObject);
            return;
        }

        if (!automaticallyLayout)
        {
            Debug.LogError(
                "<b>[SimpleScrollSnap]</b> \"Automatic Layout\" must be enabled for content to be dynamically added during runtime.");
            return;
        }

        panel = Instantiate(panel, Content, false);
        panel.transform.SetSiblingIndex(index);

        if (Validate())
        {
            if (TargetPanel <= index)
                startingPanel = TargetPanel;
            else
                startingPanel = TargetPanel + 1;

            Setup();
        }
    }

    public void RemoveFromFront()
    {
        Remove(0);
    }

    public void RemoveFromBack()
    {
        if (NumberOfPanels > 0)
        {
            Remove(NumberOfPanels - 1);
        }
        else
        {
            Remove(0);
        }
    }

    public void Remove(int index)
    {
        if (NumberOfPanels == 0)
        {
            Debug.LogError("<b>[SimpleScrollSnap]</b> There are no panels to remove.", gameObject);
            return;
        }

        if (index < 0 || index > (NumberOfPanels - 1))
        {
            Debug.LogError("<b>[SimpleScrollSnap]</b> Index must be an integer from 0 to " + (NumberOfPanels - 1) + ".",
                gameObject);
            return;
        }

        if (!automaticallyLayout)
        {
            Debug.LogError(
                "<b>[SimpleScrollSnap]</b> \"Automatic Layout\" must be enabled for content to be dynamically removed during runtime.");
            return;
        }

        DestroyImmediate(Panels[index]);

        if (Validate())
        {
            if (TargetPanel == index)
            {
                if (index == NumberOfPanels)
                    startingPanel = TargetPanel - 1;
                else
                    startingPanel = TargetPanel;
            }
            else if (TargetPanel < index)
            {
                startingPanel = TargetPanel;
            }
            else
            {
                startingPanel = TargetPanel - 1;
            }

            Setup();
        }
    }

    public void AddVelocity(Vector2 velocity)
    {
        _scrollRect.velocity += velocity;
        _selected = false;
    }
}