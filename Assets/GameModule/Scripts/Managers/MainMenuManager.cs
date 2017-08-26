﻿using LastBastion.Analytics;
using LastBastion.Biofeedback;
using LastBastion.Game.UIControllers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Main Menu scene behaviour.
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Toggle autoGameTypeToggle;
        [SerializeField] private GameObject gameType;
        //[SerializeField] private GameObject analytics;
        [SerializeField] private Toggle analyticsToggle;
        [SerializeField] private Toggle debugModeToggle;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject bbMenuPanel;
        [SerializeField] private GameObject listViewport;
        [SerializeField] private Toggle toggleEnglish;
        [SerializeField] private Toggle togglePolish;
        private GameMode[] gameTypes = { GameMode.ModeA, GameMode.ModeB };
        private Dropdown gameTypeDropdown;
        private Dropdown analyticsDropdown;
        private BandBridgeMenuController bbMenuController;
        private ListController listController;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(autoGameTypeToggle);
            Assert.IsNotNull(gameType);
            Assert.IsNotNull(analyticsToggle);
            Assert.IsNotNull(debugModeToggle);
            Assert.IsNotNull(settingsPanel);
            Assert.IsNotNull(bbMenuPanel);
            Assert.IsNotNull(listViewport);
            Assert.IsNotNull(toggleEnglish);
            Assert.IsNotNull(togglePolish);
        }

        // Use this for initialization
        void Start()
        {
            bbMenuController = bbMenuPanel.GetComponent<BandBridgeMenuController>();
            listController = listViewport.GetComponent<ListController>();
            GameManager.instance.ListController = listController;
            // add game type dropdown:
            gameTypeDropdown = gameType.GetComponent<Dropdown>();
            List<string> gameOptions = new List<string>();
            foreach (var option in gameTypes) gameOptions.Add(option.ToString());
            gameTypeDropdown.AddOptions(gameOptions);
            for (int i = 0; i < gameTypes.Length; i++)
            {
                if (gameTypes[i] == (GameMode)GameManager.instance.BiofeedbackMode)
                {
                    gameTypeDropdown.value = i;
                    break;
                }
            }
            // if autoGameModeToggle is on, disable gameTypeDropdown:
            autoGameTypeToggle.isOn = true;
            SetAutoGameMode(autoGameTypeToggle);

            //// add analytics dropdown:
            //analyticsDropdown = analytics.GetComponent<Dropdown>();
            //analyticsDropdown.AddOptions(new List<string>() { "enabled", "disabled" });
            //if (GameManager.instance.AnalyticsEnabled) analyticsDropdown.value = 0;
            //else analyticsDropdown.value = 1;

            // set up analytics and debug mode toggles:
            analyticsToggle.isOn = GameManager.instance.AnalyticsEnabled;
            debugModeToggle.isOn = GameManager.instance.DebugMode;
            
            // update chosen game language and language toggles:
            GameManager.instance.ChosenLanguage = GameLanguage.Default;
            togglePolish.isOn = !(toggleEnglish.isOn = true);
            GameManager.instance.UpdatedLanguage();
            
            // turn off settingsPanel visibility:
            TurnOffSettingsMenu();
        }

        // Update is called once per frame
        void Update()
        {
            // update the list of connected Bands if needed:
            if (GameManager.instance.BBModule.IsConnectedBandsListChanged)
            {
                listController.ClearList();
                listController.UpdateList(GameManager.instance.BBModule.ConnectedBands.ToArray());
                GameManager.instance.BBModule.IsConnectedBandsListChanged = false;
                GameManager.instance.IsReadyForNewBandData = true;
            }

            // update PairedBand label if needed:
            if (GameManager.instance.BBModule.IsPairedBandChanged)
            {
                //sensorPanelController.UpdateBandLabel(GameManager.instance.BBModule.PairedBand.ToString());
                bbMenuController.PairedBand = GameManager.instance.BBModule.PairedBand.ToString();
                GameManager.instance.BBModule.IsPairedBandChanged = false;
            }
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Starts new game.
        /// </summary>
        public void StartNewGame()
        {
            if (autoGameTypeToggle.isOn)
            {
                // choose game mode based on data from PlayerPrefs:
                int biofeedbackOnGames = PlayerPrefs.GetInt("biofeedbackOnGames");
                int biofeedbackOffGames = PlayerPrefs.GetInt("biofeedbackOffGames");
                if (biofeedbackOnGames <= biofeedbackOffGames) GameManager.instance.BiofeedbackMode = BiofeedbackMode.BiofeedbackON;
                else GameManager.instance.BiofeedbackMode = BiofeedbackMode.BiofeedbackOFF;                
            }
            else GameManager.instance.BiofeedbackMode = (BiofeedbackMode)gameTypes[gameTypeDropdown.value];
            // get analytics and debug mode toggles values:
            GameManager.instance.AnalyticsEnabled = analyticsToggle.isOn;
            GameManager.instance.DebugMode = debugModeToggle.isOn;
            //if (analyticsDropdown.value == 0) GameManager.instance.AnalyticsEnabled = true;
            //else if (analyticsDropdown.value == 1) GameManager.instance.AnalyticsEnabled = false;

            // start new game:
            GameManager.instance.StartNewGame();
        }

        /// <summary>
        /// Turn on settings menu panel.
        /// </summary>
        public void TurnOnSettingsMenu()
        {
            settingsPanel.SetActive(true);
        }

        /// <summary>
        /// Turn off settings menu panel.
        /// </summary>
        public void TurnOffSettingsMenu()
        {
            settingsPanel.SetActive(false);
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("Quitting the game...");
            Application.Quit();
        }
        #endregion


        #region UI events
        /// <summary>
        /// HostNameInput's <see cref="InputField.onEndEdit"/> behaviour.
        /// </summary>
        public void OnHostNameEndEdit()
        {
            GameManager.instance.BBModule.RemoteHostName = bbMenuController.HostName;
        }

        /// <summary>
        /// ServicePortInput's <see cref="InputField.onEndEdit"/> behaviour.
        /// </summary>
        /// <param name="newServicePort">New service port number</param>
        public void OnServicePortEndEdit()
        {
            int servicePort;
            if (!Int32.TryParse(bbMenuController.ServicePort, out servicePort)) GameManager.instance.BBModule.RemoteServicePort = BandBridgeModule.DefaultServicePort;
            else GameManager.instance.BBModule.RemoteServicePort = servicePort;
        }
        
        /// <summary>
        /// Sets game language based on language assigned to given toggle.
        /// </summary>
        /// <param name="toggle">Toggle</param>
        public void SetGameLanguage(Toggle toggle)
        {
            // if toggle is on change game language:
            if (toggle.isOn && toggle.GetComponent<AssignedLanguage>() != null)
            {
                GameManager.instance.ChosenLanguage = toggle.GetComponent<AssignedLanguage>().Language;
                GameManager.instance.UpdatedLanguage();
            }
        }


        public void SetAutoGameMode(Toggle toggle)
        {
            if (toggle.isOn) gameTypeDropdown.enabled = false;
            else gameTypeDropdown.enabled = true;
        }
        #endregion
    }
}