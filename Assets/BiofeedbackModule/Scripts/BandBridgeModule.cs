﻿using Communication.Data;
using Communication.Sockets;
using LastBastion.Analytics;
using LastBastion.Game.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;


namespace LastBastion.Biofeedback
{
    /// <summary>
    /// Class that manages connection with BandBridge server.
    /// </summary>
    public class BandBridgeModule : MonoBehaviour
    {
        #region Constants
        /// <summary>Default remote host name.</summary>
        public const string DefaultHostName = "DESKTOP-KPBRM2V";
        /// <summary>Default remote host service port.</summary>
        public const int DefaultServicePort = 2055;
        #endregion


        #region Private fields
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private string remoteHostName;
        [SerializeField] private int remoteServicePort;
        [SerializeField] private TripleTreshold hrLevel;
        [SerializeField] private TripleTreshold gsrLevel;
        //[SerializeField] private int refreshingTime = 5000;
        private bool isBandPaired = false;
        private bool isCalibrationOn = false;
        private bool canReceiveBandReadings = false;
        private int averageHr = 1;
        private int averageGsr = 1;
        private int currentHr = 1;
        private int currentGsr = 1;
        [SerializeField] private float hrModifier = 1.0f;
        private DataState hrState;
        [SerializeField] private float gsrModifier = 1.0f;
        private DataState gsrState;
        [SerializeField] private float arousalModifier = 1.0f;
        private DataState arousalState;
        private List<string> connectedBands;
        private BackgroundWorker refresherWorker;
        #endregion
        

        #region Public fields & properties
        /// <summary>Is module enabled (taht means isEnabled ang isBandPaired flags are set to true)?</summary>
        public bool IsEnabled { get { return isEnabled && isBandPaired; } }
        /// <summary>Name of the remote host.</summary>
        public string RemoteHostName
        {
            get { return remoteHostName; }
            set { remoteHostName = value; }
        }
        /// <summary>Port number of the remote host.</summary>
        public int RemoteServicePort
        {
            get { return remoteServicePort; }
            set { remoteServicePort = value; }
        }
        /// <summary>Name of the connected MS Band device.</summary>
        public StringBuilder PairedBand;
        /// <summary>Informs that new message has arrived.</summary>
        public Action<Message> MessageArrived;
        /// <summary>Is MS Band device connected?</summary>
        public bool IsBandPaired { get { return isBandPaired; } }
        /// <summary>Is calibration on?</summary>
        public bool IsCalibrationOn { get { return isCalibrationOn; } }
        /// <summary>Can receive MS Band device current readings?</summary>
        public bool CanReceiveBandReadings { get { return canReceiveBandReadings; } }
        /// <summary>Is paired MS Band device changed?</summary>
        public bool IsPairedBandChanged { get; set; }
        /// <summary>Is average values of sensors readings changed?</summary>
        public bool IsAverageReadingsChanged { get; set; }
        /// <summary>Is current values of sensors readings changed?</summary>
        public bool IsSensorsReadingsChanged { get; set; }
        /// <summary>Is list of connected MS Band devices changed?</summary>
        public bool IsConnectedBandsListChanged { get; set; }
        /// <summary>Average HR value.</summary>
        public int AverageHr { get { return averageHr; } }
        /// <summary>Average GSR value.</summary>
        public int AverageGsr { get { return averageGsr; } }
        /// <summary>Current HR value.</summary>
        public int CurrentHr { get { return currentHr; } }
        /// <summary>Current GSR value.</summary>
        public int CurrentGsr { get { return currentGsr; } }
        /// <summary>Current arousal modifier.</summary>
        public float ArousalModifier { get { return arousalModifier; } }
        /// <summary>Current arousal state.</summary>
        public DataState ArousalState { get { return arousalState; } }
        /// <summary>Current HR modifier.</summary>
        public float HrModifier { get { return hrModifier; } }
        /// <summary>Current HR state.</summary>
        public DataState HrState { get { return hrState; } }
        /// <summary>Current GSR modifier.</summary>
        public float GsrModifier { get { return gsrModifier; } }
        /// <summary>Current GSR state.</summary>
        public DataState GsrState { get { return gsrState; } }
        /// <summary>List of connected MS Band devices.</summary>
        public List<string> ConnectedBands { get { return connectedBands; } }
        /// <summary>Informs that biofeedback data has changed.</summary>
        public Action BiofeedbackDataChanged;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            remoteHostName = DefaultHostName;
            remoteServicePort = DefaultServicePort;
            PairedBand = new StringBuilder();
            MessageArrived += receivedMsg =>
            {
                DealWithReceivedMessage(receivedMsg);
                // reset all GUI flags:
                isCalibrationOn = false;
            };
        }

        // Use this for initialization
        private void Start()
        {
            connectedBands = new List<string>();
        }

        // Sent to all game objects before the application is quit
        private void OnApplicationQuit()
        {
            if (refresherWorker != null) refresherWorker.CancelAsync();
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
        /// Saves info about chosen Band device name.
        /// </summary>
        public void PairBand()
        {
            // first, unpair Band if needed:
            if (PairedBand != null && PairedBand.ToString() != "") UnpairBand();
            // pair with new Band:
            string newChosenBand = GameManager.instance.GetChosenBandName();
            if (newChosenBand == null) return;
            PairedBand.Append(newChosenBand);
            isBandPaired = true;
            IsPairedBandChanged = true;
        }

        /// <summary>
        /// Removes info about chosen Band device name.
        /// </summary>
        public void UnpairBand()
        {
            PairedBand.Remove(0, PairedBand.Length);
            isBandPaired = false;
            IsPairedBandChanged = true;
            averageHr = 0;
            averageGsr = 0;
            currentHr = 0;
            currentGsr = 0;
            IsSensorsReadingsChanged = true;
        }

        /// <summary>
        /// Refresh connection with chosen Band.
        /// </summary>
        public void RefreshPairedBand()
        {
            string chosenBand = PairedBand.ToString();
            foreach (string bandName in ConnectedBands)
            {
                // chosen Band is still connected:
                if (chosenBand == bandName) return;
            }
            // chosen Band is not connected any more:
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

        /// <summary>
        /// Initiates calibration of sensors data on chosen Band.
        /// The result of calibration is the control average value of sensors data from specific range of time.
        /// </summary>
        public void CalibrateBandData()
        {
            if (PairedBand == null || PairedBand.ToString() == "") return;

            Message msg = new Message(MessageCode.CALIB_ASK, PairedBand.ToString());
            try
            {
                canReceiveBandReadings = false;
                SendMessageToBandBridgeServer(msg);
                isCalibrationOn = true;
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
                    if (msg.Result.GetType() == typeof(string[]) || msg.Result == null)
                    {
                        // update connected Bands list:
                        ConnectedBands.Clear();
                        ConnectedBands.AddRange((string[])msg.Result);
                        IsConnectedBandsListChanged = true;
                    }
                    RefreshPairedBand();
                    break;

                // update current sensors readings:
                case MessageCode.GET_DATA_ANS:
                    if (msg.Result != null && msg.Result.GetType() == typeof(SensorData[]))
                    {
                        //// update sensors data readings:
                        //currentHr = ((SensorData[])msg.Result)[0].Data;
                        //currentGsr = ((SensorData[])msg.Result)[1].Data;
                        //IsSensorsReadingsChanged = true;
                        //// calculate biofeedback modifiers and assign stated:
                        //UpdateCurrentBiofeedbackData(averageHr, currentHr, averageGsr, currentGsr);

                        // calculate biofeedback modifiers and assign stated:
                        //UpdateCurrentBiofeedbackData(averageHr, ((SensorData[])msg.Result)[0].Data, averageGsr, ((SensorData[])msg.Result)[1].Data);
                        UpdateCurrentBiofeedbackData(((SensorData[])msg.Result)[0].Data, ((SensorData[])msg.Result)[1].Data);
                    }
                    break;

                // update control calibrated sensors readings values:
                case MessageCode.CALIB_ANS:
                    if (msg.Result != null && msg.Result.GetType() == typeof(SensorData[]))
                    {
                        // update sensors data readings:
                        //averageHr = ((SensorData[])msg.Result)[0].Data;
                        //averageGsr = ((SensorData[])msg.Result)[1].Data;
                        UpdateAverageBiofeedbackData(((SensorData[])msg.Result)[0].Data, ((SensorData[])msg.Result)[1].Data);
                        IsAverageReadingsChanged = true;
                        canReceiveBandReadings = true;
                    }
                    break;

                default: break;
            }
        }

        /// <summary>
        /// Updates average biofeedback data values.
        /// </summary>
        public void UpdateAverageBiofeedbackData(int averageHr, int averageGsr)
        {
            this.averageHr = averageHr;
            this.averageGsr = averageGsr;
        }

        /// <summary>
        /// Updates current biofeedback data values.
        /// </summary>
        public void UpdateCurrentBiofeedbackData(int currentHr, int currentGsr)
        //public void UpdateCurrentBiofeedbackData(int averageHr, int currentHr, int averageGsr, int currentGsr)
        {
            // update current HR and GSR values:
            this.currentHr = currentHr;
            this.currentGsr = currentGsr;
            IsSensorsReadingsChanged = true;
            // update HR modifier & state:
            hrModifier = (float)currentHr / averageHr;
            if (hrModifier < 0.5f) hrModifier = 0.5f;   // prevents hrModifier from getting too small values
            hrState = hrLevel.AssignState(hrModifier);
            // update GSR modifier & state:
            gsrModifier = (float)currentGsr / averageGsr;
            if (gsrModifier < 0.5f) gsrModifier = 0.5f; //prevents gsrModifier from getting too small values
            gsrState = gsrLevel.AssignState(gsrModifier);
            // update arousal data:
            arousalState = DataState.None;
            if (GameManager.instance.CurrentCalculationType == CalculationType.Alternative)
            {
                if (hrState == DataState.High || gsrState == DataState.High) arousalState = DataState.High;
                else if (hrState == DataState.Low || gsrState == DataState.Low) arousalState = DataState.Low;
                else arousalState = DataState.Medium;
                // alternative -> maximum value:
                arousalModifier = Mathf.Max(hrModifier, gsrModifier);
            }
            else if (GameManager.instance.CurrentCalculationType == CalculationType.Conjunction)
            {
                if (hrState == DataState.High && gsrState == DataState.High) arousalState = DataState.High;
                else if (hrState == DataState.Low && gsrState == DataState.Low) arousalState = DataState.Low;
                else arousalState = DataState.Medium;
                // conjunction -> minimum value:
                arousalModifier = Mathf.Min(hrModifier, gsrModifier);
            }
            // inform that biofeedback data has changed:
            BiofeedbackDataChanged();
        }
        #endregion
    }
}