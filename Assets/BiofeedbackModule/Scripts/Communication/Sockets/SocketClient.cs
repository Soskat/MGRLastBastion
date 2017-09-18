using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Communication.Data;
using Communication.Packet;
using UnityEngine;

namespace Communication.Sockets
{
    // Original source: https://msdn.microsoft.com/en-us/library/bew39x2a(v=vs.110).aspx

    /// <summary>
    /// Asynchronous client working on sockets.
    /// </summary>
    public class SocketClient
    {
        #region Constants
        /// <summary>
        /// Maximum size of one message packet.
        /// </summary>
        public const int MaxMessageSize = 2048;
        #endregion

        #region Static fields
        /// <summary>
        /// ManualResetEvent instance signal completion for sending action.
        /// </summary>
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        /// <summary>
        /// ManualResetEvent instance signal completion for receiving action.
        /// </summary>
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        /// <summary>
        /// ManualResetEvent instances signal completion for completing all actions.
        /// </summary>
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        /// <summary>
        /// <see cref="PacketProtocol"/> object.
        /// </summary>
        private static PacketProtocol packetizer = null;
        /// <summary>
        /// Received response. Is instance of <see cref="Message"/>.
        /// </summary>
        private static Message receivedResponse;
        #endregion

        #region Public static methods
        /// <summary>
        /// Starts asynchronous socket client.
        /// </summary>
        /// <param name="hostName">Name of the remote host to connect</param>
        /// <param name="port">Number of the remote host service port</param>
        /// <param name="message">Message to send</param>
        /// <param name="maxMessageSize">Maximum size of one message packet</param>
        /// <returns>Received response</returns>
        public static Message StartClient(string hostName, int port, Message message, int maxMessageSize)
        {
            // Connect to a remote device:
            try
            {
                // reset signals:
                sendDone.Reset();
                allDone.Reset();

                // Establish the remote endpoint for the socket:
                IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
                IPAddress ipAddress = Array.Find(ipHostInfo.AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket:
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint:
                var result = client.BeginConnect(remoteEP, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(3000);
                if (success)
                {
                    client.EndConnect(result);
                }
                else
                {
                    client.Close();
                    return null;
                }

                // Send test data to the remote device:
                Send(client, message);
                sendDone.WaitOne();

                // create packetizer object:
                packetizer = new PacketProtocol(maxMessageSize);
                packetizer.MessageArrived += receivedMsg =>
                {
                    if (receivedMsg.Length > 0)
                    {
                        receivedResponse = Message.Deserialize(receivedMsg);
                        Debug.Log(String.Format("\tReceived {0} bytes", receivedMsg.Length));
                        //Debug.Log("Received: " + receivedResponse);
                        allDone.Set();
                    }
                };

                // Receive the response from the remote device:
                while (!packetizer.AllBytesReceived)
                {
                    receiveDone.Reset();
                    Receive(client);
                    receiveDone.WaitOne();
                }
                allDone.WaitOne();

                // Release the socket.
                client.Shutdown(SocketShutdown.Both);
                client.Close();

                return receivedResponse;

            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
                return new Message(MessageCode.CTR_MSG, null);
            }
        }
        #endregion

        #region Private static methods
        /// <summary>
        /// Sends message to remote client.
        /// </summary>
        /// <param name="client">Remote client socket object</param>
        /// <param name="data">Message to send</param>
        private static void Send(Socket client, Message data)
        {
            try
            {
                // convert message into byte array and wrap it for network transport:
                var serializedMsg = Message.Serialize(data);
                byte[] byteData = PacketProtocol.WrapMessage(serializedMsg);

                Debug.Log(String.Format("\t...almost send {0} bytes [{1}]", byteData.Length, data.Code));//-----------

                // Begin sending the data to the remote device:
                client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);

            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        /// <summary>
        /// Ends sending the message to remote client.
        /// </summary>
        /// <param name="ar"></param>
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object:
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device:
                client.EndSend(ar);

                // Signal that all bytes have been sent:
                sendDone.Set();

            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        /// <summary>
        /// Receives message from remote client.
        /// </summary>
        /// <param name="client">Remote client socket object</param>
        private static void Receive(Socket client)
        {
            try
            {
                // Create the state object:
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device:
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        /// <summary>
        /// Ends receiving message from remote client.
        /// </summary>
        /// <param name="ar"></param>
        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket from the asynchronous state object:
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;


                if (state == null) Debug.Log("State is null");
                if (packetizer == null) Debug.Log("packetizer is null");


                // Read data from the remote device:
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // pass received data and receiving process to packetizer object:
                    packetizer.DataReceived(state.buffer);
                }

                // Signal that all bytes have been received:
                receiveDone.Set();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
        #endregion
    }
}
