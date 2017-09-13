using LastBastion.Analytics;
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
        /// <summary>Auto-choose game type toggle.</summary>
        [SerializeField] private Toggle autoGameTypeToggle;
        /// <summary>Dropdown menu with game types options.</summary>
        [SerializeField] private Dropdown gameTypeDropdown;
        /// <summary>Analytics toggle.</summary>
        [SerializeField] private Toggle analyticsToggle;
        /// <summary>Debug mode toggle.</summary>
        [SerializeField] private Toggle debugModeToggle;
        /// <summary>Biofeedback simulator mode toggle.</summary>
        [SerializeField] private Toggle simulatorToggle;
        /// <summary>Settings panel object.</summary>
        [SerializeField] private GameObject settingsPanel;
        /// <summary>BandBridge menu panel object.</summary>
        [SerializeField] private GameObject bbMenuPanel;
        /// <summary>List viewport object.</summary>
        [SerializeField] private GameObject listViewport;
        /// <summary>English language toggle.</summary>
        [SerializeField] private Toggle toggleEnglish;
        /// <summary>Polish language toggle.</summary>
        [SerializeField] private Toggle togglePolish;
        /// <summary>Game type options.</summary>
        private GameMode[] gameTypes = { GameMode.ModeA, GameMode.ModeB };
        /// <summary>Component that manages UI interaction of BandBridge panel.</summary>
        private BandBridgeMenuController bbMenuController;
        /// <summary>Component that manages UI interaction of list viewport.</summary>
        private ListController listController;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(autoGameTypeToggle);
            Assert.IsNotNull(gameTypeDropdown);
            Assert.IsNotNull(analyticsToggle);
            Assert.IsNotNull(debugModeToggle);
            Assert.IsNotNull(simulatorToggle);
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
            // add options to game type dropdown:
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

            // set up analytics, debug mode and simulator toggles:
            analyticsToggle.isOn = GameManager.instance.AnalyticsEnabled;
            debugModeToggle.isOn = GameManager.instance.DebugMode;
            simulatorToggle.isOn = GameManager.instance.BBModule.IsSimulatorEnabled;
            
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
            if (GameManager.instance.BBModule.ConnectedBandsListChanged)
            {
                listController.ClearList();
                listController.UpdateList(GameManager.instance.BBModule.ConnectedBands.ToArray());
                GameManager.instance.BBModule.ConnectedBandsListChanged = false;
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
        /// Starts a new game.
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
            // get analytics, debug mode and simulator toggles values:
            GameManager.instance.AnalyticsEnabled = analyticsToggle.isOn;
            GameManager.instance.DebugMode = debugModeToggle.isOn;
            GameManager.instance.BBModule.IsSimulatorEnabled = simulatorToggle.isOn;

            // start new game:
            GameManager.instance.StartNewGame();
        }

        /// <summary>
        /// Turns on settings menu panel.
        /// </summary>
        public void TurnOnSettingsMenu()
        {
            settingsPanel.SetActive(true);
        }

        /// <summary>
        /// Turns off settings menu panel.
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
        /// <param name="toggle">Language toggle</param>
        public void SetGameLanguage(Toggle toggle)
        {
            // if toggle is on change game language:
            if (toggle.isOn && toggle.GetComponent<AssignedLanguage>() != null)
            {
                GameManager.instance.ChosenLanguage = toggle.GetComponent<AssignedLanguage>().Language;
                GameManager.instance.UpdatedLanguage();
            }
        }

        /// <summary>
        /// Sets auto-choose mode of game mode if given toggle is on.
        /// </summary>
        /// <param name="toggle">Toggle</param>
        public void SetAutoGameMode(Toggle toggle)
        {
            if (toggle.isOn) gameTypeDropdown.interactable = false;
            else gameTypeDropdown.interactable = true;
        }
        #endregion
    }
}