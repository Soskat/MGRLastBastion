using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


/// <summary>
/// Component that manages BandBridge UI menu logic.
/// </summary>
public class BandBridgeMenuController : MonoBehaviour {

    #region Private fields
    [SerializeField] private GameObject listView;
    [SerializeField] private Text pairedBandMenuLabel;
    [SerializeField] private InputField hostNameInput;
    [SerializeField] private InputField servicePortInput;
    private ListController listController;
    #endregion


    #region Public properties
    /// <summary>Controller of UI list.</summary>
    public ListController ListController { get { return listController; } }
    /// <summary>Paired Band label.</summary>
    public string PairedBand
    {
        get { return pairedBandMenuLabel.text; }
        set { pairedBandMenuLabel.text = value; }
    }
    /// <summary>Host name input field.</summary>
    public string HostName
    {
        get { return hostNameInput.text; }
        set { hostNameInput.text = value; }
    }
    /// <summary>Service port input field.</summary>
    public string ServicePort
    {
        get { return servicePortInput.text; }
        set { servicePortInput.text = value; }
    }
    #endregion


    #region MonoBehaviour methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Assert.IsNotNull(listView);
        Assert.IsNotNull(pairedBandMenuLabel);
        Assert.IsNotNull(hostNameInput);
        Assert.IsNotNull(servicePortInput);
    }

    // Use this for initialization
    void Start()
    {
        listController = listView.GetComponent<ListController>();
        HostName = GameManager.instance.BBModule.RemoteHostName;
        ServicePort = GameManager.instance.BBModule.RemoteServicePort.ToString();
        PairedBand = GameManager.instance.BBModule.PairedBand.ToString();
    }
    #endregion
}
