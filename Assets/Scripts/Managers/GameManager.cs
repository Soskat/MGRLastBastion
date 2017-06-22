using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

/// <summary>
/// Component that manages all game logic.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Static fields
    /// <summary>
    /// <see cref="GameManager"/> public static object.
    /// </summary>
    public static GameManager instance;
    #endregion



    private GameType gameType;
    [SerializeField] private int currentLevelID;
    [SerializeField] private string[] gameLevels;
    [SerializeField] private int currentCalculationTypeID;
    [SerializeField] private CalculationType[] calculationTypes;
    
    public CalculationType CurrentCalculationType { get { return calculationTypes[currentCalculationTypeID]; } }



    #region Private fields
    private SensorPanelController sensorPanelController;
    #endregion


    #region Public fields & properties
    public BandBridgeModule BBModule { get; set; }
    public bool IsReadyForNewBandData { get; set; }
    public ListController ListController { get; set; }
    #endregion


    #region MonoBehaviour methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // make sure that Singleton design pattern is preserved and GameManager object will always exist:
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // make sure all objects exist:
            //DoAssertions();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        BBModule = gameObject.GetComponent<BandBridgeModule>();
        IsReadyForNewBandData = false;

        gameLevels = new string[] { "Intro", null, "Summary", null, "Summary", "Survey" };
        calculationTypes = new CalculationType[2];
        currentLevelID = 0;
        currentCalculationTypeID = 0;
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    void Update()
    {
        // get current Band sensors readings:
        if (BBModule.CanReceiveBandReadings && BBModule.IsBandPaired && IsReadyForNewBandData)
        {
            BBModule.GetBandData();
            IsReadyForNewBandData = false;
        }

        // Update GUI if needed: =============================================

        // update sensors readings values:
        if (BBModule.IsSensorsReadingsChanged)
        {
            if (BBModule.IsBandPaired)
            {
                sensorPanelController.UpdateCurrentReadings(BBModule.CurrentHrReading, BBModule.CurrentGsrReading);
            }
            else
            {
                sensorPanelController.ResetLabels();
            }
            BBModule.IsSensorsReadingsChanged = false;
            IsReadyForNewBandData = true;
        }

        // update average sensors readings values:
        if (BBModule.IsAverageReadingsChanged)
        {
            if (BBModule.IsBandPaired)
            {
                sensorPanelController.UpdateAverageReadings(BBModule.AverageHrReading, BBModule.AverageGsrReading);
            }
            else
            {
                sensorPanelController.ResetLabels();
            }
            BBModule.IsAverageReadingsChanged = false;
        }
    }
    #endregion


    #region Public methods
    /// <summary>
    /// Gets currently selected item on connected Bands list.
    /// </summary>
    /// <returns>Currently selected Band name</returns>
    public string GetChoosenBandName()
    {
        return ListController.GetSelectedItem();
    }


    public void StartNewGame()
    {
        currentLevelID = 0;
        currentCalculationTypeID = 0;

        // set levels in random order:
        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                gameLevels[1] = "LevelA";
                gameLevels[3] = "LevelB";
                break;

            case 1:
                gameLevels[1] = "LevelB";
                gameLevels[3] = "LevelA";
                break;
        }
        // set biofeedback calculation mode in random order:
        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                calculationTypes[0] = CalculationType.Alternative;
                calculationTypes[1] = CalculationType.Conjunction;
                break;

            case 1:
                calculationTypes[0] = CalculationType.Conjunction;
                calculationTypes[1] = CalculationType.Alternative;
                break;
        }
        // setup new analysis data:
        // ...

        // load next scene:
        LoadNextLevel();
    }

    public void LevelHasEnded()
    {
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        // set up current calculation type if needed:
        if (currentLevelID == 3) currentCalculationTypeID++;
        
        // load next scene (or main menu):
        if (currentLevelID < gameLevels.Length)
        {
            SceneManager.LoadScene(gameLevels[currentLevelID]);
            //Debug.Log(gameLevels[currentLevelID] + " scene has been loaded");
            currentLevelID++;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion
    

    #region Private methods
    ///// <summary>
    ///// Performs assertions to make sure everything is properly initialized.
    ///// </summary>
    //private void DoAssertions()
    //{
    //}
    #endregion
}
