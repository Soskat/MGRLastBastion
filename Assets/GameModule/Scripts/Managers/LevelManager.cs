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
        [SerializeField] private string sceneName;
        [SerializeField] private int runesLimit = 0;
        [SerializeField] private Goal currentGoal;
        [SerializeField] private GameObject goalUpdatePanel;
        [SerializeField] private Text goalUpdateHeadlineText;
        [SerializeField] private Text goalUpdateContentText;
        // achievements counters:
        [SerializeField] private int collectedRunes = 0;
        [SerializeField] private int openedDoors = 0;
        [SerializeField] private int lightSwitchUses = 0;
        private Stopwatch stopwatch;
        private TimeSpan currentTime;
        private GameObject player;
        private BiofeedbackAudioManager playerBiofeedback;
        #endregion

        
        #region Public fields & properties
        /// <summary>Fixed max amount of the runes that player can find in this level.</summary>
        public int RunesLimit { get { return runesLimit; } }
        /// <summary>Current plot goal.</summary>
        public Goal CurrentGoal { get { return currentGoal; } }
        /// <summary>Reference to player game object.</summary>
        public GameObject Player { get { return player; } }
        /// <summary>Reference to player's BiofeedbackController component.</summary>
        public BiofeedbackAudioManager PlayerBiofeedback { get { return playerBiofeedback; } }
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
                playerBiofeedback = player.GetComponent<BiofeedbackAudioManager>();
                // make some assertions:
                Assert.IsNotNull(goalUpdatePanel);
                Assert.IsNotNull(goalUpdateHeadlineText);
                Assert.IsNotNull(goalUpdateContentText);
            }
            else if (instance != this) Destroy(gameObject);
        }

        // Use this for initialization
        void Start()
        {
            goalUpdatePanel.SetActive(false);
            currentGoal = GetComponent<PlotManager>().Init();
            // reset achievements:
            collectedRunes = 0;
            openedDoors = 0;
            lightSwitchUses = 0;
            // start stopwatch:
            stopwatch = new Stopwatch();
            stopwatch.Start();
            
            // save level info:
            if (GameManager.instance.AnalyticsEnabled)
            {
                DataManager.AddLevelInfo(sceneName, GameManager.instance.CurrentCalculationType, GameManager.instance.BBModule.AverageHr, GameManager.instance.BBModule.AverageGsr);
                DataManager.AddGameEvent(Analytics.EventType.GameStart, stopwatch.Elapsed);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // read input:
            if (!Player.GetComponent<InteractionController>().IsFocused && Input.GetKeyDown(KeyCode.Q)) ShowCurrentGoal();

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
        /// Actions after finding a rune.
        /// </summary>
        public void FoundRune()
        {
            // update runes count:
            collectedRunes++;
            // show update info:
            StopAllCoroutines();
            if (collectedRunes > 1) StartCoroutine(ShowPlotInfoPanel("Rune found", "You have collected " + collectedRunes + " runes"));
            else StartCoroutine(ShowPlotInfoPanel("Rune found", "You have collected " + collectedRunes + " rune"));
        }

        /// <summary>
        /// Opened a door.
        /// </summary>
        public void OpenedDoor()
        {
            openedDoors++;
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
            if (newGoal.Weight > currentGoal.Weight)
            {
                currentGoal = newGoal;
                // show update info:
                StopAllCoroutines();
                StartCoroutine(ShowPlotInfoPanel("Goal update", currentGoal.Content));
            }
        }

        /// <summary>
        /// Shows current plot goal.
        /// </summary>
        public void ShowCurrentGoal()
        {
            StopAllCoroutines();
            StartCoroutine(ShowPlotInfoPanel("Goal", currentGoal.Content));
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
            GameManager.instance.CollectedRunes = collectedRunes;
            GameManager.instance.OpenedDoors = openedDoors;
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
            AddBiofeedbackEvents(currentTime);
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
