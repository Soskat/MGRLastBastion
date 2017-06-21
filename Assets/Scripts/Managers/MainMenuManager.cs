using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    #region Private fields
    [SerializeField] private GameObject gameType;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject bbMenuPanel;
    [SerializeField] private GameObject listViewport;
    private Dropdown gameTypeDropdown;
    private BandBridgeMenuController bbMenuController;
    private ListController listController;
    private bool isSettingsMenuOn = false;
    #endregion
    

    #region MonoBehaviour methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        DoAssertions();
    }

    // Use this for initialization
    void Start()
    {
        bbMenuController = bbMenuPanel.GetComponent<BandBridgeMenuController>();
        gameTypeDropdown = gameType.GetComponent<Dropdown>();
        gameTypeDropdown.AddOptions(new List<string>() { GameType.ModeA.ToString(), GameType.ModeB.ToString() });
        listController = listViewport.GetComponent<ListController>();
        GameManager.instance.ListController = listController;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSettingsMenuOn)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            settingsPanel.SetActive(false);
        }

        // update the list of connected Bands:
        if (GameManager.instance.BBModule.IsConnectedBandsListChanged)
        {
            listController.ClearList();
            listController.UpdateList(GameManager.instance.BBModule.ConnectedBands.ToArray());
            GameManager.instance.BBModule.IsConnectedBandsListChanged = false;
            GameManager.instance.IsReadyForNewBandData = true;
        }

        // update PairedBand label:
        if (GameManager.instance.BBModule.IsPairedBandChanged)
        {
            //sensorPanelController.UpdateBandLabel(GameManager.instance.BBModule.PairedBand.ToString());
            bbMenuController.PairedBand = GameManager.instance.BBModule.PairedBand.ToString();
            GameManager.instance.BBModule.IsPairedBandChanged = false;
        }
    }
    #endregion


    #region Public methods
    /// <summary>
    /// Turn on settings menu panel.
    /// </summary>
    public void TurnOnSettingsMenu()
    {
        isSettingsMenuOn = true;
    }

    /// <summary>
    /// Turn off settings menu panel.
    /// </summary>
    public void TurnOffSettingsMenu()
    {
        isSettingsMenuOn = false;
    }
    #endregion


    #region UI events
    /// <summary>
    /// HostNameInput's <see cref="InputField.onEndEdit"/> behaviour.
    /// </summary>
    public void OnHostNameEndEdit()
    {
        GameManager.instance.BBModule.RemoteHostName = bbMenuController.HostName;
    }

    /// <summary>
    /// ServicePortInput's <see cref="InputField.onEndEdit"/> behaviour.
    /// </summary>
    /// <param name="newServicePort">New service port number</param>
    public void OnServicePortEndEdit()
    {
        int servicePort;
        if (!Int32.TryParse(bbMenuController.ServicePort, out servicePort))
        {
            GameManager.instance.BBModule.RemoteServicePort = BandBridgeModule.DefaultServicePort;
        }
        else
        {
            GameManager.instance.BBModule.RemoteServicePort = servicePort;
        }
    }
    #endregion


    #region Private methods
    /// <summary>
    /// Performs assertions to make sure everything is properly initialized.
    /// </summary>
    private void DoAssertions()
    {
        Assert.IsNotNull(gameType);
        Assert.IsNotNull(settingsPanel);
        Assert.IsNotNull(bbMenuPanel);
        Assert.IsNotNull(listViewport);
    }
    #endregion
}