using UnityEditor;
using UnityEngine;
using System;

[Flags]
public enum EditorListOption
{
    None = 0,                                       // show none
    ListSize = 1,                                   // show list size
    ListLabel = 2,                                  // show list label
    ElementLabels = 4,                              // show list's element label
    Buttons = 8,                                    // show buttons
    Default = ListSize | ListLabel | ElementLabels, // use all options
    NoElementLabels = ListSize | ListLabel,         // use all options without element labels
    All = Default | Buttons
}