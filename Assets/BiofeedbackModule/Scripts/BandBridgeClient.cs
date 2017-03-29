using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Communication.Client;

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
            SocketClient.StartClient(HostName, ServicePort);
        }
    }
}