using LastBastion.Analytics;
using LastBastion.Game.Managers;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game
{
    public class DebugInfo : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private GUISkin guiSkin;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        private void Start()
        {
            Assert.IsNotNull(guiSkin);
        }

        private void OnGUI()
        {
            if (guiSkin != null) GUI.skin = guiSkin;

            // background box:
            GUI.Box(new Rect(0, 0, 100, 90), GUIContent.none);
            // game data:
            GUI.Label(new Rect(10, 10, 40, 12), LevelManager.instance.LevelName.ToString());
            GUI.Label(new Rect(60, 10, 80, 12), ((BiofeedbackMode)GameManager.instance.GameMode).ToString());
            // HR and GSR data:
            GUI.Label(new Rect(43, 28, 46, 12), "Average:");
            GUI.Label(new Rect(94, 28, 42, 12), "Current:");
            GUI.Label(new Rect(10, 42, 28, 12), "HR:");
            GUI.Label(new Rect(43, 42, 46, 12), GameManager.instance.BBModule.AverageHr.ToString());
            GUI.Label(new Rect(94, 42, 42, 12), GameManager.instance.BBModule.CurrentHr.ToString());
            GUI.Label(new Rect(10, 56, 28, 12), "GSR:");
            GUI.Label(new Rect(43, 56, 46, 12), GameManager.instance.BBModule.AverageGsr.ToString());
            GUI.Label(new Rect(94, 56, 42, 12), GameManager.instance.BBModule.CurrentGsr.ToString());
            // current modifiers and arousal state:
            GUI.Label(new Rect(55, 74, 25, 12), "HR");
            GUI.Label(new Rect(55, 88, 25, 12), GameManager.instance.BBModule.HrModifier.ToString());
            GUI.Label(new Rect(85, 74, 25, 12), "GSR");
            GUI.Label(new Rect(85, 88, 25, 12), GameManager.instance.BBModule.GsrModifier.ToString());
            GUI.Label(new Rect(115, 74, 40, 12), "Arousal");
            GUI.Label(new Rect(115, 88, 40, 12), GameManager.instance.BBModule.ArousalModifier.ToString());
            GUI.Label(new Rect(10, 88, 40, 12), "Modifier:");
            GUI.Label(new Rect(10, 104, 68, 12), "Arousal state:");
            GUI.Label(new Rect(83, 104, 50, 12), GameManager.instance.BBModule.ArousalState.ToString());
        }
        #endregion
    }
}
