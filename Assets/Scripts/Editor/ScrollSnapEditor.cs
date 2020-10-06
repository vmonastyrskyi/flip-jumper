using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    [CustomEditor(typeof(ScrollSnap))]
    public class ScrollSnapEditor : UnityEditor.Editor
    {
        private int _selectedProperty;
        private float _selectedMinValue;
        private float _selectedMaxValue;
        private float _selectedMinDisplacement;
        private float _selectedMaxDisplacement;
        private bool _showTransitionEffects = true;
        private bool _showMovement = true;
        private bool _showMargin = true;
        private bool _showNavigation = true;
        private bool _showSelection = true;
        private bool _showEvents = true;
        private bool _showDisplacement;
        private bool _showValue;
        private SerializedProperty _movementType;
        private SerializedProperty _movementAxis;
        private SerializedProperty _automaticallyLayout;
        private SerializedProperty _sizeControl;
        private SerializedProperty _size;
        private SerializedProperty _automaticLayoutSpacing;
        private SerializedProperty _leftMargin;
        private SerializedProperty _rightMargin;
        private SerializedProperty _topMargin;
        private SerializedProperty _bottomMargin;
        private SerializedProperty _infinitelyScroll;
        private SerializedProperty _infiniteScrollingEndSpacing;
        private SerializedProperty _startingPanel;
        private SerializedProperty _swipeGestures;
        private SerializedProperty _minimumSwipeSpeed;
        private SerializedProperty _previousButton;
        private SerializedProperty _nextButton;
        private SerializedProperty _snapTarget;
        private SerializedProperty _snappingSpeed;
        private SerializedProperty _useUnscaledTime;
        private SerializedProperty _useOcclusionCulling;
        private SerializedProperty _thresholdSnappingSpeed;
        private SerializedProperty _inertia;
        private SerializedProperty _onPanelSelecting;
        private SerializedProperty _onPanelSelected;
        private SerializedProperty _onPanelChanging;
        private SerializedProperty _onPanelChanged;
        private ScrollSnap _scrollSnap;
        private AnimationCurve _selectedFunction = AnimationCurve.Constant(0, 1, 1);

        private void OnEnable()
        {
            _scrollSnap = target as ScrollSnap;

            // Serialized Properties
            _movementType = serializedObject.FindProperty("movementType");
            _movementAxis = serializedObject.FindProperty("movementAxis");
            _automaticallyLayout = serializedObject.FindProperty("automaticallyLayout");
            _sizeControl = serializedObject.FindProperty("sizeControl");
            _size = serializedObject.FindProperty("size");
            _automaticLayoutSpacing = serializedObject.FindProperty("automaticLayoutSpacing");
            _leftMargin = serializedObject.FindProperty("leftMargin");
            _rightMargin = serializedObject.FindProperty("rightMargin");
            _topMargin = serializedObject.FindProperty("topMargin");
            _bottomMargin = serializedObject.FindProperty("bottomMargin");
            _infinitelyScroll = serializedObject.FindProperty("infinitelyScroll");
            _useOcclusionCulling = serializedObject.FindProperty("useOcclusionCulling");
            _infiniteScrollingEndSpacing = serializedObject.FindProperty("infiniteScrollingEndSpacing");
            _startingPanel = serializedObject.FindProperty("startingPanel");
            _swipeGestures = serializedObject.FindProperty("swipeGestures");
            _minimumSwipeSpeed = serializedObject.FindProperty("minimumSwipeSpeed");
            _previousButton = serializedObject.FindProperty("previousButton");
            _nextButton = serializedObject.FindProperty("nextButton");
            _snapTarget = serializedObject.FindProperty("snapTarget");
            _snappingSpeed = serializedObject.FindProperty("snappingSpeed");
            _thresholdSnappingSpeed = serializedObject.FindProperty("thresholdSnappingSpeed");
            _inertia = serializedObject.FindProperty("inertia");
            _useUnscaledTime = serializedObject.FindProperty("useUnscaledTime");
            _onPanelSelecting = serializedObject.FindProperty("onPanelSelecting");
            _onPanelSelected = serializedObject.FindProperty("onPanelSelected");
            _onPanelChanging = serializedObject.FindProperty("onPanelChanging");
            _onPanelChanged = serializedObject.FindProperty("onPanelChanged");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            MovementAndLayoutSettings();
            NavigationSettings();
            SnapSettings();
            TransitionEffects();
            EventHandlers();

            serializedObject.ApplyModifiedProperties();
            PrefabUtility.RecordPrefabInstancePropertyModifications(_scrollSnap);
        }

        private void MovementAndLayoutSettings()
        {
            EditorGUILayout.Space();

            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            _showMovement = EditorGUILayout.Foldout(_showMovement, "Movement and Layout Settings", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (_showMovement)
            {
                MovementType();
                StartingPanel();
            }

            EditorGUILayout.Space();
        }

        private void MovementType()
        {
            EditorGUILayout.PropertyField(_movementType,
                new GUIContent("Movement Type",
                    "Determines how users will be able to move between panels within the ScrollRect."));
            if (_scrollSnap.movementType == ScrollSnap.MovementType.Fixed)
            {
                EditorGUI.indentLevel++;

                MovementAxis();
                AutomaticLayout();
                InfiniteScrolling();
                UseOcclusionCulling();

                EditorGUI.indentLevel--;
            }
        }

        private void MovementAxis()
        {
            EditorGUILayout.PropertyField(_movementAxis,
                new GUIContent("Movement Axis", "Determines the axis the user's movement will be restricted to."));
        }

        private void AutomaticLayout()
        {
            EditorGUILayout.PropertyField(_automaticallyLayout,
                new GUIContent("Automatic Layout",
                    "Should panels be automatically positioned and scaled according to the specified movement axis, spacing, margins and size?"));
            if (_scrollSnap.automaticallyLayout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_sizeControl,
                    new GUIContent("Size Control", "Determines how the panels' size should be controlled."));
                if (_scrollSnap.sizeControl == ScrollSnap.SizeControl.Manual)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_size,
                        new GUIContent("Size",
                            "The size (in pixels) that panels will be when automatically laid out."));
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Slider(_automaticLayoutSpacing, 0, 1,
                    new GUIContent("Spacing",
                        "The spacing between panels, calculated using a fraction of the panel’s width (if the movement axis is horizontal) or height (if the movement axis is vertical)."));
                _showMargin = EditorGUILayout.Foldout(_showMargin,
                    new GUIContent("Margin", "The size of border (in pixels) for each panel."), true);
                if (_showMargin)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_leftMargin, new GUIContent("Left"));
                    EditorGUILayout.PropertyField(_rightMargin, new GUIContent("Right"));
                    EditorGUILayout.PropertyField(_topMargin, new GUIContent("Top"));
                    EditorGUILayout.PropertyField(_bottomMargin, new GUIContent("Bottom"));
                    EditorGUI.indentLevel--;
                }

                EditorGUI.indentLevel--;
            }
        }

        private void InfiniteScrolling()
        {
            EditorGUILayout.PropertyField(_infinitelyScroll,
                new GUIContent("Infinite Scrolling",
                    "Should panels wrap around to the opposite end once passed, giving the illusion of an infinite list of elements?"));
            if (_scrollSnap.infinitelyScroll)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Slider(_infiniteScrollingEndSpacing, 0, 1,
                    new GUIContent("End Spacing",
                        "The spacing maintained between panels once wrapped around to the opposite end."));
                EditorGUI.indentLevel--;
            }
        }

        private void UseOcclusionCulling()
        {
            EditorGUILayout.PropertyField(_useOcclusionCulling,
                new GUIContent("Use Occlusion Culling", "Should panels not visible in the viewport be disabled?"));
        }

        private void StartingPanel()
        {
            EditorGUILayout.IntSlider(_startingPanel, 0, _scrollSnap.NumberOfPanels - 1,
                new GUIContent("Starting Panel",
                    "The number of the panel that will be displayed first, based on a 0-indexed array."));
        }

        private void NavigationSettings()
        {
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            _showNavigation = EditorGUILayout.Foldout(_showNavigation, "Navigation Settings", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (_showNavigation)
            {
                SwipeGestures();
                PreviousButton();
                NextButton();
            }

            EditorGUILayout.Space();
        }

        private void SwipeGestures()
        {
            EditorGUILayout.PropertyField(_swipeGestures,
                new GUIContent("Swipe Gestures",
                    "Should users are able to use swipe gestures to navigate between panels?"));
            if (_scrollSnap.swipeGestures)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_minimumSwipeSpeed,
                    new GUIContent("Minimum Swipe Speed",
                        "The speed at which the user must be swiping in order for a transition to occur to another panel."));
                EditorGUI.indentLevel--;
            }
        }

        private void PreviousButton()
        {
            EditorGUILayout.ObjectField(_previousButton, typeof(Button),
                new GUIContent("Previous Button", "(Optional) Button used to transition to the previous panel."));
        }

        private void NextButton()
        {
            EditorGUILayout.ObjectField(_nextButton, typeof(Button),
                new GUIContent("Next Button", "(Optional) Button used to transition to the next panel."));
        }

        private void SnapSettings()
        {
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            _showSelection = EditorGUILayout.Foldout(_showSelection, "Snap Settings", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (_showSelection)
            {
                SnapTarget();
                SnapSpeed();
                ThresholdSnapSpeed();
                HardSnap();
                UseUnscaledTime();
            }

            EditorGUILayout.Space();
        }

        private void SnapTarget()
        {
            using (new EditorGUI.DisabledScope(_scrollSnap.movementType == ScrollSnap.MovementType.Free))
            {
                EditorGUILayout.PropertyField(_snapTarget,
                    new GUIContent("Snap Target",
                        "Determines what panel should be targeted and snapped to once the threshold snapping speed has been reached."));
            }

            if (_scrollSnap.movementType == ScrollSnap.MovementType.Free)
            {
                _scrollSnap.snapTarget = ScrollSnap.SnapTarget.Nearest;
            }
        }

        private void SnapSpeed()
        {
            EditorGUILayout.PropertyField(_snappingSpeed,
                new GUIContent("Snap Speed", "The speed at which the targeted panel snaps into position."));
        }

        private void ThresholdSnapSpeed()
        {
            EditorGUILayout.PropertyField(_thresholdSnappingSpeed,
                new GUIContent("Threshold Snap Speed",
                    "The speed at which the ScrollRect will stop scrolling and begin snapping to the targeted panel (where -1 is used as infinity)."));
        }

        private void HardSnap()
        {
            EditorGUILayout.PropertyField(_inertia,
                new GUIContent("Hard Snap",
                    "Should the inertia of the ScrollRect be disabled once a panel has been selected? If enabled, the ScrollRect will not overshoot the targeted panel when snapping into position and instead Lerp precisely towards the targeted panel."));
        }

        private void UseUnscaledTime()
        {
            EditorGUILayout.PropertyField(_useUnscaledTime,
                new GUIContent("Use Unscaled Time", "Should the scroll-snap update irrespective of the time scale?"));
        }

        private void TransitionEffects()
        {
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            _showTransitionEffects = EditorGUILayout.Foldout(_showTransitionEffects,
                new GUIContent("Transition Effects",
                    "Effects applied to panels based on their distance from the center."), true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (_showTransitionEffects)
            {
                EditorGUI.indentLevel++;
                AddTransitionEffect();
                InitTransitionEffects();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
        }

        private void AddTransitionEffect()
        {
            // Properties
            var properties = new List<string>()
            {
                "localPosition.z",
                "localScale.x",
                "localScale.y",
                "localRotation.x",
                "localRotation.y",
                "localRotation.z",
                "color.a",
                "color.gray"
            };
            foreach (var transitionEffect in _scrollSnap.transitionEffects)
            {
                properties.Remove(transitionEffect.Label);
            }

            _selectedProperty =
                EditorGUILayout.Popup(
                    new GUIContent("Property",
                        "The selected property of a panel that will be affected by the distance from the centre."),
                    _selectedProperty, properties.ToArray());

            // Selected Min/Max Values
            _showValue = EditorGUILayout.Foldout(_showValue, "Value", true);
            if (_showValue)
            {
                EditorGUI.indentLevel++;
                _selectedMinValue =
                    EditorGUILayout.FloatField(new GUIContent("Min", "The minimum value that can be assigned."),
                        _selectedMinValue);
                _selectedMaxValue =
                    EditorGUILayout.FloatField(new GUIContent("Max", "The maximum value that can be assigned."),
                        _selectedMaxValue);
                EditorGUI.indentLevel--;
            }

            // Selected Min/Max Displacements
            _showDisplacement = EditorGUILayout.Foldout(_showDisplacement, "Displacement", true);
            if (_showDisplacement)
            {
                EditorGUI.indentLevel++;
                _selectedMinDisplacement = EditorGUILayout.FloatField(
                    new GUIContent("Min", "The minimum displacement at which the value will be affected."),
                    _selectedMinDisplacement);
                _selectedMaxDisplacement = EditorGUILayout.FloatField(
                    new GUIContent("Max", "The maximum displacement at which the value will be affected."),
                    _selectedMaxDisplacement);
                EditorGUI.indentLevel--;
            }

            // Selected Function
            var x = _selectedMinDisplacement;
            var y = _selectedMinValue;
            var width = _selectedMaxDisplacement - _selectedMinDisplacement;
            var height = _selectedMaxValue - _selectedMinValue;

            _selectedFunction = EditorGUILayout.CurveField(
                new GUIContent("Function",
                    "The function (with respect to displacement from centre) that will be used to determine the value."),
                _selectedFunction, Color.white, new Rect(x, y, width, height));

            // Add Transition Effect
            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * 16);
            if (GUILayout.Button("Add Transition Effect"))
            {
                var function = new AnimationCurve(_selectedFunction.keys);
                _scrollSnap.transitionEffects.Add(new TransitionEffect(properties[_selectedProperty], _selectedMinValue,
                    _selectedMaxValue, _selectedMinDisplacement, _selectedMaxDisplacement, function, _scrollSnap));
            }

            GUILayout.EndHorizontal();
        }

        private void InitTransitionEffects()
        {
            // Initialize
            foreach (var transitionEffect in _scrollSnap.transitionEffects)
            {
                transitionEffect.Init();
            }
        }

        private void EventHandlers()
        {
            EditorStyles.foldout.fontStyle = FontStyle.Bold;
            _showEvents = EditorGUILayout.Foldout(_showEvents, "Event Handlers", true);
            EditorStyles.foldout.fontStyle = FontStyle.Normal;

            if (_showEvents)
            {
                EditorGUILayout.PropertyField(_onPanelSelecting, new GUIContent("On Panel Selecting"));
                EditorGUILayout.PropertyField(_onPanelSelected, new GUIContent("On Panel Selected"));
                EditorGUILayout.PropertyField(_onPanelChanging, new GUIContent("On Panel Changing"));
                EditorGUILayout.PropertyField(_onPanelChanged, new GUIContent("On Panel Changed"));
            }
        }
    }
}