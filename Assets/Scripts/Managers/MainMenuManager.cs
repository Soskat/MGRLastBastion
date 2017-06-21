using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MainMenuManager : MonoBehaviour {

    #region Private fields
    [SerializeField] GameObject gameSettingsPanel;
    [SerializeField] GameObject bbMenuPanel;
    #endregion



    #region MonoBehaviour methods
    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        DoAssertions();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion


    #region Private methods
    /// <summary>
    /// Performs assertions to make sure everything is properly initialized.
    /// </summary>
    private void DoAssertions()
    {
        Assert.IsNotNull(gameSettingsPanel);
        Assert.IsNotNull(bbMenuPanel);
    }
    #endregion
}
