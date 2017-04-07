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
    public InputField hostNameInput;
    public InputField servicePortInput;

    private string defaultHostName = "DESKTOP-KPBRM2V";
    private int defaultServicePort = 2055;
    public string HostName;
    public int ServicePort;


    public string ChoosenBand = "";
    public GameObject contentPanel;
    public GameObject itemPrefab;
    public List<string> ConnectedBands;

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
        ConnectedBands = new List<string>();

        ListTest();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchMenuState();
	}


    public void ChooseBand()
    {

    }

    public void RefreshList()
    {
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
                if(resp.Result != null && resp.Result.GetType() == typeof(string[]) && ((string[])resp.Result).Length > 0)
                {
                    foreach(string bandName in (string[])resp.Result)
                    {
                        ConnectedBands.Add(bandName);
                        GameObject newItem = Instantiate(itemPrefab) as GameObject;
                        newItem.GetComponent<Text>().text = bandName;
                        newItem.transform.SetParent(contentPanel.transform);
                    }
                }
            }
            Debug.Log("End of work");
        };
        worker.RunWorkerAsync();
    }



    private void ListTest()
    {
        ConnectedBands.Add("buka");
        ConnectedBands.Add("jest");
        ConnectedBands.Add("fioletowa");
        foreach(var item in ConnectedBands)
        {
            GameObject newItem = Instantiate(itemPrefab) as GameObject;
            newItem.GetComponent<Text>().text = item;
            newItem.transform.SetParent(contentPanel.transform);
        }
    }


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
    /// Performs assertions to make sure everything is properly initialized.
    /// </summary>
    private void DoAssertTests()
    {
        Assert.IsNotNull(menuPanel);
        Assert.IsNotNull(hostNameInput);
        Assert.IsNotNull(servicePortInput);
        Assert.IsNotNull(contentPanel);
        Assert.IsNotNull(itemPrefab);
    }
}
