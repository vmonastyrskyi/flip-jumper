using UnityEditor;
using UnityEditor.UI;

namespace Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(NonDrawingGraphic))]
    public class NonDrawingGraphicEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Script);
            RaycastControlsGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}