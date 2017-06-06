using UnityEngine;
using UnityEditor;
using FuzzyLogicEngine.FuzzyValues;

[CustomPropertyDrawer(typeof(FuzzyValue))]
public class FuzzyValueDrawer : PropertyDrawer {
    
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
            contentPosition.width /= 3f;
            Rect linguisticVariableRect = new Rect(contentPosition.x, contentPosition.y, contentPosition.width, contentPosition.height);  
            Rect linguisticNameRect = new Rect(contentPosition.x + contentPosition.width, contentPosition.y, contentPosition.width, contentPosition.height);
            Rect membershipValueRect = new Rect(contentPosition.x + 2*contentPosition.width, contentPosition.y, contentPosition.width, contentPosition.height);
            // draw properties:
            EditorGUIUtility.labelWidth = 40f;
            EditorGUI.PropertyField(linguisticVariableRect, property.FindPropertyRelative("linguisticVariable"), new GUIContent("Type"));
            EditorGUI.PropertyField(linguisticNameRect, property.FindPropertyRelative("linguisticValue"), new GUIContent("Value"));
            EditorGUIUtility.labelWidth = 80f;
            EditorGUI.PropertyField(membershipValueRect, property.FindPropertyRelative("membershipValue"), new GUIContent("membValue"));
            // restore saved indent level:
            EditorGUI.indentLevel = oldIndentLevel;
        }
        EditorGUI.EndProperty();
    }    
}


// BACKUP

//[CustomPropertyDrawer(typeof(FuzzyValue))]
//public class FuzzyValueDrawer : PropertyDrawer
//{

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        label = EditorGUI.BeginProperty(position, label, property);
//        {
//            int oldIndentLevel = EditorGUI.indentLevel;

//            Rect contentPosition = EditorGUI.PrefixLabel(position, label);

//            // calculate rectangles for drawing properties:
//            float propertyHeight = contentPosition.height / 3;
//            Rect linguisticVariableRect = new Rect(contentPosition.x, contentPosition.y, contentPosition.width, propertyHeight);
//            Rect linguisticNameRect = new Rect(contentPosition.x, contentPosition.y + propertyHeight, contentPosition.width, propertyHeight);
//            Rect membershipValueRect = new Rect(contentPosition.x, contentPosition.y + 2 * propertyHeight, contentPosition.width, propertyHeight);

//            EditorGUI.PropertyField(linguisticVariableRect, property.FindPropertyRelative("linguisticVariable"));
//            EditorGUI.PropertyField(linguisticNameRect, property.FindPropertyRelative("linguisticValue"));
//            EditorGUI.PropertyField(membershipValueRect, property.FindPropertyRelative("membershipValue"));

//            EditorGUI.indentLevel = oldIndentLevel;
//        }
//        EditorGUI.EndProperty();
//    }


//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return base.GetPropertyHeight(property, label) * 3;
//    }

//}