using Communication.Data;
using Communication.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using UnityEngine;


public class BandBridgeModule : MonoBehaviour {

    #region Constants
    /// <summary>
    /// Default remote host name.
    /// </summary>
    public const string DefaultHostName = "DESKTOP-KPBRM2V";
    /// <summary>
    /// Default remote host service port.
    /// </summary>
    public const int DefaultServicePort = 2055;
    #endregion

    #region Public fields
    public int RefreshingTime = 5000;
    public string RemoteHostName;
    public int RemoteServicePort;
    public StringBuilder PairedBand;
    public bool IsBandPaired = false;
    public bool IsPairedBandChanged = false;
    public int CurrHrReading = 0;
    public int CurrGsrReading = 0;
    public bool IsSensorsReadingsChanged = false;
    public List<string> ConnectedBands;
    public bool IsConnectedBandsListChanged = false;
    public Action<Message> MessageArrived;
    #endregion

    private BackgroundWorker refresherWorker;

    #region Unity methods
    private void Awake()
    {
        RemoteHostName = DefaultHostName;
        RemoteServicePort = DefaultServicePort;
        MessageArrived += receivedMsg =>
        {
            DealWithReceivedMessage(receivedMsg);
        };
    }

    private void Start()
    {
        PairedBand = new StringBuilder();
        ConnectedBands = new List<string>();

        //// refresh connected Bands list periodically:
        //refresherWorker = new BackgroundWorker();
        //refresherWorker.WorkerSupportsCancellation = true;
        //refresherWorker.DoWork += (s, e) =>
        //{
        //    while (!e.Cancel)
        //    {
        //        Debug.Log("Inside refresherWorker...");
        //        RefreshList();
        //        Thread.Sleep(RefreshingTime);
        //        if (refresherWorker.CancellationPending)
        //        {
        //            e.Cancel = true;
        //        }
        //    }
        //};
        //refresherWorker.RunWorkerCompleted += (s, e) =>
        //{
        //    Debug.Log("RefresherWorker work is done");
        //};
        //refresherWorker.RunWorkerAsync();
    }

    private void OnApplicationQuit()
    {
        if (refresherWorker != null)
        {
            refresherWorker.CancelAsync();
        }
    }
    #endregion
    
    #region Public methods
    /// <summary>
    /// Sends to BandBridge server request to get latest list of connected MS Band devices.
    /// </summary>
    public void RefreshList()
    {
        try
        {
            Message msg = new Message(MessageCode.SHOW_LIST_ASK, null);
            SendMessageToBandBridgeServer(msg);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    /// <summary>
    /// Saves info about choosen Band device name.
    /// </summary>
    public void PairBand()
    {
        // first, unpair Band if needed:
        if (PairedBand != null && PairedBand.ToString() != "")
        {
            UnpairBand();
        }
        // pair with new Band:
        string newChoosenBand = GameManager.gameManager.GetChoosenBandName();
        if (newChoosenBand == null) return;
        PairedBand.Append(newChoosenBand);
        IsBandPaired = true;
        IsPairedBandChanged = true;
    }

    /// <summary>
    /// Removes info about choosen Band device name.
    /// </summary>
    public void UnpairBand()
    {
        PairedBand.Remove(0, PairedBand.Length);
        IsBandPaired = false;
        IsPairedBandChanged = true;
        CurrHrReading = 0;
        CurrGsrReading = 0;
        IsSensorsReadingsChanged = true;
    }

    /// <summary>
    /// Refresh connection with choosen Band.
    /// </summary>
    public void RefreshPairedBand()
    {
        string choosenBand = PairedBand.ToString();
        foreach(string bandName in ConnectedBands)
        {
            // choosen Band is still connected:
            if (choosenBand == bandName) return;
        }
        // choosen Band is not connected any more:
        UnpairBand();
    }
    
    /// <summary>
    /// Gets current sensors readings from paired Band.
    /// </summary>
    public void GetBandData()
    {
        Message msg = new Message(MessageCode.GET_DATA_ASK, PairedBand.ToString());
        try
        {
            SendMessageToBandBridgeServer(msg);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    #endregion

    #region Private methods
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
            e.Result = SocketClient.StartClient(RemoteHostName, RemoteServicePort, msg, SocketClient.MaxMessageSize);
        };
        worker.RunWorkerCompleted += (s, e) =>
        {
            Message resp = (Message)e.Result;
            MessageArrived(resp);
        };
        worker.RunWorkerAsync();
    }

    /// <summary>
    /// Deals with received message response.
    /// </summary>
    /// <param name="msg">Received response</param>
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
                        ConnectedBands.Clear();
                        ConnectedBands.AddRange((string[])msg.Result);
                        IsConnectedBandsListChanged = true;
                    }
                }
                RefreshPairedBand();
                break;
            
            // update current sensors readings:
            case MessageCode.GET_DATA_ANS:
                if (msg != null && msg.Code == MessageCode.GET_DATA_ANS && msg.Result.GetType() == typeof(SensorData[]))
                {
                    // update sensors data readings:
                    CurrHrReading = ((SensorData[])msg.Result)[0].Data;
                    CurrGsrReading = ((SensorData[])msg.Result)[1].Data;
                    IsSensorsReadingsChanged = true;
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
