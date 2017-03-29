using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Communication.Data;
using Communication.Packet;
using UnityEngine;

namespace Communication.Client
{
    // State object for receiving data from remote device.
    public class StateObject {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        // public StringBuilder sb = new StringBuilder();
    }

    public class SocketClient {

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static PacketProtocol packetizer = null;
        private static String response = String.Empty;
        private static Message receivedResponse;

        private static void StartClient(string hostName, int port) {
            // Connect to a remote device.
            // try {
                // Establish the remote endpoint for the socket
                IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
                Debug.Log("Host IP addresses:");
                foreach (var item in ipHostInfo.AddressList)
                {
                    Debug.Log(item.ToString());
                }
                IPAddress ipAddress = ipHostInfo.AddressList[ipHostInfo.AddressList.Length - 1];
                Debug.Log("ipAddress: " + ipAddress.ToString());
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    
                // Connect to the remote endpoint.
                client.BeginConnect( remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                packetizer = new PacketProtocol(2048);
                // SensorData sd = new SensorData(SensorCode.HR, 125);
                Message message = new Message(Command.SHOW_ASK, null);
                Debug.Log(message == null);

                // Send test data to the remote device.
                Send(client, message);
                sendDone.WaitOne();

                Debug.Log("Prepare for receiving...");
                packetizer.MessageArrived += receivedMsg => 
                {
                    Console.Write(":: received bytes: " + receivedMsg.Length + " => ");
                    if (receivedMsg.Length > 0)
                    {
                        Debug.Log("deserialize message");
                        receivedResponse = Message.Deserialize(receivedMsg);
                        Debug.Log("::Received: " + receivedResponse);
                    }
                    else Debug.Log("keepalive message");
                };
                // Receive the response from the remote device.
                Receive(client);
                receiveDone.WaitOne();

                // Write the response to the console.
                if (response != null) Debug.Log("Response received: " + response);
                else Debug.Log("Buka - response received == null!");

                // Release the socket.
                client.Shutdown(SocketShutdown.Both);
                client.Close();

            // } catch (Exception e) {
            //     Debug.Log(e.ToString());
            // }
        }


        private static void ConnectCallback(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.
                Socket client = (Socket) ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                Debug.Log("Socket connected to " + client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.
                connectDone.Set();
            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }


        private static void Send(Socket client, Message data) {
            // Convert the string data to byte data using ASCII encoding.
            // byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Debug.Log("Message to send: " + data);
            // convert message into byte array and wrap it for network transport:
            var serializedMsg = Message.Serialize(data);
            Debug.Log("serialized msg length: " + serializedMsg.Length);

            byte[] byteData = PacketProtocol.WrapMessage(serializedMsg);
            Debug.Log("wrapped serialized msg length: " + byteData.Length);

            // Begin sending the data to the remote device.
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }


        private static void SendCallback(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.
                Socket client = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Debug.Log("Sent " + bytesSent + " bytes to server.");

                // Signal that all bytes have been sent.
                sendDone.Set();
            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }


        private static void Receive(Socket client) {
            try {
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = client;
                // Begin receiving the data from the remote device.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }


        private static void ReceiveCallback( IAsyncResult ar ) {
            try {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.
                StateObject state = (StateObject) ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                Debug.Log("Recv_2 - bytesRead: " + bytesRead);

                if (bytesRead > 0) {

                    // There might be more data, so store the data received so far.
                    // state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    
                    packetizer.DataReceived(state.buffer);

                    // // Get the rest of the data.
                    // client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                // else {
                //     // All the data has arrived; put it in response.

                //     // if (state.sb.Length > 1) {
                //     //     response = state.sb.ToString();
                //     // }

                //     // Signal that all bytes have been received.
                //     receiveDone.Set();
                //     Debug.Log("Recv is done");
                // }
                receiveDone.Set();
                Debug.Log("Recv is done");
            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }






        public static void Main(String[] args) {
            StartClient("DESKTOP-KPBRM2V", 2055);
            // StartClient("ROGwolf", 2055);
            
            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey();
        }
    }
}
