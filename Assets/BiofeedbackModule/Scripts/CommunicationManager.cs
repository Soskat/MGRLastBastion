using Communication.Data;
using Communication.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
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
    //private const int defaultOpenPort = 2065;
    //private const int defaultBacklog = 10;
    [SerializeField] private string remoteHostName;
    [SerializeField] private int remoteServicePort;
    //[SerializeField] private string localHostName;
    //[SerializeField] private int localOpenPort;
    //[SerializeField] private int localBacklog;
    [SerializeField] private StringBuilder pairedBand = new StringBuilder();
    //[SerializeField] private string pairedBand = "";

    private bool isMenuOn = false;
    //private Thread serverThread;
    private int currHrReading = 0;
    private int currGsrReading = 0;



    private void Awake()
    {
        // make sure all objects exist:
        DoAssertTests();
    }

    // Use this for initialization
    void Start () {
        //// setup remote & local hosts info:
        remoteHostName = defaultHostName;
        remoteServicePort = defaultServicePort;
        //localHostName = Dns.GetHostName();
        //localOpenPort = defaultOpenPort;
        //localBacklog = defaultBacklog;

        // update GUI:
        menuPanel.SetActive(false);
        hostNameInput.text = remoteHostName;
        servicePortInput.text = remoteServicePort.ToString();
        pairedBandLabel.text = "";
        hrReadingLabel.text = "";
        gsrReadingLabel.text = "";
        //ConnectedBands = new List<string>();
        ListTest();

        //// setup server for incoming paired Band's sensors readings:
        //serverThread = new Thread(new ThreadStart(BandDataServerService));

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchMenuState();
	}

    //private void OnApplicationQuit()
    //{
    //    SocketServer.EnableWorking = false;
    //    serverThread.Join();
    //}


    public void GetBandData()
    {
        Message resp = SendMessageToBandBridgeServer(new Message(MessageCode.GET_DATA_ASK, pairedBand.ToString()));
        if(resp != null && resp.Result.GetType() == typeof(SensorData[]))
        {
            // update sensors data:
            UpdateSensorReading(((SensorData[])resp.Result)[0]);
            UpdateSensorReading(((SensorData[])resp.Result)[1]);
        }
        Debug.Log("Got sensors data: " + resp);
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
        UpdatePairedBandGUI(pairedBand.ToString());

        //string newPairedBand = listView.GetComponent<ListController>().GetSelectedItem();
        //pairedBandMenuLabel.text = pairedBand;

        //// create new message:
        //PairRequest request = new PairRequest(localHostName, localOpenPort, newPairedBand);
        //Message msg = new Message(MessageCode.PAIR_BAND_ASK, request);
        //try
        //{
        //    //Debug.Log("______________Try to send PairRequest to BBserver");
        //    Message resp = SendMessageToBandBridgeServer(msg);
        //    //Debug.Log("______________Got resp: " + resp);
        //    if (resp != null && resp.Code == MessageCode.PAIR_BAND_ANS)
        //    {
        //        // choosen Band was paired succesfully:
        //        if ((bool)resp.Result)
        //        {
        //            StartListeningForBandData();
        //            pairedBand = newPairedBand;
        //            UpdatePairedBandGUI(pairedBand);
        //        }
        //    }
        //} catch(Exception ex) {
        //    Debug.Log(ex.ToString());
        //}
    }

    /// <summary>
    /// Sends to BandBridge server request to unpair with choosen Band.
    /// </summary>
    public void UnpairBand()
    {
        pairedBand.Remove(0, pairedBand.Length);
        //Message msg = new Message(MessageCode.FREE_BAND_ASK, pairedBand);
        //try
        //{
        //    // send request to BandBridge server:
        //    SendMessageToBandBridgeServer(msg);
        //    // update GUI:
        //    pairedBand = "";
        //    UpdatePairedBandGUI(pairedBand);
        //    hrReadingLabel.text = "";
        //    gsrReadingLabel.text = "";
        //    // stop listening for incoming sensors readings data:
        //    StopListeningForBandData();
        //}
        //catch (Exception ex) {
        //    Debug.Log(ex.ToString());
        //}
    }

    /// <summary>
    /// Refresh connection with choosen Band.
    /// </summary>
    public void RefreshPairedBand()
    {
        string choosenBand = pairedBand.ToString();
        foreach(string bandName in listView.GetComponent<ListController>().connectedBands)
        {
            // choosen Band is still connected:
            if (choosenBand == bandName) return;
        }
        // choosen Band is not connected any more:
        UnpairBand();
        //pairedBand = "";
        UpdatePairedBandGUI(pairedBand.ToString());
        //StopListeningForBandData();
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
            Message resp = SendMessageToBandBridgeServer(msg);

            if (resp != null && resp.Code == MessageCode.SHOW_LIST_ANS)
            {
                if (resp.Result.GetType() == typeof(string[]) || resp.Result == null)
                {
                    listView.GetComponent<ListController>().UpdateList((string[])resp.Result);
                }
            }
            RefreshPairedBand();

        } catch(Exception ex) {
            Debug.Log(ex.ToString());
        }
    }

    public void UpdateSensorReading(SensorData sd)
    {
        // update HR:
        if (sd.Code == SensorCode.HR)
        {
            currHrReading = sd.Data;
            // update GUI:
            hrReadingLabel.text = currHrReading.ToString();
        }
        // update GSR:
        else
        {
            currGsrReading = sd.Data;
            // update GUI:
            gsrReadingLabel.text = currGsrReading.ToString();
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

    //private void BandDataServerService()
    //{
    //    SocketServer.EnableWorking = true;
    //    SocketServer.NewReadingArrived += newReading =>
    //    {
    //        Debug.Log("Got new reading!: " + newReading);
    //        UpdateSensorReading(newReading);
    //    };
    //    SocketServer.StartListening(localOpenPort, localBacklog, SocketClient.MaxMessageSize);
    //    Debug.Log("_=_=__SERVER THREAD STARTED");
    //}

    //public void StartListeningForBandData()
    //{
    //    serverThread.Start();
    //}

    //public void StopListeningForBandData()
    //{
    //    //SocketServer.EnableWorking = false;
    //    //Debug.Log("_=_=__SERVER THREAD STOPPED");
    //}

    /// <summary>
    /// Sends specified message to BandBridge server and returns its response.
    /// </summary>
    /// <param name="msg">Message to send</param>
    /// <returns>Received response</returns>
    private Message SendMessageToBandBridgeServer(Message msg)
    {
        return SocketClient.StartClient(remoteHostName, remoteServicePort, msg, SocketClient.MaxMessageSize);
    }

    #endregion


    #region Debug & test methods

    private void ListTest()
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < UnityEngine.Random.Range(3, 7); i++)
        {
            temp.Add("miau" + i);
        }
        listView.GetComponent<ListController>().UpdateList(temp.ToArray());
    }

    #endregion

    #region BACKUP -- BACKUP -- BACKUP

    //public void RefreshList_OLD_BackgroundWorker()
    //{
    //    Debug.Log("___________Connect to BandBridge to refresh Bands list...");
    //    // create new request:
    //    Message msg = new Message(MessageCode.SHOW_LIST_ASK, null);
    //    // start background work:
    //    BackgroundWorker worker = new BackgroundWorker();
    //    worker.WorkerReportsProgress = true;
    //    worker.WorkerSupportsCancellation = true;   // ---------- ??????

    //    worker.ProgressChanged += (s, e) =>
    //    {
    //        Debug.Log(e.ProgressPercentage + "%");

    //        if (e.UserState != null)
    //        {
    //            Debug.Log("___________Buka 2.5: " + (string[])e.UserState);
    //            listView.GetComponent<ListController>().UpdateList((string[])e.UserState);
    //            Debug.Log("___________List view was updated");
    //        }
    //        else
    //        {
    //            Debug.Log("Buka a nie Bands list");
    //        }
    //    };

    //    worker.DoWork += (s, e) =>
    //    {
    //        try
    //        {
    //            e.Result = SocketClient.StartClient(remoteHostName, remoteServicePort, msg, SocketClient.MaxMessageSize);
    //            Debug.Log("___________" + e.Result);


    //            Message resp = (Message)e.Result;

    //            if (resp != null && resp.Code == MessageCode.SHOW_LIST_ANS)
    //            {
    //                Debug.Log("___________Buka 1");
    //                if (resp.Result.GetType() == typeof(string[]) || resp.Result == null)
    //                {
    //                    Debug.Log("___________Buka 2");
    //                    //(s as BackgroundWorker).ReportProgress(0);
    //                    //(s as BackgroundWorker).ReportProgress(0, resp.Result);
    //                    lock (listView)
    //                    {
    //                        listView.GetComponent<ListController>().UpdateList((string[])resp.Result);
    //                    }
    //                    Debug.Log("___________Buka 3");
    //                }
    //            }
    //            Debug.Log("___________End of work");
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.Log("___________" + ex.Message);
    //            e.Result = null;
    //            worker.CancelAsync();
    //        }
    //    };


    //    //worker.RunWorkerCompleted += (s, e) =>
    //    //{
    //    //    Debug.Log("___________Work completed");
    //    //    Message resp = (Message)e.Result;

    //    //    // little hack here - wait until whole response from server come
    //    //    //while (resp == null) { Debug.Log(resp); }


    //    //    Debug.Log("___________Got response: " + resp);

    //    //    if (resp != null && resp.Code == MessageCode.SHOW_LIST_ANS)
    //    //    {
    //    //        Debug.Log("___________Buka 1");
    //    //        if (resp.Result.GetType() == typeof(string[]) || resp.Result == null)
    //    //        {
    //    //            Debug.Log("___________Buka 2");
    //    //            (s as BackgroundWorker).ReportProgress(0);
    //    //            //(s as BackgroundWorker).ReportProgress(0, resp.Result);
    //    //            Debug.Log("___________Buka 3");
    //    //        }
    //    //    }
    //    //    Debug.Log("___________End of work");
    //    //};

    //    listView.GetComponent<ListController>().ClearList();
    //    worker.RunWorkerAsync();
    //}

    #endregion
}
