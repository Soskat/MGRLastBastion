using LastBastion.Analytics;
using LastBastion.Game.UIControllers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Level scene behaviour.
    /// </summary>
    public class LevelPreManager : MonoBehaviour
    {

        #region Private fields
        [SerializeField] private string sceneName;
        [SerializeField] private Button endSceneButton;
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private GameObject sensorsPanel;
        private SensorPanelController sensorPanelController;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(endSceneButton);
            Assert.IsNotNull(backToMainMenuButton);
            Assert.IsNotNull(sensorsPanel);
        }

        // Use this for initialization
        void Start()
        {
            endSceneButton.onClick.AddListener(() => { GameManager.instance.LevelHasEnded(); });
            backToMainMenuButton.onClick.AddListener(() => { GameManager.instance.BackToMainMenu(); });
            sensorPanelController = sensorsPanel.GetComponent<SensorPanelController>();

            // set average readings value labels:
            sensorPanelController.UpdateAverageReadings(GameManager.instance.BBModule.AverageHr, GameManager.instance.BBModule.AverageGsr);

            // save level info:
            if (GameManager.instance.AnalyticsEnabled)
            {
                DataManager.AddLevelInfo(sceneName, GameManager.instance.CurrentCalculationType, GameManager.instance.BBModule.AverageHr, GameManager.instance.BBModule.AverageGsr);
                GameManager.instance.SetTime();
                DataManager.AddGameEvent(Analytics.EventType.GameStart, GameManager.instance.GetTime);
            }
        }

        // Update is called once per frame
        void Update()
        {

            // get current Band sensors readings:
            if (GameManager.instance.BBModule.CanReceiveBandReadings && GameManager.instance.BBModule.IsBandPaired && GameManager.instance.IsReadyForNewBandData)
            {
                GameManager.instance.BBModule.GetBandData();
                GameManager.instance.IsReadyForNewBandData = false;
            }

            // Update GUI if needed: =============================================

            // update sensors readings values:
            if (GameManager.instance.BBModule.IsSensorsReadingsChanged)
            {
                if (GameManager.instance.BBModule.IsBandPaired)
                {
                    sensorPanelController.UpdateCurrentReadings(GameManager.instance.BBModule.CurrentHr, GameManager.instance.BBModule.CurrentGsr);

                    // save new sensors readings values:
                    if (GameManager.instance.AnalyticsEnabled)
                    {
                        GameManager.instance.SetTime();
                        DataManager.AddGameEvent(Analytics.EventType.HrData, GameManager.instance.GetTime, GameManager.instance.BBModule.CurrentHr);
                        DataManager.AddGameEvent(Analytics.EventType.GsrData, GameManager.instance.GetTime, GameManager.instance.BBModule.CurrentGsr);
                        // arousal ...
                    }
                }
                else sensorPanelController.ResetLabels();

                GameManager.instance.BBModule.IsSensorsReadingsChanged = false;
                GameManager.instance.IsReadyForNewBandData = true;
            }

            // reset labels if lost connection with MS Band device:
            if (!GameManager.instance.BBModule.IsBandPaired) sensorPanelController.ResetLabels();
        }
        #endregion
    }
}