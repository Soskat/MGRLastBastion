using Communication.Data;
using Communication.Packet;
using Communication.Sockets;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace Communication.Sockets
{
    // Original source: https://msdn.microsoft.com/en-us/library/fx6588te(v=vs.110).aspx

    /// <summary>
    /// Asynchronous server working on sockets.
    /// </summary>
    class SocketServer
    {
        // Thread signal.  
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static PacketProtocol packetizer = null;
        private static Message receivedResponse;

        public static bool EnableWorking { get; set; }


        public static void StartListening(int openPort, int backlogLength, int maxMessageSize)
        {
            try {
                // Establish the local endpoint for the socket:
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = Array.Find(ipHostInfo.AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, openPort);

                // Create a TCP/IP socket.  
                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                EnableWorking = true;

                // create packetizer object:
                packetizer = new PacketProtocol(maxMessageSize);
                packetizer.MessageArrived += receivedMsg =>
                {
                    Debug.Log("SS:: Received bytes: " + receivedMsg.Length + " => ");
                    if (receivedMsg.Length > 0)
                    {
                        //Debug.Log("deserialize message");
                        receivedResponse = Message.Deserialize(receivedMsg);
                        Debug.Log(receivedResponse);

                        // signal that new message arrived:
                        // ...
                    }
                };

                // Bind the socket to the local endpoint and listen for incoming connections:
                listener.Bind(localEndPoint);
                listener.Listen(backlogLength);

                while (EnableWorking)
                {
                    // Set the event to nonsignaled state.  
                    connectDone.Reset();
                    receiveDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Debug.Log("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    // Wait until a connection is made before continuing.  
                    connectDone.WaitOne();

                    // receive the whole message from the remote device:
                    while (!packetizer.AllBytesReceived)
                    {
                        Receive(listener);
                        receiveDone.WaitOne();
                    }
                }

            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            try {
                // Get the socket that handles the client request.  
                Socket listener = (Socket)ar.AsyncState;
                listener.EndAccept(ar);

                // Signal the main thread to continue.  
                connectDone.Set();

            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }

        private static void Receive(Socket listener)
        {
            try {
                // Create the state object:
                StateObject state = new StateObject();
                state.workSocket = listener;

                // Begin receiving the data from the remote device:
                listener.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            try {
                // Retrieve the state object and the handler socket from the asynchronous state object:
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket:
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // pass received data and receiving process to packetizer object:
                    packetizer.DataReceived(state.buffer);
                }

                // Signal that all bytes have been received:
                receiveDone.Set();

            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }
        



    }
}
