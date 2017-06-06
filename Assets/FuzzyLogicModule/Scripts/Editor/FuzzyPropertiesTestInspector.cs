using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FuzzyPropertiesTest))]
public class FuzzyPropertiesTestInspector : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();  // synchronize serialized object with the component it represents
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rule"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("color"));
        EditorList.Show(serializedObject.FindProperty("rules"), EditorListOption.All);
        EditorList.Show(serializedObject.FindProperty("fuzzyValues"), EditorListOption.All);
        serializedObject.ApplyModifiedProperties(); // commit any changes
    }

}
