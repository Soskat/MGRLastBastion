using Communication.Client;
using Communication.Data;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Assets.BiofeedbackModule.Scripts
{
    class BandBridgeTest : MonoBehaviour
    {
        public string HostName = "192.168.0.73";
        //public string HostName = "DESKTOP-KPBRM2V";
        public int ServicePort = 2055;
        public string ChoosenBandName = "Fake Band Name";
        public string[] ConnectedBands = new string[] {  };


        // that's what it should look like: ===========================================================   !!!
        private void Test__SHOW_ASK__Result_null()
        {
            Debug.Log(">> SHOW_ASK / Result == null ---------------------");
            Message msg = new Message(Command.SHOW_ASK, null);
            Debug.Log("MSG = " + msg);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) => {
                e.Result = SocketClient.StartClient(HostName, ServicePort, msg);
            };
            worker.RunWorkerCompleted += (s, e) => {
                Message resp = (Message)e.Result;

                while (resp == null) ; // little hack here - wait until whole response from server come

                Debug.Assert(resp != null, "Response is null!");
                if (resp != null)
                {
                    Debug.Assert(resp.Code == Command.SHOW_ANS, "Wrong response Code - expected SHOW_ANS, but get: " + resp.Code);
                    Debug.Assert((resp.Result == null) || (resp.Result.GetType() == typeof(string[])),
                                 "Wrong response Result - expected null or string[], but get: " + resp.Result.GetType());

                    if (resp.Result != null && resp.Result.GetType() == typeof(string[]) && ((string[])resp.Result).Length > 0)
                    {
                        ConnectedBands = (string[])resp.Result;
                        ChoosenBandName = ConnectedBands[0];
                    }
                }
            };

            worker.RunWorkerAsync();
            
            //Debug.Log(">> SHOW_ASK / Result == null ---------------------");
            //Message msg = new Message(Command.SHOW_ASK, null);
            //Debug.Log("MSG = " + msg);
            //Message resp = SendMessageToBandBridge(msg);
            //Debug.Assert(resp != null, "Response is null!");
            //if (resp != null)
            //{
            //    Debug.Assert(resp.Code == Command.SHOW_ANS, "Wrong response Code - expected SHOW_ANS, but get: " + resp.Code);
            //    Debug.Assert((resp.Result == null) || (resp.Result.GetType() == typeof(string[])),
            //                 "Wrong response Result - expected null or string[], but get: " + resp.Result.GetType());

            //    if (resp.Result != null && resp.Result.GetType() == typeof(string[]) && ((string[])resp.Result).Length > 0)
            //    {
            //        ConnectedBands = (string[])resp.Result;
            //        ChoosenBandName = ConnectedBands[0];
            //    }
            //}
        }


        #region Test methods


        private void Test__SHOW_ASK__Result_not_null()
        {
            Debug.Log(">> SHOW_ASK / Result != null ---------------------");
            Message msg = new Message(Command.SHOW_ASK, 42);
            Debug.Log("MSG = " + msg);
            Message resp = SocketClient.StartClient(HostName, ServicePort, msg);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Assert(resp.Code == Command.SHOW_ANS, "Wrong response Code - expected SHOW_ANS, but get: " + resp.Code);
                Debug.Assert((resp.Result == null) || (resp.Result.GetType() == typeof(string[])),
                             "Wrong response Result - expected null or string[], but get: " + resp.Result.GetType());

                if (resp.Result != null && resp.Result.GetType() == typeof(string[]) && ((string[])resp.Result).Length > 0)
                {
                    ConnectedBands = (string[])resp.Result;
                    ChoosenBandName = ConnectedBands[0];
                }
            }
        }

        private void Test__GET_DATA_ASK__Result_null()
        {
            Debug.Log(">> GET_DATA_ASK / Result == null ---------------------");
            Message msg = new Message(Command.GET_DATA_ASK, null);
            Debug.Log("MSG = " + msg);
            Message resp = SocketClient.StartClient(HostName, ServicePort, msg);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Assert(resp.Code == Command.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                Debug.Assert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result.GetType());
            }
        }

        private void Test__GET_DATA_ASK__Result_not_string()
        {
            Debug.Log(">> GET_DATA_ASK / typeof(Result) != typeof(string) ----");
            Message msg = new Message(Command.GET_DATA_ASK, 42);
            Debug.Log("MSG = " + msg);
            Message resp = SocketClient.StartClient(HostName, ServicePort, msg);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Assert(resp.Code == Command.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                Debug.Assert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result.GetType());
            }
        }

        private void Test__GET_DATA_ASK__Result_string()
        {
            Debug.Log(">> GET_DATA_ASK / typeof(Result) == typeof(string) ----");
            Message msg = new Message(Command.GET_DATA_ASK, ChoosenBandName);
            Debug.Log("MSG = " + msg);
            Message resp = SocketClient.StartClient(HostName, ServicePort, msg);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Assert(resp.Code == Command.GET_DATA_ANS, "Wrong response Code - expected GET_DATA_ANS, but get: " + resp.Code);
                Debug.Assert((resp.Result == null) || (resp.Result.GetType() == typeof(SensorData[])),
                             "Wrong response Result - expected null or typeof(SensorData[]), but get: " + resp.Result.GetType());
            }
        }

        private void Test__SHOW_ANS()
        {
            Debug.Log(">> SHOW_ANS / Result == null ----------------------");
            Message msg = new Message(Command.SHOW_ANS, null);
            Debug.Log("MSG = " + msg);
            Message resp = SocketClient.StartClient(HostName, ServicePort, msg);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Assert(resp.Code == Command.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                Debug.Assert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result.GetType());
            }
        }

        private void Test__GET_DATA_ANS()
        {
            Debug.Log(">> GET_DATA_ANS / Result == null ----------------------");
            Message msg = new Message(Command.GET_DATA_ANS, null);
            Debug.Log("MSG = " + msg);
            Message resp = SocketClient.StartClient(HostName, ServicePort, msg);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Assert(resp.Code == Command.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                Debug.Assert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result.GetType());
            }
        }

        private void Test__CTR_MSG()
        {
            Debug.Log(">> CTR_MSG / Result == null ---------------------------");
            Message msg = new Message(Command.CTR_MSG, null);
            Debug.Log("MSG = " + msg);
            Message resp = SocketClient.StartClient(HostName, ServicePort, msg);
            Debug.Assert(resp != null, "Response is null!");
            if (resp != null)
            {
                Debug.Assert(resp.Code == Command.CTR_MSG, "Wrong response Code - expected CTR_MSG, but get: " + resp.Code);
                Debug.Assert(resp.Result == null, "Wrong response Result - expected null, but get: " + resp.Result.GetType());
            }
        }
        
        #endregion
        
        /// <summary>
        /// Starts tests.
        /// </summary>
        public void DoTests()
        {
            Thread t;

            // Begin tests:
            t = new Thread(new ThreadStart(Test__SHOW_ASK__Result_null));
            t.Start(); t.Join(); Thread.Sleep(1);
            Debug.Log("\n");
            //t = new Thread(new ThreadStart(Test__SHOW_ASK__Result_not_null));
            //t.Start(); t.Join(); Thread.Sleep(1);
            //Debug.Log("\n");
            //t = new Thread(new ThreadStart(Test__GET_DATA_ASK__Result_null));
            //t.Start(); t.Join(); Thread.Sleep(1);
            //Debug.Log("\n");
            //t = new Thread(new ThreadStart(Test__GET_DATA_ASK__Result_not_string));
            //t.Start(); t.Join(); Thread.Sleep(1);
            //Debug.Log("\n");
            t = new Thread(new ThreadStart(Test__GET_DATA_ASK__Result_string));
            t.Start(); t.Join(); Thread.Sleep(1);
            Debug.Log("\n");
            //t = new Thread(new ThreadStart(Test__SHOW_ANS));
            //t.Start(); t.Join(); Thread.Sleep(1);
            //Debug.Log("\n");
            //t = new Thread(new ThreadStart(Test__GET_DATA_ANS));
            //t.Start(); t.Join(); Thread.Sleep(1);
            //Debug.Log("\n");
            //t = new Thread(new ThreadStart(Test__CTR_MSG));
            //t.Start(); t.Join(); Thread.Sleep(1);
            //Debug.Log("\n");
        }







        private Message SendMessageToBandBridge(Message message)
        {
            // original source: http://stackoverflow.com/a/34040733

            Message response = null;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) => {
                e.Result = SocketClient.StartClient(HostName, ServicePort, message);
            };
            worker.RunWorkerCompleted += (s, e) => {
                response = (Message)e.Result;

                //while (response == null) ; // little hack here - wait until whole response from server come

                //Debug.Assert(response != null, "Response is null!");
                //if (response != null)
                //{
                //    Debug.Assert(response.Code == Command.SHOW_ANS, "Wrong response Code - expected SHOW_ANS, but get: " + response.Code);
                //    Debug.Assert((response.Result == null) || (response.Result.GetType() == typeof(string[])),
                //                 "Wrong response Result - expected null or string[], but get: " + response.Result.GetType());

                //    if (response.Result != null && response.Result.GetType() == typeof(string[]) && ((string[])response.Result).Length > 0)
                //    {
                //        ConnectedBands = (string[])response.Result;
                //        ChoosenBandName = ConnectedBands[0];
                //    }
                //}
            };

            worker.RunWorkerAsync();

            return response;
        }







        private void OnGUI()
        {
            if(GUI.Button(new Rect(10, 10, 150, 30), "Test all messages"))
            {
                Debug.Log("=== Start message system tests ===");
                DoTests();
                Debug.Log("=== Stop message system tests ====");
            }
        }
    }
}
