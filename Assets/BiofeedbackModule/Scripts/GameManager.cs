using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region Public fields
    public GameObject menuPanel;
    public GameObject listView;
    public Text pairedBandMenuLabel;
    public Text pairedBandLabel;
    public Text hrReadingLabel;
    public Text gsrReadingLabel;
    public InputField hostNameInput;
    public InputField servicePortInput;

    /// <summary>
    /// <see cref="GameManager"/> public static object.
    /// </summary>
    public static GameManager gameManager;
    #endregion

    #region Private fields
    private BandBridgeModule bbModule;
    private ListController listController;
    private bool isMenuOn = false;
    #endregion


    public bool isReadyForNewBandData = false;



    #region Unity methods
    private void Awake()
    {
        // make sure that Singleton design pattern is preserved and GameManager object will always exist:
        if (gameManager == null)
        {
            gameManager = this;
            // make sure all objects exist:
            DoAssertTests();

            bbModule = gameObject.GetComponent<BandBridgeModule>();
            listController = listView.GetComponent<ListController>();
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // update GUI:
        menuPanel.SetActive(false);
        hostNameInput.text = bbModule.RemoteHostName;
        servicePortInput.text = bbModule.RemoteServicePort.ToString();
        pairedBandLabel.text = "-";
        hrReadingLabel.text = "-";
        gsrReadingLabel.text = "-";
    }


    void Update()
    {
        // get current Band sensors readings:
        if (bbModule.IsBandPaired && isReadyForNewBandData)
        {
            bbModule.GetBandData();
            isReadyForNewBandData = false;
        }

        // show biofeedback module menu if needed:
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchMenuState();
        }

        // update GUI if needed:
        if (bbModule.IsSensorsReadingsChanged)
        {
            if (bbModule.IsBandPaired)
            {
                hrReadingLabel.text = bbModule.CurrHrReading.ToString();
                gsrReadingLabel.text = bbModule.CurrGsrReading.ToString();
            }
            else
            {
                hrReadingLabel.text = "-";
                gsrReadingLabel.text = "-";
            }
            bbModule.IsSensorsReadingsChanged = false;
            isReadyForNewBandData = true;
        }

        if (bbModule.IsConnectedBandsListChanged)
        {
            listController.ClearList();
            listController.UpdateList(bbModule.ConnectedBands.ToArray());
            bbModule.IsConnectedBandsListChanged = false;
            isReadyForNewBandData = true;
        }

        if (bbModule.IsPairedBandChanged)
        {
            pairedBandLabel.text = bbModule.PairedBand.ToString();
            pairedBandMenuLabel.text = bbModule.PairedBand.ToString();
            bbModule.IsPairedBandChanged = false;
        }
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Turns BandBridge menu on and off.
    /// </summary>
    public void SwitchMenuState()
    {
        isMenuOn = !isMenuOn;
        if (isMenuOn)
            menuPanel.SetActive(true);
        else
            menuPanel.SetActive(false);
    }
    
    /// <summary>
    /// Gets currently selected item on connected Bands list.
    /// </summary>
    /// <returns>Currently selected Band name</returns>
    public string GetChoosenBandName()
    {
        return listController.GetSelectedItem();
    }
    #endregion


    #region UI events
    /// <summary>
    /// HostNameInput's <see cref="InputField.onEndEdit"/> behaviour.
    /// </summary>
    public void OnHostNameEndEdit()
    {
        bbModule.RemoteHostName = hostNameInput.text;
    }

    /// <summary>
    /// ServicePortInput's <see cref="InputField.onEndEdit"/> behaviour.
    /// </summary>
    /// <param name="newServicePort">New service port number</param>
    public void OnServicePortEndEdit()
    {
        int servicePort;
        if (!Int32.TryParse(servicePortInput.text, out servicePort))
        {
            bbModule.RemoteServicePort = BandBridgeModule.DefaultServicePort;
        }
        else
        {
            bbModule.RemoteServicePort = servicePort;
        }
    }
    #endregion
    
    #region Private methods
    /// <summary>
    /// Performs assertions to make sure everything is properly initialized.
    /// </summary>
    private void DoAssertTests()
    {
        Assert.IsNotNull(menuPanel);
        Assert.IsNotNull(listView);
        Assert.IsNotNull(pairedBandMenuLabel);
        Assert.IsNotNull(pairedBandLabel);
        Assert.IsNotNull(hrReadingLabel);
        Assert.IsNotNull(gsrReadingLabel);
        Assert.IsNotNull(hostNameInput);
        Assert.IsNotNull(servicePortInput);
    }    
    #endregion    
}
