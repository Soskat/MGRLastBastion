using LastBastion.Analytics;
using LastBastion.Game.Player;
using LastBastion.Game.Plot;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Level scene behaviour.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        #region Static fields
        /// <summary><see cref="LevelManager"/> public static object.</summary>
        public static LevelManager instance;
        #endregion


        #region Private fields
        [SerializeField] private LevelName levelName;
        [SerializeField] private int runesLimit = 0;
        [SerializeField] private Goal currentGoal;
        [SerializeField] private GameObject endGameTriggerPanel;
        [SerializeField] private GameObject goalUpdatePanel;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private Button skipLevelButton;
        [SerializeField] private Text goalUpdateHeadlineText;
        [SerializeField] private Text goalUpdateContentText;
        #region Achievements counters:
        [SerializeField] private int searchedRooms = 0;
        [SerializeField] private int lightSwitchUses = 0;
        #endregion
        private Stopwatch stopwatch;
        private TimeSpan currentTime;
        private GameObject player;
        private PlayerAudioManager playerBiofeedback;
        private RunesManager runesManager;
        private int activatedRunes;
        private bool menuOn;
        #endregion


        #region Public fields & properties
        /// <summary>Name of the level.</summary>
        public LevelName LevelName { get { return levelName; } }
        /// <summary>Fixed max amount of the runes that player can find in this level.</summary>
        public int RunesLimit { get { return runesLimit; } }
        /// <summary>Current plot goal.</summary>
        public Goal CurrentGoal { get { return currentGoal; } }
        /// <summary>Reference to player game object.</summary>
        public GameObject Player { get { return player; } }
        /// <summary>Reference to player's BiofeedbackController component.</summary>
        public PlayerAudioManager PlayerBiofeedback { get { return playerBiofeedback; } }
        /// <summary>Reference to RuneManager instance.</summary>
        public RunesManager RuneManager { get { return runesManager; } }
        /// <summary>Is current goal the last one?</summary>
        public bool CurrentGoalIsTheOrbGoal { get { return currentGoal.Weight == GetComponent<PlotManager>().OrbGoal.Weight; } }
        /// <summary>Is the outro of the level playing?</summary>
        public bool IsOutroOn { get; set; }
        /// <summary>Is game paused?</summary>
        public bool IsPaused { get { return menuOn; } }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // make sure that Singleton design pattern is preserved and GameManager object will always exist:
            if (instance == null)
            {
                instance = this;
                player = GameObject.FindGameObjectWithTag("Player");
                playerBiofeedback = player.GetComponent<PlayerAudioManager>();
                GameObject go = GameObject.FindGameObjectWithTag("RunesManager");
                if (go != null) runesManager = go.GetComponent<RunesManager>();
                IsOutroOn = false;
                activatedRunes = 0;
                // make some assertions:
                Assert.IsNotNull(endGameTriggerPanel);
                Assert.IsNotNull(goalUpdatePanel);
                Assert.IsNotNull(menuPanel);
                Assert.IsNotNull(resumeButton);
                Assert.IsNotNull(backToMainMenuButton);
                Assert.IsNotNull(skipLevelButton);
                Assert.IsNotNull(goalUpdateHeadlineText);
                Assert.IsNotNull(goalUpdateContentText);
            }
            else if (instance != this) Destroy(gameObject);
        }

        // Use this for initialization
        void Start()
        {
            goalUpdatePanel.SetActive(false);
            SetEndGamePanelActivityStateTo(false);
            currentGoal = GetComponent<PlotManager>().Init();
            // set up in-game menu:
            resumeButton.onClick.AddListener(() => {
                menuOn = false;
                menuPanel.SetActive(menuOn);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            });
            backToMainMenuButton.onClick.AddListener(() => {
                StopAllCoroutines();
                GameManager.instance.BackToMainMenu();
            });
            skipLevelButton.onClick.AddListener(() => {
                EndLevel();
            });
            skipLevelButton.gameObject.SetActive(GameManager.instance.DebugMode);
            menuOn = false;
            menuPanel.SetActive(menuOn);
            // reset achievements:
            searchedRooms = 0;
            lightSwitchUses = 0;
            // start stopwatch:
            stopwatch = new Stopwatch();
            stopwatch.Start();
            // if debug mode is enabled, show debug info:
            if (GameManager.instance.DebugMode && GetComponent<DebugInfo>() != null) GetComponent<DebugInfo>().enabled = true;
            else GetComponent<DebugInfo>().enabled = false;
            
            // save level info:
            if (GameManager.instance.AnalyticsEnabled)
            {
                //DataManager.AddLevelInfo(levelName, GameManager.instance.CurrentCalculationType, GameManager.instance.BBModule.AverageHr, GameManager.instance.BBModule.AverageGsr);
                DataManager.AddLevelInfo(levelName, GameManager.instance.CurrentCalculationType);
                DataManager.AddGameEvent(Analytics.EventType.GameStart, stopwatch.Elapsed);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuOn = menuOn ? false : true;
                menuPanel.SetActive(menuOn);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            // read input:
            if (!Player.GetComponent<InteractionController>().IsFocused && !IsOutroOn && Input.GetKeyDown(KeyCode.Q)) ShowCurrentGoal();

            // debug mode:
            if (GameManager.instance.DebugMode)
            {
                if (Input.GetKeyDown(KeyCode.Keypad1)) EndLevel();
            }

            // manage biofeedback: ===============================================
            // get current Band sensors readings:
            if (GameManager.instance.BBModule.IsBandPaired && GameManager.instance.BBModule.CanReceiveBandReadings && GameManager.instance.IsReadyForNewBandData)
            {
                GameManager.instance.BBModule.GetBandData();
                GameManager.instance.IsReadyForNewBandData = false;
            }
            // update sensors readings values:
            if (GameManager.instance.BBModule.IsSensorsReadingsChanged)
            {
                // save new sensors readings values:
                if (GameManager.instance.AnalyticsEnabled) AddBiofeedbackEvents();
                // reset flags:
                GameManager.instance.BBModule.IsSensorsReadingsChanged = false;
                GameManager.instance.IsReadyForNewBandData = true;
            }
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Sets activity state of endGameTriggerPanel to given value.
        /// </summary>
        /// <param name="visibility">New value activity state</param>
        public void SetEndGamePanelActivityStateTo(bool visibility)
        {
            endGameTriggerPanel.SetActive(visibility);
        }

        /// <summary>
        /// Actions after finding a rune.
        /// </summary>
        public void FoundRune()
        {
            if (runesManager == null) return;
            // update runes count:
            runesManager.CollectRune();
            // show update info:
            StopAllCoroutines();
            // choosen language is polish:
            if (GameManager.instance.ChoosenLanguage == GameLanguage.Polish)
            {
                if (runesManager.CollectedRunes == 1)
                    StartCoroutine(ShowPlotInfoPanel("Znaleziono runę", "Zebrałeś " + runesManager.CollectedRunes + " runę"));
                else if (runesManager.CollectedRunes > 1 && runesManager.CollectedRunes < 5)
                    StartCoroutine(ShowPlotInfoPanel("Znaleziono runę", "Zebrałeś " + runesManager.CollectedRunes + " runy"));
                else StartCoroutine(ShowPlotInfoPanel("Znaleziono runę", "Zebrałeś " + runesManager.CollectedRunes + " run"));
            }
            // choosen language is english:
            else
            {
                if (runesManager.CollectedRunes > 1) StartCoroutine(ShowPlotInfoPanel("Rune found", "You have collected " + runesManager.CollectedRunes + " runes"));
                else StartCoroutine(ShowPlotInfoPanel("Rune found", "You have collected " + runesManager.CollectedRunes + " rune"));
            }
        }

        /// <summary>
        /// Actions after activating a rune.
        /// </summary>
        public void ActivatedRune()
        {
            activatedRunes++;
            // if player activated all runes, uptade current goal:
            if (activatedRunes >= runesManager.RunesAmount) UpdatePlotGoal(GetComponent<PlotManager>().LastGoal);
        }

        /// <summary>
        /// Searched a room.
        /// </summary>
        public void SearchedRoom()
        {
            searchedRooms++;
        }

        /// <summary>
        /// Fades out the camera after given delay.
        /// </summary>
        /// <param name="delay">Time delay</param>
        /// <returns></returns>
        public IEnumerator FadeOutCamera(float delay)
        {
            yield return new WaitForSeconds(delay);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().farClipPlane = 0.2f;
        }

        /// <summary>
        /// Used a light switch.
        /// </summary>
        public void UsedLightSwitch()
        {
            lightSwitchUses++;
        }

        /// <summary>
        /// Updates current plot goal.
        /// </summary>
        /// <param name="newGoal">The new goal</param>
        public void UpdatePlotGoal(Goal newGoal)
        {
            //UnityEngine.Debug.Log("New goal content: " + newGoal.Content + " (weight: " + newGoal.Weight + ")");
            if (newGoal.Weight > currentGoal.Weight)
            {
                currentGoal = newGoal;
                // show update info:
                StopAllCoroutines();
                // choosen language is polish:
                if (GameManager.instance.ChoosenLanguage == GameLanguage.Polish) StartCoroutine(ShowPlotInfoPanel("Aktualizacja celu", currentGoal.Content));
                // choosen language is english:
                else StartCoroutine(ShowPlotInfoPanel("Goal update", currentGoal.Content));
                // if newGoal is the last goal, activate rune orbs:
                if (CurrentGoalIsTheOrbGoal) runesManager.ActivateOrbs();
            }
        }

        /// <summary>
        /// Shows current plot goal.
        /// </summary>
        public void ShowCurrentGoal()
        {
            StopAllCoroutines();
            // choosen language is polish:
            if (GameManager.instance.ChoosenLanguage == GameLanguage.Polish) StartCoroutine(ShowPlotInfoPanel("Cel", currentGoal.Content));
            // choosen language is english:
            else StartCoroutine(ShowPlotInfoPanel("Goal", currentGoal.Content));
        }

        /// <summary>
        /// Shows and fades out plot info panel.
        /// </summary>
        /// <param name="headline">The headline text</param>
        /// <param name="content">The content text</param>
        /// <returns></returns>
        public IEnumerator ShowPlotInfoPanel(string headline, string content)
        {
            goalUpdateHeadlineText.text = headline;
            goalUpdateContentText.text = content;
            goalUpdatePanel.GetComponent<CanvasGroup>().alpha = 1.0f;
            goalUpdatePanel.SetActive(true);
            yield return new WaitForSeconds(2.0f);
            // start fading:
            float elapsedTime = 0f;
            while (goalUpdatePanel.GetComponent<CanvasGroup>().alpha > 0)
            {
                elapsedTime += Time.deltaTime;
                goalUpdatePanel.GetComponent<CanvasGroup>().alpha = Mathf.Clamp01(1.0f - (elapsedTime / 2.0f));
                yield return null;
            }
            goalUpdatePanel.SetActive(false);
            yield return null;
        }

        /// <summary>
        /// Last tasks before switching to next level.
        /// </summary>
        public void EndLevel()
        {
            stopwatch.Stop();
            if (GameManager.instance.AnalyticsEnabled)
            {
                DataManager.AddGameEvent(Analytics.EventType.GameEnd, stopwatch.Elapsed);
            }
            // save achievements progress:
            GameManager.instance.GameTime = stopwatch.Elapsed;
            GameManager.instance.CollectedRunes = runesManager.CollectedRunes;
            GameManager.instance.RunesAmount = runesManager.RunesAmount;
            GameManager.instance.SearchedRooms = searchedRooms;
            GameManager.instance.LightSwitchUses = lightSwitchUses;
            // inform game manager that level has ended:
            GameManager.instance.LoadNextLevel();
        }

        /// <summary>
        /// Saves game event with current HR and GSR readings.
        /// </summary>
        /// <param name="eventType">Type of the event</param>
        /// <param name="value">Additional event object value</param>
        public void AddGameEvent(Analytics.EventType eventType, object value = null)
        {
            currentTime = stopwatch.Elapsed;
            DataManager.AddGameEvent(eventType, currentTime, value);
            if (GameManager.instance.BBModule.IsBandPaired) AddBiofeedbackEvents(currentTime);
        }

        /// <summary>
        /// Saves biofeedback info: current HR, GSR and Arousal modifier.
        /// </summary>
        /// <param name="value">Time when event occured</param>
        private void AddBiofeedbackEvents(object time = null)
        {
            if (time != null) currentTime = (TimeSpan)time;
            else currentTime = stopwatch.Elapsed;
            DataManager.AddGameEvent(Analytics.EventType.HrData, currentTime, GameManager.instance.BBModule.CurrentHr);
            DataManager.AddGameEvent(Analytics.EventType.GsrData, currentTime, GameManager.instance.BBModule.CurrentGsr);
            DataManager.AddGameEvent(Analytics.EventType.ArousalData, currentTime, GameManager.instance.BBModule.ArousalModifier);
        }
        #endregion
    }
}
