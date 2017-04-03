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
    public class SocketClient {

        // ManualResetEvent instances signal completion:
        //private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static PacketProtocol packetizer = null;
        private static Message receivedResponse;

        //private static int receiveBytesOffset = 0;
        
        public static Message StartClient(string hostName, int port, Message message, int maxMessageSize)
        {
            // Connect to a remote device:
            try {
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
                    Debug.Log("Socket connected to " + client.RemoteEndPoint.ToString());
                }
                else
                {
                    Debug.Log("Could not connect to " + client.RemoteEndPoint.ToString());
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
                    Debug.Log(":: Received bytes: " + receivedMsg.Length + " => ");
                    if (receivedMsg.Length > 0)
                    {
                        Debug.Log("deserialize message");
                        receivedResponse = Message.Deserialize(receivedMsg);
                        Debug.Log(":: Received: " + receivedResponse);
                    }
                };

                // Receive the response from the remote device:
                while (!packetizer.AllBytesReceived)
                {
                    Receive(client);
                    receiveDone.WaitOne();
                }

                // Release the socket.
                client.Shutdown(SocketShutdown.Both);
                client.Close();

                // reset signals completion:
                sendDone.Reset();
                receiveDone.Reset();
                
                return receivedResponse;

            } catch (Exception e) {
                Debug.Log(e.ToString());
                return null;
            }
        }


        private static void Send(Socket client, Message data)
        {
            try {
                // convert message into byte array and wrap it for network transport:
                var serializedMsg = Message.Serialize(data);

                byte[] byteData = PacketProtocol.WrapMessage(serializedMsg);

                // Begin sending the data to the remote device:
                client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);

            } catch(Exception e) {
                Debug.Log(e.ToString());
            }
        }


        private static void SendCallback(IAsyncResult ar)
        {
            try {
                // Retrieve the socket from the state object:
                Socket client = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device:
                int bytesSent = client.EndSend(ar);
                Debug.Log(":: Sent " + bytesSent + " bytes to server.");

                // Signal that all bytes have been sent:
                sendDone.Set();

            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }


        private static void Receive(Socket client)
        {
            try {
                // Create the state object:
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device:
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);

            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }


        private static void ReceiveCallback(IAsyncResult ar)
        {
            try {
                // Retrieve the state object and the client socket from the asynchronous state object:
                StateObject state = (StateObject) ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device:
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0) {
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
