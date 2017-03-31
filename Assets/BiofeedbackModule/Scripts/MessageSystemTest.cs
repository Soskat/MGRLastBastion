using Communication.Client;
using Communication.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using UnityEngine;

public class MessageSystemTest : MonoBehaviour {

    public string HostName = "192.168.0.73";
    //public string HostName = "DESKTOP-KPBRM2V";
    public string ServicePortStr = "2055";
    public string ChoosenBandName;
    public string[] ConnectedBands = new string[] { "Test Fake Band", "Test", "Fake", "Band" };
    public int selectionGridIndex = 0;

    private Vector2 scrollPos;

    private StringBuilder debugInfo = new StringBuilder();
    private string DebugInfo
    {
        get { return debugInfo.ToString(); }
        set { debugInfo.Append("\n" + value); }
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

    



    private void OnGUI()
    {
        #region Server data & other settings        
        GUI.Label(new Rect(10, 10, 90, 20), "Server IP:");
        HostName = GUI.TextField(new Rect(100, 10, 150, 20), HostName);
        GUI.Label(new Rect(10, 35, 90, 20), "Service port:");
        ServicePortStr = GUI.TextField(new Rect(100, 35, 150, 20), ServicePortStr);
        #endregion

        #region Connected Bands settings:
        GUI.Label(new Rect(280, 10, 250, 20), "Connected Bands:");
        scrollPos = GUI.BeginScrollView(new Rect(280, 35, 250, 100), scrollPos, new Rect(0, 0, 230, 300));
            selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, ConnectedBands, 1, GUILayout.ExpandHeight(true), GUILayout.MaxHeight(300), GUILayout.Height(100));
        GUI.EndScrollView();
        GUI.Label(new Rect(280, 145, 90, 20), "Selected Band:");
        ChoosenBandName = GUI.TextField(new Rect(380, 145, 150, 20), ConnectedBands[selectionGridIndex]);
        #endregion

        #region Buttons
        // === test SHOW_ASK message ===========================
        if (GUI.Button(new Rect(10, 75, 250, 30), "[SHOW_ASK][null]"))
        {
            Message message = new Message(Command.SHOW_ASK, null);
            Debug.Log("Prepaired message: " + message);
            Message resp = Test_SendMessageToBandBridge(message);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Log("Received response: " + resp);
                Debug.Assert(resp.Code == Command.SHOW_ANS, "Wrong response Code - expected SHOW_ANS, but get: " + resp.Code);
                Debug.Assert((resp.Result == null) || (resp.Result.GetType() == typeof(List<string>)),
                            "Wrong response Result - expected null or List<string>, but get: " + resp.Result);
            }
        }
        if (GUI.Button(new Rect(10, 105, 250, 30), "[SHOW_ASK][42]"))
        {
            Message message = new Message(Command.SHOW_ASK, 42);
            Debug.Log("Prepaired message: " + message);
            Message resp = Test_SendMessageToBandBridge(message);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Log("Received response: " + resp);
                Debug.Assert(resp.Code == Command.SHOW_ANS, "Wrong response Code - expected SHOW_ANS, but get: " + resp.Code);
                Debug.Assert((resp.Result == null) || (resp.Result.GetType() == typeof(List<string>)),
                            "Wrong response Result - expected null or List<string>, but get: " + resp.Result);


                // if there are bands connected to BandBridge, choose first from the list:
                if (resp.Result != null && resp.Result.GetType() == typeof(List<string>))
                {
                    if (((List<string>)resp.Result).Count > 0)
                    {
                        ChoosenBandName = ((List<string>)resp.Result)[0];
                    }
                }
            }
        }
        // === test GET_DATA_ASK message =======================
        if (GUI.Button(new Rect(10, 145, 250, 30), "[GET_DATA_ASK][null]"))
        {
            Message message = new Message(Command.GET_DATA_ASK, null);
            Debug.Log("Prepaired message: " + message);
            Message resp = Test_SendMessageToBandBridge(message);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Log("Received response: " + resp);
                Debug.Assert(resp.Code == Command.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                Debug.Assert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result);
            }
        }
        if (GUI.Button(new Rect(10, 175, 250, 30), "[GET_DATA_ASK][42]"))
        {
            Message message = new Message(Command.GET_DATA_ASK, 42);
            Debug.Log("Prepaired message: " + message);
            Message resp = Test_SendMessageToBandBridge(message);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Log("Received response: " + resp);
                Debug.Assert(resp.Code == Command.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                Debug.Assert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result);
            }
        }
        if (GUI.Button(new Rect(10, 205, 250, 30), "[GET_DATA_ASK][" + ChoosenBandName + "]"))
        {
            Message message = new Message(Command.GET_DATA_ASK, ChoosenBandName);
            Debug.Log("Prepaired message: " + message);
            Message resp = Test_SendMessageToBandBridge(message);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Log("Received response: " + resp);
                Debug.Assert(resp.Code == Command.GET_DATA_ANS, "Wrong response Code - expected GET_DATA_ANS, but get: " + resp.Code);
                Debug.Assert((resp.Result == null) || (resp.Result.GetType() == typeof(SensorData[])),
                            "Wrong response Result - expected null or typeof(SensorData), but get: " + resp.Result);
            }
        }
        // === test SHOW_ANS, GET_DATA_ANS & CTR_MSG message ===
        if (GUI.Button(new Rect(10, 245, 250, 30), "[SHOW_ANS][null]"))
        {
            Message message = new Message(Command.SHOW_ANS, null);
            Debug.Log("Prepaired message: " + message);
            Message resp = Test_SendMessageToBandBridge(message);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Log("Received response: " + resp);
                Debug.Assert(resp.Code == Command.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                Debug.Assert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result);
            }
        }
        if (GUI.Button(new Rect(10, 275, 250, 30), "[GET_DATA_ANS][null]"))
        {
            Message message = new Message(Command.GET_DATA_ANS, null);
            Debug.Log("Prepaired message: " + message);
            Message resp = Test_SendMessageToBandBridge(message);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Log("Received response: " + resp);
                Debug.Assert(resp.Code == Command.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                Debug.Assert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result);
            }
        }
        if (GUI.Button(new Rect(10, 305, 250, 30), "[CTR_MSG][null]"))
        {
            Message message = new Message(Command.CTR_MSG, null);
            Debug.Log("Prepaired message: " + message);
            Message resp = Test_SendMessageToBandBridge(message);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Log("Received response: " + resp);
                Debug.Assert(resp.Code == Command.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                Debug.Assert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result);
            }
        }
        #endregion

        #region Debug log
        GUI.Label(new Rect(10, 350, 200, 25), "Debug info:");
        GUI.TextArea(new Rect(10, 375, 520, 75), DebugInfo);
        #endregion
    }




    private Message Test_SendMessageToBandBridge(Message message)
    {
        // original source: http://stackoverflow.com/a/34040733

        Message response = null;
        ManualResetEvent sendDone = new ManualResetEvent(false);

        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (s, e) => {
            e.Result = SocketClient.StartClient(HostName, ServicePort, message);
        };
        worker.RunWorkerCompleted += (s, e) => {
            response = (Message)e.Result;
            sendDone.Set();
        };
        worker.RunWorkerAsync();
        sendDone.WaitOne();

        return response;
    }
}
