using UnityEngine;
using Communication.Sockets;
using Communication.Data;
using System.ComponentModel;

public class BandBridgeClient : MonoBehaviour {

    public string HostName = "DESKTOP-KPBRM2V";
    public int ServicePort = 2055;

    public int maxMessageSize = 2048;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}




    private void OnGUI()
    {
        if(GUI.Button(new Rect(50, 50, 100, 30), "Connect test"))
        {
            Message message = new Message(MessageCode.SHOW_LIST_ASK, null);
            Message response = SendMessageToBandBridge(message);
            DealWithResponse(response);
        }
    }


    private Message SendMessageToBandBridge(Message message)
    {
        // original source: http://stackoverflow.com/a/34040733

        Debug.Log("Prepaired message: " + message);
        Message response = null;


        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (s, e) => {
            e.Result = SocketClient.StartClient(HostName, ServicePort, message, maxMessageSize);
        };
        worker.RunWorkerCompleted += (s, e) => {
            response = (Message)e.Result;
            //DealWithResponse((Message)e.Result);
        };
        worker.RunWorkerAsync();


        return response;
    }


    private void DealWithResponse(Message response)
    {
        Debug.Log("Received response: " + response + " ------------------------------------");
    }
}