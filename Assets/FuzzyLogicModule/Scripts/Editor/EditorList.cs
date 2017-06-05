using UnityEditor;
using UnityEngine;

public static class EditorList
{
    // option buttons
    /// <summary> Button for moving the element down in the list. </summary>
    private static GUIContent moveButtonContent = new GUIContent("\u21b4", "move down");
    /// <summary> Button for duplicationg the element. </summary>
    private static GUIContent duplicateButtonContent = new GUIContent("+", "duplicate");
    /// <summary> Button for deleting the element. </summary>
    private static GUIContent deleteButtonContent = new GUIContent("-", "delete");
    /// <summary> Button for adding new element to the list. </summary>
    private static GUIContent addButtonContent = new GUIContent("+", "add element");
    /// <summary> Style for mini option button. </summary>
    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    /// <summary>
    /// Shows customized list.
    /// </summary>
    /// <param name="list">List to show</param>
    /// <param name="options">List's options</param>
    public static void Show(SerializedProperty list, EditorListOption options = EditorListOption.Default)
    {
        // check if property is actually a list or an array:
        if (!list.isArray)
        {
            EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error); // show error
            return;
        }


        bool showListLabel = (options & EditorListOption.ListLabel) != 0;
        bool showListSize = (options & EditorListOption.ListSize) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }
        if (list.isExpanded)
        {
            SerializedProperty size = list.FindPropertyRelative("Array.size");
            if (showListSize)
            {
                EditorGUILayout.PropertyField(size); // show the array size
            }
            if (size.hasMultipleDifferentValues)
            {
                EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);   // show info
            }
            else
            {
                ShowElements(list, options);
            }
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }

    /// <summary>
    /// Shows customized list's elements.
    /// </summary>
    /// <param name="list">List of elements to show</param>
    /// <param name="options">List's options</param>
    private static void ShowElements(SerializedProperty list, EditorListOption options)
    {
        bool showElementLabels = (options & EditorListOption.ElementLabels) != 0;
        bool showButtons = (options & EditorListOption.Buttons) != 0;

        for (int i = 0; i < list.arraySize; i++)
        {
            if (showButtons)
            {
                EditorGUILayout.BeginHorizontal();
            }

            if (showElementLabels)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            else
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
            }

            if (showButtons)
            {
                ShowButtons(list, i);
                EditorGUILayout.EndHorizontal();
            }
        }

        // if list is empty, show a button for adding new element:
        if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
        {
            list.arraySize += 1;
        }
    }


    /// <summary>
    /// Shows element's options buttons.
    /// </summary>
    /// <param name="list">List of elements</param>
    /// <param name="index">Index of the element</param>
    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            list.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            list.InsertArrayElementAtIndex(index);
        }
        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }
    }
}