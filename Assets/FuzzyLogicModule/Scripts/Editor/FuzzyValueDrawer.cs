using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FuzzyLogicEngine.FuzzyValues;

[CustomPropertyDrawer(typeof(FuzzyValue))]
public class FuzzyValueDrawer : PropertyDrawer {
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        {
            int oldIndentLevel = EditorGUI.indentLevel;

            Rect contentPosition = EditorGUI.PrefixLabel(position, label);

            // calculate rectangles for drawing properties:
            float propertyHeight = contentPosition.height / 3;
            Rect linguisticVariableRect = new Rect(contentPosition.x, contentPosition.y, contentPosition.width, propertyHeight);  
            Rect linguisticNameRect = new Rect(contentPosition.x, contentPosition.y + propertyHeight, contentPosition.width, propertyHeight);
            Rect membershipValueRect = new Rect(contentPosition.x, contentPosition.y + 2 * propertyHeight, contentPosition.width, propertyHeight);

            EditorGUI.PropertyField(linguisticVariableRect, property.FindPropertyRelative("linguisticVariable"), GUIContent.none);
            EditorGUI.PropertyField(linguisticNameRect, property.FindPropertyRelative("linguisticValue"), GUIContent.none);
            EditorGUI.PropertyField(membershipValueRect, property.FindPropertyRelative("membershipValue"), GUIContent.none);
            
            EditorGUI.indentLevel = oldIndentLevel;
        }
        EditorGUI.EndProperty();
    }

    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 3;
    }
    
}
