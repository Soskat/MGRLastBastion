using FuzzyLogicEngine.FuzzyValues;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FuzzyValueType))]
public class FuzzyValueTypeDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        {
            // save current indent level:
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            // draw object's label:
            Rect contentPosition = EditorGUI.PrefixLabel(position, label);
            // calculate rectangles for properties:
            contentPosition.height *= 0.5f;
            Rect fuzzyTypeRect = new Rect(contentPosition.x, contentPosition.y, contentPosition.width, contentPosition.height);
            Rect fuzzyValueRect = new Rect(contentPosition.x, contentPosition.y + contentPosition.height, contentPosition.width, contentPosition.height);
            // draw properties:
            EditorGUI.PropertyField(fuzzyTypeRect, property.FindPropertyRelative("Type"), GUIContent.none);
            EditorGUI.PropertyField(fuzzyValueRect, property.FindPropertyRelative("Value"), GUIContent.none);
            // restore saved indent level:
            EditorGUI.indentLevel = oldIndentLevel;
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 2;
    }
}
