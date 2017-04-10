using Communication.Sockets;
using Communication.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using UnityEngine;


public enum DataStatus { decrease, increase, steady, unknown }


public class MessageSystemTest : MonoBehaviour {

    private const string defaultNone = "<none>";

    public string HostName = "192.168.0.73";
    //public string remoteHostName = "DESKTOP-KPBRM2V";
    public string ServicePortStr = "2055";
    public string ChoosenBandName;
    public string[] ConnectedBands = new string[] { "Test Fake Band", "Test", "Fake", "Band" };
    public int selectionGridIndex = 0;

    private Vector2 scrollPosList;
    private Vector2 scrollPosLog;

    private int debugInfoLines = 0;
    private StringBuilder debugInfo = new StringBuilder();
    private string DebugInfo
    {
        get { return debugInfo.ToString(); }
        set
        {
            debugInfo.AppendLine(value);
            debugInfoLines += 1;
        }
    }

    private int servicePort = 2055;
    private int ServicePort
    {
        get
        {
            if (Int32.TryParse(ServicePortStr, out servicePort))
            {
                return servicePort;
            }
            else return 2055;
        }
    }

    
    private int currHrReading;
    private int oldHrReading;
    private DataStatus statusHr = DataStatus.unknown;
    private int currGsrReading;
    private int oldGsrReading;
    private DataStatus statusGsr = DataStatus.unknown;

    int maxMessageSize = 2048;

    private void OnGUI()
    {
        #region Server data & other settings
        GUILayout.BeginArea(new Rect(10, 10, 250, 50));
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Server IP:", GUILayout.Width(90));
                HostName = GUILayout.TextField(HostName, GUILayout.Width(150));
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Service port:", GUILayout.Width(90));
                ServicePortStr = GUILayout.TextField(ServicePortStr, GUILayout.Width(150));
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
        #endregion

        #region Buttons
        GUILayout.BeginArea(new Rect(10, 75, 250, 335));
        {
            // === test SHOW_LIST_ASK message =======================================================================================
            GUILayout.BeginVertical();
            {
                if (GUILayout.Button("[SHOW_LIST_ASK][null]", GUILayout.Width(250), GUILayout.Height(25)))
                {
                    Message message = new Message(MessageCode.SHOW_LIST_ASK, null);
                    ShowDebugLog("Prepaired message: " + message);
                    Message resp = SendMessageToBandBridge(message);
                    DoAssert(resp != null, "Response is null!");
                    if (resp != null)
                    {
                        ShowDebugLog("Received response: " + resp);
                        DoAssert(resp.Code == MessageCode.SHOW_LIST_ANS, "Wrong response Code - expected SHOW_LIST_ANS, but get: " + resp.Code);
                        DoAssert((resp.Result == null) || (resp.Result.GetType() == typeof(string[])),
                                    "Wrong response Result - expected null or string[], but get: " + resp.Result);

                        // if respone Result was correct, update ConnectedBands list:
                        if (resp.Result == null || resp.Result.GetType() == typeof(string[]))
                        {
                            SetConnectedBandsList((string[])resp.Result);
                        }
                    }
                    ShowDebugLog("-------------------------------");
                }
                if (GUILayout.Button("[SHOW_LIST_ASK][42]", GUILayout.Width(250), GUILayout.Height(25)))
                {
                    Message message = new Message(MessageCode.SHOW_LIST_ASK, 42);
                    ShowDebugLog("Prepaired message: " + message);
                    Message resp = SendMessageToBandBridge(message);
                    DoAssert(resp != null, "Response is null!");
                    if (resp != null)
                    {
                        ShowDebugLog("Received response: " + resp);
                        DoAssert(resp.Code == MessageCode.SHOW_LIST_ANS, "Wrong response Code - expected SHOW_LIST_ANS, but get: " + resp.Code);
                        DoAssert((resp.Result == null) || (resp.Result.GetType() == typeof(string[])),
                                    "Wrong response Result - expected null or string[], but get: " + resp.Result);

                        // if respone Result was correct, update ConnectedBands list:
                        if (resp.Result == null || resp.Result.GetType() == typeof(string[]))
                        {
                            SetConnectedBandsList((string[])resp.Result);
                        }
                    }
                    ShowDebugLog("-------------------------------");
                }
            }
            GUILayout.EndVertical();
            GUILayout.Space(10);
            // === test GET_DATA_ASK message ===================================================================================
            GUILayout.BeginVertical();
            {
                if (GUILayout.Button("[GET_DATA_ASK][null]", GUILayout.Width(250), GUILayout.Height(25)))
                {
                    Message message = new Message(MessageCode.GET_DATA_ASK, null);
                    ShowDebugLog("Prepaired message: " + message);
                    Message resp = SendMessageToBandBridge(message);
                    DoAssert(resp != null, "Response is null!");
                    if (resp != null)
                    {
                        ShowDebugLog("Received response: " + resp);
                        DoAssert(resp.Code == MessageCode.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                        DoAssert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result);
                    }
                    ShowDebugLog("-------------------------------");
                }
                if (GUILayout.Button("[GET_DATA_ASK][42]", GUILayout.Width(250), GUILayout.Height(25)))
                {
                    Message message = new Message(MessageCode.GET_DATA_ASK, 42);
                    ShowDebugLog("Prepaired message: " + message);
                    Message resp = SendMessageToBandBridge(message);
                    DoAssert(resp != null, "Response is null!");
                    if (resp != null)
                    {
                        ShowDebugLog("Received response: " + resp);
                        DoAssert(resp.Code == MessageCode.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                        DoAssert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result);
                    }
                    ShowDebugLog("-------------------------------");
                }
                if (GUILayout.Button("[GET_DATA_ASK][" + ChoosenBandName + "]", GUILayout.Width(250), GUILayout.Height(25)))
                {
                    Message message = new Message(MessageCode.GET_DATA_ASK, ChoosenBandName);
                    ShowDebugLog("Prepaired message: " + message);
                    Message resp = SendMessageToBandBridge(message);
                    DoAssert(resp != null, "Response is null!");
                    if (resp != null)
                    {
                        ShowDebugLog("Received response: " + resp);
                        DoAssert(resp.Code == MessageCode.GET_DATA_ANS, "Wrong response Code - expected GET_DATA_ANS, but get: " + resp.Code);
                        DoAssert((resp.Result == null) || (resp.Result.GetType() == typeof(SensorData[])),
                                    "Wrong response Result - expected null or typeof(SensorData), but get: " + resp.Result);

                        // show data in GUI:
                        if (resp.Result != null)
                        {
                            SensorData[] sensorDataArray = (SensorData[])resp.Result;
                            foreach(SensorData data in sensorDataArray)
                            {
                                if (data.Code == SensorCode.HR)
                                {
                                    oldHrReading = currHrReading;
                                    currHrReading = data.Data;
                                    statusHr = UpdateDataStatus(oldHrReading, currHrReading);
                                }
                                else if (data.Code == SensorCode.GSR)
                                {
                                    oldGsrReading = currGsrReading;
                                    currGsrReading = data.Data;
                                    statusGsr = UpdateDataStatus(oldGsrReading, currGsrReading);
                                }
                            }
                        }
                    }
                    ShowDebugLog("-------------------------------");
                }
            }
            GUILayout.EndVertical();
            GUILayout.Space(10);
            // === test SHOW_LIST_ANS, GET_DATA_ANS & CTR_MSG message ===============================================================
            GUILayout.BeginVertical();
            {
                if (GUILayout.Button("[SHOW_LIST_ANS][null]", GUILayout.Width(250), GUILayout.Height(25)))
                {
                    Message message = new Message(MessageCode.SHOW_LIST_ANS, null);
                    ShowDebugLog("Prepaired message: " + message);
                    Message resp = SendMessageToBandBridge(message);
                    DoAssert(resp != null, "Response is null!");
                    if (resp != null)
                    {
                        ShowDebugLog("Received response: " + resp);
                        DoAssert(resp.Code == MessageCode.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                        DoAssert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result);
                    }
                    ShowDebugLog("-------------------------------");
                }
                if (GUILayout.Button("[GET_DATA_ANS][null]", GUILayout.Width(250), GUILayout.Height(25)))
                {
                    Message message = new Message(MessageCode.GET_DATA_ANS, null);
                    ShowDebugLog("Prepaired message: " + message);
                    Message resp = SendMessageToBandBridge(message);
                    DoAssert(resp != null, "Response is null!");
                    if (resp != null)
                    {
                        ShowDebugLog("Received response: " + resp);
                        DoAssert(resp.Code == MessageCode.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                        DoAssert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result);
                    }
                    ShowDebugLog("-------------------------------");
                }
                if (GUILayout.Button("[CTR_MSG][null]", GUILayout.Width(250), GUILayout.Height(25)))
                {
                    Message message = new Message(MessageCode.CTR_MSG, null);
                    ShowDebugLog("Prepaired message: " + message);
                    Message resp = SendMessageToBandBridge(message);
                    DoAssert(resp != null, "Response is null!");
                    if (resp != null)
                    {
                        ShowDebugLog("Received response: " + resp);
                        DoAssert(resp.Code == MessageCode.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                        DoAssert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result);
                    }
                    ShowDebugLog("-------------------------------");
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
        #endregion

        #region Connected Bands settings:
        GUILayout.BeginArea(new Rect(280, 10, 250, 350));
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Connected Bands:", GUILayout.Width(250));
                scrollPosList = GUILayout.BeginScrollView(scrollPosList, GUILayout.Width(250), GUILayout.Height(100));
                {
                    selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, ConnectedBands, 1,
                                                                 GUILayout.ExpandHeight(true),
                                                                 GUILayout.MaxHeight(300),
                                                                 GUILayout.Height(100));
                }
                GUILayout.EndScrollView();
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Selected Band:", GUILayout.Width(90));
                    if (ConnectedBands.Length > 0) ChoosenBandName = ConnectedBands[selectionGridIndex];
                    else ChoosenBandName = defaultNone;
                    GUILayout.TextField(ChoosenBandName, GUILayout.Width(150));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
        #endregion

        #region Biofeedback info
        GUILayout.BeginArea(new Rect(300, 220, 200, 200));
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("HR:", GUILayout.Width(35));
                GUILayout.TextField(currHrReading.ToString(), GUILayout.Width(75));
                GUILayout.TextField(statusHr.ToString(), GUILayout.Width(75));
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("GSR:", GUILayout.Width(35));
                GUILayout.TextField(currGsrReading.ToString(), GUILayout.Width(75));
                GUILayout.TextField(statusGsr.ToString(), GUILayout.Width(75));
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
        #endregion

        #region Debug log
        GUILayout.BeginArea(new Rect(10, 350, 520, 150));
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Debug info:");
                scrollPosLog = GUILayout.BeginScrollView(scrollPosLog, GUILayout.Height(120));
                {
                    GUILayout.TextArea(DebugInfo, GUILayout.MinHeight(80), GUILayout.Height(debugInfoLines * 15));
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
        #endregion
    }




    private Message SendMessageToBandBridge(Message message)
    {
        // original source: http://stackoverflow.com/a/34040733

        Message response = null;
        ManualResetEvent sendDone = new ManualResetEvent(false);    // ------------------------------- to remove

        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (s, e) => {
            e.Result = SocketClient.StartClient(HostName, ServicePort, message, maxMessageSize);
        };
        worker.RunWorkerCompleted += (s, e) => {
            response = (Message)e.Result;
            sendDone.Set();    // ------------------------------- to remove
        };
        worker.RunWorkerAsync();
        sendDone.WaitOne();    // ------------------------------- to remove

        return response;
    }


    private DataStatus UpdateDataStatus(int oldReading, int currReading)
    {
        if (oldReading > currReading)
            return DataStatus.decrease;
        else if (oldReading == currReading)
            return DataStatus.steady;
        else
            return DataStatus.increase;
    }

    private void ResetBandData()
    {
        oldHrReading = currHrReading = 0;
        statusHr = DataStatus.unknown;
        oldGsrReading = currGsrReading = 0;
        statusGsr = DataStatus.unknown;
    }

    private void SetConnectedBandsList(string[] newList)
    {
        ConnectedBands = newList;
        if (newList == null)
        {
            ResetBandData();
        }
    }


    private void ShowDebugLog(string message)
    {
        Debug.Log(message);
        DebugInfo = message;
    }

    private void DoAssert(bool condition, string message)
    {
        if (!condition)
        {
            Debug.Log(message);
            DebugInfo = message;
        }
    }
}
