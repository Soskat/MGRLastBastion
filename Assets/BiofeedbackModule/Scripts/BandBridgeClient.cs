using UnityEngine;
using Communication.Client;
using Communication.Data;

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
            Debug.Log(">>__La message: " + message);

            Message response = SocketClient.StartClient(HostName, ServicePort, message);
            Debug.Log(">>__La response: " + response);
        }
    }
}