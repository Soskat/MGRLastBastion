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
    public class GameManager : MonoBehaviour
    {
        #region Static fields
        /// <summary>
        /// <see cref="GameManager"/> public static object.
        /// </summary>
        public static GameManager instance;
        #endregion


        #region Private fields
        [SerializeField] private int currentLevelID = -1;
        [SerializeField] private string[] gameLevels;
        [SerializeField] private int currentCalculationTypeID = 0;
        [SerializeField] private CalculationType[] calculationTypes;
        private DateTime startTime;
        private DateTime currentTime;
        private int indexOfFirstLevel;
        private int indexOfSecondLevel;
        #endregion


        #region Public fields & properties
        /// <summary>Instance of <see cref="BandBridgeModule"/> class.</summary>
        public BandBridgeModule BBModule { get; set; }
        /// <summary>Instance of <see cref="BiofeedbackSimulator"/> class.</summary>
        public BiofeedbackSimulator BBSimulator { get; set; }
        /// <summary>Is ready for new MS Band device sensors data?</summary>
        public bool IsReadyForNewBandData { get; set; }
        /// <summary>Instance of <see cref="ListController"/> class.</summary>
        public ListController ListController { get; set; }
        /// <summary>Active method of calculating player's arousal.</summary>
        public CalculationType CurrentCalculationType { get { return calculationTypes[currentCalculationTypeID]; } }
        /// <summary>Current time stamp.</summary>
        public int GetTime { get { return (currentTime - startTime).Milliseconds; } }
        /// <summary>Current game mode.</summary>
        public GameMode GameMode { get; set; }
        /// <summary>Is analytics module enabled?</summary>
        public bool AnalyticsEnabled = true;
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
                BBModule = gameObject.GetComponent<BandBridgeModule>();
                BBSimulator = gameObject.GetComponent<BiofeedbackSimulator>();
            }
            else if (instance != this) Destroy(gameObject);
        }

        // Use this for initialization
        void Start()
        {
            IsReadyForNewBandData = false;
            gameLevels = new string[] { "Intro", null, "Summary", "Intro", null, "Summary", "Survey" };
            indexOfFirstLevel = 1;
            indexOfSecondLevel = 4;
            calculationTypes = new CalculationType[2];

            // initialize analytics system:
            DataManager.InitializeSystem();
        }

        // Update is called every frame, if the MonoBehaviour is enabled
        void Update()
        {

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
        /// Sets current in-game reference time.
        /// </summary>
        public void SetTime()
        {
            currentTime = DateTime.Now;
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
            if (AnalyticsEnabled)
            {
                DataManager.BeginAnalysis(GameMode);
                startTime = DateTime.Now;
            }

            IsReadyForNewBandData = true;

            // load next scene:
            LoadNextLevel();
        }

        /// <summary>
        /// Informs that level has ended.
        /// </summary>
        public void LevelHasEnded()
        {
            if (AnalyticsEnabled && (currentLevelID == indexOfFirstLevel || currentLevelID == indexOfSecondLevel))
            {
                SetTime();
                DataManager.AddGameEvent(Analytics.EventType.GameEnd, GetTime);
            }
            LoadNextLevel();
        }

        /// <summary>
        /// Loads next game level.
        /// </summary>
        public void LoadNextLevel()
        {
            currentLevelID++;

            // set up current calculation type if needed:
            if (currentLevelID == 3) currentCalculationTypeID++;

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