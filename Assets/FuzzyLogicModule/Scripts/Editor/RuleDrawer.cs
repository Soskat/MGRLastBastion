using UnityEngine;
using UnityEditor;
using FuzzyLogicEngine.Rules;

[CustomPropertyDrawer(typeof(Rule))]
public class RuleDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int oldIndentLevel = EditorGUI.indentLevel;
        label = EditorGUI.BeginProperty(position, label, property);
        {
            // draw label
            Rect contentPos = EditorGUI.PrefixLabel(position, label);
            EditorGUI.indentLevel = 0;

            float contentWidth = contentPos.width * 0.25f;
            Rect condition1Rect = new Rect(contentPos.x, contentPos.y, contentWidth, contentPos.height);
            Rect ruleOperRect = new Rect(contentPos.x + contentWidth, contentPos.y, contentWidth, contentPos.height);
            Rect condition2Rect = new Rect(contentPos.x + 2 * contentWidth, contentPos.y, contentWidth, contentPos.height);
            Rect conclusionRect = new Rect(contentPos.x + 3 * contentWidth, contentPos.y, contentWidth, contentPos.height);
            
            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(condition1Rect, property.FindPropertyRelative("condition1"), GUIContent.none);
            EditorGUI.PropertyField(ruleOperRect, property.FindPropertyRelative("ruleOper"), GUIContent.none);
            EditorGUI.PropertyField(condition2Rect, property.FindPropertyRelative("condition2"), GUIContent.none);
            EditorGUI.PropertyField(conclusionRect, property.FindPropertyRelative("conclusion"), GUIContent.none);
        }
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oldIndentLevel;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 3;
    }
}
