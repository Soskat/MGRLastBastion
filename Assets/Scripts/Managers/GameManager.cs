using System;
using UnityEngine;
using UnityEngine.Assertions;


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

    
    #region Private fields
    private SensorPanelController sensorPanelController;
    #endregion


    #region Public fields & properties
    public BandBridgeModule BBModule { get; set; }
    public bool IsReadyForNewBandData = false;
    public ListController ListController;
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
