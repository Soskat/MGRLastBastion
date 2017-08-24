using LastBastion.Analytics;
using LastBastion.Biofeedback;
using LastBastion.Game.UIControllers;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages all game logic.
    /// </summary>
    [RequireComponent(typeof(AssetManager))]
    [RequireComponent(typeof(BiofeedbackSimulator))]
    [RequireComponent(typeof(BandBridgeModule))]
    public class GameManager : MonoBehaviour
    {
        #region Static fields
        /// <summary><see cref="GameManager"/> public static object.</summary>
        public static GameManager instance;
        #endregion


        #region Private fields
        [SerializeField] private BiofeedbackMode biofeedbackMode = BiofeedbackMode.BiofeedbackON;
        [SerializeField] private bool debugMode = false;
        [SerializeField] private int currentLevelID = -1;
        [SerializeField] private string[] gameLevels;
        [SerializeField] private int currentCalculationTypeID = 0;
        [SerializeField] private CalculationType[] calculationTypes;
        private int indexOfFirstLevel;
        private int indexOfSecondLevel;
        private int ignoreLightLayer;
        private int interactiveObjectsLayer;
        #endregion


        #region Public fields & properties
        /// <summary>Is debug mode on?</summary>
        public bool DebugMode { get { return debugMode; } }
        /// <summary>Choosen game language.</summary>
        public GameLanguage ChoosenLanguage;
        /// <summary>Instance of <see cref="BandBridgeModule"/> class.</summary>
        public BandBridgeModule BBModule { get; set; }
        /// <summary>Instance of <see cref="AssetManager"/> class.</summary>
        public AssetManager Assets { get; set; }
        /// <summary>Is ready for new MS Band device sensors data?</summary>
        public bool IsReadyForNewBandData { get; set; }
        /// <summary>Instance of <see cref="ListController"/> class.</summary>
        public ListController ListController { get; set; }
        /// <summary>Active method of calculating player's arousal.</summary>
        public CalculationType CurrentCalculationType { get { return calculationTypes[currentCalculationTypeID]; } }
        /// <summary>Current biofeedback mode.</summary>
        public BiofeedbackMode BiofeedbackMode { get { return biofeedbackMode; } set { biofeedbackMode = value; } }
        /// <summary>Is analytics module enabled?</summary>
        public bool AnalyticsEnabled = true;
        /// <summary>The Room where player currently is.</summary>
        public GameObject ActiveRoom { get; set; }
        /// <summary>IgnoreLight layer number.</summary>
        public int IgnoreLightLayer { get { return ignoreLightLayer; } }
        /// <summary>InteractiveObjects layer number.</summary>
        public int InteractiveObjectsLayer { get { return interactiveObjectsLayer; } }
        /// <summary>Reference to SurveyManager component.</summary>
        public SurveyManager SurveyManager { get; set; }
        #region Achievements counters ---------
        /// <summary>Time of the game.</summary>
        public TimeSpan GameTime { get; set; }
        /// <summary>Collected runes.</summary>
        public int CollectedRunes { get; set; }
        /// <summary>Amount of runes at all.</summary>
        public int RunesAmount { get; set; }
        /// <summary>Searched rooms count.</summary>
        public int SearchedRooms { get; set; }
        /// <summary>Uses of the lightswitches count.</summary>
        public int LightSwitchUses { get; set; }
        #endregion
        /// <summary>Informs all objects that choosen language has changed.</summary>
        public Action UpdatedLanguage { get; set; }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // make sure that Singleton design pattern is preserved and GameManager object will always exist:
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                BBModule = GetComponent<BandBridgeModule>();
                Assets = GetComponent<AssetManager>();
                ignoreLightLayer = LayerMask.NameToLayer("IgnoreLight");
                interactiveObjectsLayer = LayerMask.NameToLayer("InteractiveObjects");
            }
            else if (instance != this) Destroy(gameObject);
        }

        // Use this for initialization
        void Start()
        {
            IsReadyForNewBandData = false;
            gameLevels = new string[] { "Intro", null, "Summary", null, "Summary", "Survey" };
            indexOfFirstLevel = 1;
            indexOfSecondLevel = 3;
            calculationTypes = new CalculationType[2] { CalculationType.Alternative, CalculationType.Conjunction };
            UpdatedLanguage += () => { };  // in case there were no active LabelTranslator components
            // initialize analytics system:
            DataManager.InitializeSystem();
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Gets currently selected item on connected Bands list.
        /// </summary>
        /// <returns>Currently selected Band name</returns>
        public string GetChoosenBandName()
        {
            return ListController.GetSelectedItem();
        }
        
        /// <summary>
        /// Starts new game.
        /// </summary>
        public void StartNewGame()
        {
            currentLevelID = -1;
            currentCalculationTypeID = 0;

            // set levels in random order:
            switch (UnityEngine.Random.Range(0, 2))
            {
                case 0:
                    gameLevels[indexOfFirstLevel] = "LevelA";
                    gameLevels[indexOfSecondLevel] = "LevelB";
                    break;

                case 1:
                    gameLevels[indexOfFirstLevel] = "LevelB";
                    gameLevels[indexOfSecondLevel] = "LevelA";
                    break;
            }
            // set biofeedback calculation mode in random order:
            switch (UnityEngine.Random.Range(0, 2))
            {
                case 0:
                    calculationTypes[0] = CalculationType.Alternative;
                    calculationTypes[1] = CalculationType.Conjunction;
                    break;

                case 1:
                    calculationTypes[0] = CalculationType.Conjunction;
                    calculationTypes[1] = CalculationType.Alternative;
                    break;
            }

            // setup new analysis data:
            if (AnalyticsEnabled) DataManager.BeginAnalysis(BiofeedbackMode);

            IsReadyForNewBandData = true;

            // load next scene:
            LoadNextLevel();
        }

        /// <summary>
        /// Loads next game scene.
        /// </summary>
        public void LoadNextLevel()
        {
            currentLevelID++;

            // set up current calculation type if needed:
            if (currentLevelID == 2) currentCalculationTypeID++;

            // load next scene (or main menu):
            if (currentLevelID < gameLevels.Length) SceneManager.LoadScene(gameLevels[currentLevelID]);
            else BackToMainMenu();
        }

        /// <summary>
        /// Loads main menu scene.
        /// </summary>
        public void BackToMainMenu()
        {
            if (AnalyticsEnabled) DataManager.EndAnalysis();
            IsReadyForNewBandData = false;
            SceneManager.LoadScene("MainMenu");
        }
        #endregion
    }
}