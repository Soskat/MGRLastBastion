using UnityEngine;
using Communication.Client;
using Communication.Data;
using System.ComponentModel;

public class BandBridgeClient : MonoBehaviour {

    public string HostName = "DESKTOP-KPBRM2V";
    public int ServicePort = 2055;

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
            Message message = new Message(Command.SHOW_ASK, null);
            SendMessageToBandBridge(message);
        }
    }


    private void SendMessageToBandBridge(Message message)
    {
        // original source: http://stackoverflow.com/a/34040733

        Debug.Log("Prepaired message: " + message);

        BackgroundWorker worker = new BackgroundWorker();
        worker.DoWork += (s, e) => {
            e.Result = SocketClient.StartClient(HostName, ServicePort, message);
        };
        worker.RunWorkerCompleted += (s, e) => {
            DealWithResponse((Message)e.Result);
        };
        worker.RunWorkerAsync();
    }


    private void DealWithResponse(Message response)
    {
        Debug.Log("Received response: " + response);
    }
}