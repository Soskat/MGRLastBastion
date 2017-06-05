using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FuzzyPropertiesTest))]
public class FuzzyPropertiesTestInspector : Editor {

    public override void OnInspectorGUI()
    {
        serializedObject.Update();  // synchronize serialized object with the component it represents
        EditorList.Show(serializedObject.FindProperty("rules"), EditorListOption.NoElementLabels | EditorListOption.Buttons);
        serializedObject.ApplyModifiedProperties(); // commit any changes
    }

}
