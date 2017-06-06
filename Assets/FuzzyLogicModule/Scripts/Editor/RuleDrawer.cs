using UnityEngine;
using UnityEditor;
using FuzzyLogicEngine.Rules;

[CustomPropertyDrawer(typeof(Rule))]
public class RuleDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        {
            // save current indent level:
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            // draw object's label:
            EditorGUIUtility.labelWidth = 80;
            Rect contentPos = EditorGUI.PrefixLabel(position, label);
            // calculate rectangles for properties:
            float contentWidth = contentPos.width * 0.25f;
            float ruleOperOffset = contentPos.height * 0.25f;
            Rect condition1Rect = new Rect(contentPos.x, contentPos.y, contentWidth, contentPos.height);
            Rect ruleOperRect = new Rect(contentPos.x + contentWidth, contentPos.y + ruleOperOffset, contentWidth, contentPos.height);
            Rect condition2Rect = new Rect(contentPos.x + 2 * contentWidth, contentPos.y, contentWidth, contentPos.height);
            Rect conclusionRect = new Rect(contentPos.x + 3 * contentWidth, contentPos.y, contentWidth, contentPos.height);
            // draw background rectangle for properties:
            EditorGUI.DrawRect(contentPos, new Color(0.1f, 0.1f, 0.1f));
            // draw properties:
            EditorGUI.PropertyField(condition1Rect, property.FindPropertyRelative("condition1"), GUIContent.none);
            EditorGUI.PropertyField(ruleOperRect, property.FindPropertyRelative("ruleOper"), GUIContent.none);
            EditorGUI.PropertyField(condition2Rect, property.FindPropertyRelative("condition2"), GUIContent.none);
            EditorGUI.PropertyField(conclusionRect, property.FindPropertyRelative("conclusion"), GUIContent.none);
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
