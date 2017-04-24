using Communication.Data;
using Communication.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;



public class CommunicationManager : MonoBehaviour {

    public GameObject menuPanel;
    public GameObject listView;
    public Text pairedBandMenuLabel;
    public Text pairedBandLabel;
    public Text hrReadingLabel;
    public Text gsrReadingLabel;
    public InputField hostNameInput;
    public InputField servicePortInput;

    private const string defaultHostName = "DESKTOP-KPBRM2V";
    private const int defaultServicePort = 2055;
    [SerializeField] private string remoteHostName;
    [SerializeField] private int remoteServicePort;
    [SerializeField] private StringBuilder pairedBand = new StringBuilder();
    private bool isPairedBandChanged = false;

    private bool isMenuOn = false;
    private int currHrReading = 0;
    private int currGsrReading = 0;
    private bool isSensorsReadingsChanged = false;

    private List<string> connectedBands;
    private bool isConnectedBandsListChanged = false;
    
    public static Action<Message> MessageArrived { get; set; }






    private void Awake()
    {
        // make sure all objects exist:
        DoAssertTests();

        MessageArrived += receivedMsg =>
        {
            DealWithReceivedMessage(receivedMsg);
        };
    }

    // Use this for initialization
    void Start () {
        //// setup remote & local hosts info:
        remoteHostName = defaultHostName;
        remoteServicePort = defaultServicePort;

        // update GUI:
        menuPanel.SetActive(false);
        hostNameInput.text = remoteHostName;
        servicePortInput.text = remoteServicePort.ToString();
        pairedBandLabel.text = "";
        hrReadingLabel.text = "";
        gsrReadingLabel.text = "";

        connectedBands = new List<string>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchMenuState();

        if (isSensorsReadingsChanged)
        {
            hrReadingLabel.text = currHrReading.ToString();
            gsrReadingLabel.text = currGsrReading.ToString();
            isSensorsReadingsChanged = false;
        }

        if (isConnectedBandsListChanged)
        {
            listView.GetComponent<ListController>().ClearList();
            listView.GetComponent<ListController>().UpdateList(connectedBands.ToArray());
            isConnectedBandsListChanged = false;
        }

        if (isPairedBandChanged)
        {
            pairedBandLabel.text = pairedBand.ToString();
            pairedBandMenuLabel.text = pairedBand.ToString();
            isPairedBandChanged = false;
        }
	}
    

    public void GetBandData()
    {
        Message msg = new Message(MessageCode.GET_DATA_ASK, pairedBand.ToString());
        try
        {
            SendMessageToBandBridgeServer(msg);

        } catch (Exception ex) {

            Debug.Log(ex);
        }
    }


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
    /// Saves info about choosen Band and sends to BandBridge server request to pair with it.
    /// </summary>
    public void PairBand()
    {
        // first, unpair Band if needed:
        if(pairedBand != null && pairedBand.ToString() != "")
        {
            UnpairBand();
        }

        pairedBand.Append(listView.GetComponent<ListController>().GetSelectedItem());
        isPairedBandChanged = true;
    }

    /// <summary>
    /// Sends to BandBridge server request to unpair with choosen Band.
    /// </summary>
    public void UnpairBand()
    {
        pairedBand.Remove(0, pairedBand.Length);
        isPairedBandChanged = true;
    }

    /// <summary>
    /// Refresh connection with choosen Band.
    /// </summary>
    public void RefreshPairedBand()
    {
        string choosenBand = pairedBand.ToString();
        foreach(string bandName in connectedBands)
        {
            // choosen Band is still connected:
            if (choosenBand == bandName) return;
        }
        // choosen Band is not connected any more:
        UnpairBand();
    }

    private void UpdatePairedBandGUI(string newText)
    {
        pairedBandLabel.text = newText;
        pairedBandMenuLabel.text = newText;
    }

    /// <summary>
    /// Sends to BandBridge server request to get latest list of connected MS Band devices.
    /// </summary>
    public void RefreshList()
    {
        // create new request:
        Message msg = new Message(MessageCode.SHOW_LIST_ASK, null);

        try
        {
            listView.GetComponent<ListController>().ClearList();
            SendMessageToBandBridgeServer(msg);

        } catch(Exception ex) {
            Debug.Log(ex.ToString());
        }
    }

    #endregion


    #region UI events

    /// <summary>
    /// HostNameInput's <see cref="InputField.onEndEdit"/> behaviour.
    /// </summary>
    /// <param name="newHostName">New host name</param>
    public void OnHostNameEndEdit(string newHostName)
    {
        remoteHostName = newHostName;
    }

    /// <summary>
    /// ServicePortInput's <see cref="InputField.onEndEdit"/> behaviour.
    /// </summary>
    /// <param name="newServicePort">New service port number</param>
    public void OnServicePortEndEdit(string newServicePort)
    {
        if (!Int32.TryParse(newServicePort, out remoteServicePort))
        {
            remoteServicePort = defaultServicePort;
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
    

    /// <summary>
    /// Sends specified message to BandBridge server and returns its response.
    /// </summary>
    /// <param name="msg">Message to send</param>
    /// <returns>Received response</returns>
    private void SendMessageToBandBridgeServer(Message msg)
    {
        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (s, e) =>
        {
            e.Result = SocketClient.StartClient(remoteHostName, remoteServicePort, msg, SocketClient.MaxMessageSize);
        };
        worker.RunWorkerCompleted += (s, e) =>
        {
            Message resp = (Message)e.Result;
            Debug.Log(">> Received response: " + resp);

            MessageArrived(resp);
            Debug.Log("Work is done");
        };
        worker.RunWorkerAsync();
    }


    private void DealWithReceivedMessage(Message msg)
    {
        if (msg == null) return;
        
        switch (msg.Code)
        {
            // refresh list of connected Band devices:
            case MessageCode.SHOW_LIST_ANS:
                if (msg != null && msg.Code == MessageCode.SHOW_LIST_ANS)
                {
                    if (msg.Result.GetType() == typeof(string[]) || msg.Result == null)
                    {
                        // update connected Bands list:
                        connectedBands.Clear();
                        connectedBands.AddRange((string[])msg.Result);
                        isConnectedBandsListChanged = true;
                    }
                }
                RefreshPairedBand();
                break;
            
            // update current sensors readings:
            case MessageCode.GET_DATA_ANS:
                if (msg != null && msg.Code == MessageCode.GET_DATA_ANS && msg.Result.GetType() == typeof(SensorData[]))
                {
                    // update sensors data readings:
                    currHrReading = ((SensorData[])msg.Result)[0].Data;
                    currGsrReading = ((SensorData[])msg.Result)[1].Data;
                    isSensorsReadingsChanged = true;
                }
                break;

            default:
                break;
        }
    }

    #endregion


    #region Debug & test methods
    /// <summary>
    /// Fakes the BandBridge app services behaviour.
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    private Message FakeBandBridgeService(Message msg)
    {
        Thread.Sleep(1000);
        switch (msg.Code)
        {
            case MessageCode.SHOW_LIST_ASK:
                return new Message(MessageCode.SHOW_LIST_ANS, new string[] { "FakeBand 1", "FakeBand 2", "FakeBand 3" });

            case MessageCode.GET_DATA_ASK:
                SensorData hrData = new SensorData(SensorCode.HR, 75);
                SensorData gsrData = new SensorData(SensorCode.HR, 11);
                return new Message(MessageCode.GET_DATA_ANS, new SensorData[] { hrData, gsrData });

            default:
                return new Message(MessageCode.CTR_MSG, null);
        }
    }
    #endregion
}
