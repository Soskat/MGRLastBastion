using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    
    [SerializeField] private string sceneName;
    [SerializeField] private Button endSceneButton;
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private GameObject sensorsPanel;

    private SensorPanelController sensorPanelController;


    private void Awake()
    {
        Assert.IsNotNull(endSceneButton);
        Assert.IsNotNull(backToMainMenuButton);
        Assert.IsNotNull(sensorsPanel);
    }


    // Use this for initialization
    void Start () {
        endSceneButton.onClick.AddListener(() => { GameManager.instance.LevelHasEnded(); });
        backToMainMenuButton.onClick.AddListener(() => { GameManager.instance.BackToMainMenu(); });
        sensorPanelController = sensorsPanel.GetComponent<SensorPanelController>();

        // set average readings value labels:
        sensorPanelController.UpdateAverageReadings(GameManager.instance.BBModule.AverageHrReading, GameManager.instance.BBModule.AverageGsrReading);

        // save level info:
        if (GameManager.instance.AnalyticsEnabled)
        {
            DataManager.AddLevelInfo(sceneName, GameManager.instance.CurrentCalculationType, GameManager.instance.BBModule.AverageHrReading, GameManager.instance.BBModule.AverageGsrReading);
            GameManager.instance.SetTime();
            DataManager.AddGameEvent(EventType.GameStart, GameManager.instance.GetTime);
        }
    }
	
	// Update is called once per frame
	void Update () {

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
                sensorPanelController.UpdateCurrentReadings(GameManager.instance.BBModule.CurrentHrReading, GameManager.instance.BBModule.CurrentGsrReading);

                // save new sensors readings values:
                if (GameManager.instance.AnalyticsEnabled)
                {
                    GameManager.instance.SetTime();
                    DataManager.AddGameEvent(EventType.HrData, GameManager.instance.GetTime, GameManager.instance.BBModule.CurrentHrReading);
                    DataManager.AddGameEvent(EventType.GsrData, GameManager.instance.GetTime, GameManager.instance.BBModule.CurrentGsrReading);
                    // arousal
                }
            }
            else
            {
                sensorPanelController.ResetLabels();
            }
            GameManager.instance.BBModule.IsSensorsReadingsChanged = false;
            GameManager.instance.IsReadyForNewBandData = true;
        }

        //// update average sensors readings values:
        //if (GameManager.instance.BBModule.IsAverageReadingsChanged)
        //{
        //    if (GameManager.instance.BBModule.IsBandPaired)
        //    {
        //        sensorPanelController.UpdateAverageReadings(GameManager.instance.BBModule.AverageHrReading, GameManager.instance.BBModule.AverageGsrReading);
        //    }
        //    else
        //    {
        //        sensorPanelController.ResetLabels();
        //    }
        //    GameManager.instance.BBModule.IsAverageReadingsChanged = false;
        //}

        // reset labels if lost connection with MS Band device:
        if (!GameManager.instance.BBModule.IsBandPaired)
        {
            sensorPanelController.ResetLabels();
        }


    }
}
