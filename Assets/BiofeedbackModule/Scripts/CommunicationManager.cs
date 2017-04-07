using Communication.Data;
using Communication.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;



public class CommunicationManager : MonoBehaviour {

    public GameObject menuPanel;
    public GameObject listView;
    public Text choosenBandLabel;
    public InputField hostNameInput;
    public InputField servicePortInput;

    private string defaultHostName = "DESKTOP-KPBRM2V";
    private int defaultServicePort = 2055;
    [SerializeField] private string HostName;
    [SerializeField] private int ServicePort;
    [SerializeField] private string ChoosenBand = "";
    private bool isMenuOn = false;



    private void Awake()
    {
        // make sure all objects exist:
        DoAssertTests();
    }

    // Use this for initialization
    void Start () {
        HostName = defaultHostName;
        ServicePort = defaultServicePort;
        menuPanel.SetActive(false);
        hostNameInput.text = HostName;
        servicePortInput.text = ServicePort.ToString();
        //ConnectedBands = new List<string>();

        ListTest();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchMenuState();
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
    public void ChooseBand()
    {
        ChoosenBand = listView.GetComponent<ListController>().GetSelectedItem();
        choosenBandLabel.text = ChoosenBand;
    }

    /// <summary>
    /// Sends to BandBridge server request to get latest list of connected MS Band devices.
    /// </summary>
    public void RefreshList()
    {
        //ListTest();
        //return;

        Debug.Log("Connect to BandBridge to refresh Bands list...");
        // create new request:
        Message msg = new Message(MessageCode.SHOW_ASK, null);
        // start background work:
        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (s, e) =>
        {
            try
            {
                e.Result = SocketClient.StartClient(HostName, ServicePort, msg, SocketClient.MaxMessageSize);
            }
            catch (Exception ex)
            {
                e.Result = null;
                worker.CancelAsync();
            }
        };
        worker.RunWorkerCompleted += (s, e) =>
        {
            Message resp = (Message)e.Result;
            while (resp == null) ; // little hack here - wait until whole response from server come

            if (resp != null && resp.Code == MessageCode.SHOW_ANS)
            {
                if (resp.Result.GetType() == typeof(string[]) || resp.Result == null)
                {
                    listView.GetComponent<ListController>().UpdateList((string[])resp.Result);
                }
            }
            Debug.Log("End of work");
        };

        listView.GetComponent<ListController>().ClearList();
        worker.RunWorkerAsync();
    }

    #endregion


    #region UI events

    /// <summary>
    /// HostNameInput's <see cref="InputField.onEndEdit"/> behaviour.
    /// </summary>
    /// <param name="newHostName">New host name</param>
    public void OnHostNameEndEdit(string newHostName)
    {
        HostName = newHostName;
    }

    /// <summary>
    /// ServicePortInput's <see cref="InputField.onEndEdit"/> behaviour.
    /// </summary>
    /// <param name="newServicePort">New service port number</param>
    public void OnServicePortEndEdit(string newServicePort)
    {
        if (!Int32.TryParse(newServicePort, out ServicePort))
        {
            ServicePort = defaultServicePort;
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
        Assert.IsNotNull(hostNameInput);
        Assert.IsNotNull(servicePortInput);
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
}
