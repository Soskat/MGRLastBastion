using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CommunicationManager : MonoBehaviour {

    public GameObject menuPanel;
    public GameObject refreshingInfo;

    private bool isMenuOn = false;



    private void Awake()
    {
        // make sure all objects exist:
        DoAssertTests();
    }

    // Use this for initialization
    void Start () {
        menuPanel.SetActive(false);
        refreshingInfo.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchMenuState();
	}


    /// <summary>
    /// Turns BandBridge menu on and off.
    /// </summary>
    public void SwitchMenuState()
    {
        isMenuOn = !isMenuOn;
        if (isMenuOn)
            menuPanel.SetActive(true);
        else
            menuPanel.SetActive(false);
    }

    /// <summary>
    /// Performs assertions to make sure everything is properly initialized.
    /// </summary>
    private void DoAssertTests()
    {
        Assert.IsNotNull(menuPanel);
        Assert.IsNotNull(refreshingInfo);
    }
}
