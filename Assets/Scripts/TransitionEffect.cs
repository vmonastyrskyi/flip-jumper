using System;
using UnityEditor;
// using UnityEditor.SceneManagement;
using UnityEngine;
// using UnityEngine.SceneManagement;

[Serializable]
public class TransitionEffect
{
    [SerializeField] protected float minDisplacement;
    [SerializeField] protected float maxDisplacement;
    [SerializeField] protected float minValue;
    [SerializeField] protected float maxValue;
    [SerializeField] protected float defaultMinValue;
    [SerializeField] protected float defaultMaxValue;
    [SerializeField] protected float defaultMinDisplacement;
    [SerializeField] protected float defaultMaxDisplacement;
    [SerializeField] protected bool showPanel;
    [SerializeField] protected bool showDisplacement;
    [SerializeField] protected bool showValue;
    [SerializeField] private string label;
    [SerializeField] private AnimationCurve function;
    [SerializeField] private AnimationCurve defaultFunction;
    [SerializeField] private ScrollSnap scrollSnap;

    public string Label
    {
        get => label;
        set => label = value;
    }

    public float MinValue
    {
        get => MinValue;
        set => minValue = value;
    }

    public float MaxValue
    {
        get => maxValue;
        set => maxValue = value;
    }

    public float MinDisplacement
    {
        get => minDisplacement;
        set => minDisplacement = value;
    }

    public float MaxDisplacement
    {
        get => maxDisplacement;
        set => maxDisplacement = value;
    }

    public AnimationCurve Function
    {
        get => function;
        set => function = value;
    }

    public TransitionEffect(string label, float minValue, float maxValue, float minDisplacement, float maxDisplacement,
        AnimationCurve function, ScrollSnap scrollSnap)
    {
        this.label = label;
        this.scrollSnap = scrollSnap;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.minDisplacement = minDisplacement;
        this.maxDisplacement = maxDisplacement;
        this.function = function;

        SetDefaultValues(minValue, maxValue, minDisplacement, maxDisplacement, function);

// #if UNITY_EDITOR
//         EditorSceneManager.MarkSceneDirty(SceneManager
//             .GetActiveScene());
// #endif
    }

    private void SetDefaultValues(float minValue, float maxValue, float minDisplacement, float maxDisplacement,
        AnimationCurve function)
    {
        defaultMinValue = minValue;
        defaultMaxValue = maxValue;
        defaultMinDisplacement = minDisplacement;
        defaultMaxDisplacement = maxDisplacement;
        defaultFunction = function;
    }

#if UNITY_EDITOR
    public void Init()
    {
        GUILayout.BeginVertical("HelpBox");
        showPanel = EditorGUILayout.Foldout(showPanel, label, true);
        if (showPanel)
        {
            EditorGUI.indentLevel++;
            var x = minDisplacement;
            var y = minValue;
            var width = maxDisplacement - minDisplacement;
            var height = maxValue - minValue;

            // Min/Max Values
            showValue = EditorGUILayout.Foldout(showValue, "Value", true);
            if (showValue)
            {
                EditorGUI.indentLevel++;
                minValue = EditorGUILayout.FloatField(new GUIContent("Min"), minValue);
                maxValue = EditorGUILayout.FloatField(new GUIContent("Max"), maxValue);
                EditorGUI.indentLevel--;
            }

            // Min/Max Displacements
            showDisplacement = EditorGUILayout.Foldout(showDisplacement, "Displacement", true);
            if (showDisplacement)
            {
                EditorGUI.indentLevel++;
                minDisplacement = EditorGUILayout.FloatField(new GUIContent("Min"), minDisplacement);
                maxDisplacement = EditorGUILayout.FloatField(new GUIContent("Max"), maxDisplacement);
                EditorGUI.indentLevel--;
            }

            // Function
            function = EditorGUILayout.CurveField("Function", function, Color.white, new Rect(x, y, width, height));

            // Reset
            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * 16);
            if (GUILayout.Button("Reset"))
            {
                Reset();
            }

            // Remove
            if (GUILayout.Button("Remove"))
            {
                scrollSnap.transitionEffects.Remove(this);
            }

            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }

        GUILayout.EndVertical();
    }
#endif

    public void Reset()
    {
        minValue = defaultMinValue;
        maxValue = defaultMaxValue;
        minDisplacement = defaultMinDisplacement;
        maxDisplacement = defaultMaxDisplacement;
        function = defaultFunction;
    }

    public float GetValue(float displacement)
    {
        return function?.Evaluate(displacement) ?? 0f;
    }
}