﻿using LastBastion.Game.Plot;
using System;
using System.Collections;
using System.Collections.Generic;
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
        #region Private fields
        [SerializeField] private string sceneName;
        [SerializeField] private int runesLimit = 0;
        [SerializeField] private Goal currentGoal;
        [SerializeField] private GameObject goalUpdatePanel;
        [SerializeField] private Text goalUpdateHeadlineText;
        [SerializeField] private Text goalUpdateContentText;
        // achievements counters:
        [SerializeField] private TimeSpan gameTime;
        [SerializeField] private int collectedRunes = 0;
        [SerializeField] private int openedDoors = 0;
        [SerializeField] private int lightSwitchUses = 0;
        private Stopwatch stopwatch;
        #endregion


        #region Public fields & properties
        /// <summary>Fixed max amount of the runes that player can find in this level.</summary>
        public int RunesLimit { get { return runesLimit; } }
        /// <summary>Current plot goal.</summary>
        public Goal CurrentGoal { get { return currentGoal; } }
        /// <summary>Time of the game.</summary>
        public TimeSpan GameTime { get { return gameTime; } }
        /// <summary>Collected runes.</summary>
        public int CollectedRunes { get { return collectedRunes; } }
        /// <summary>Opened doors count.</summary>
        public int OpenedDoors { get { return openedDoors; } }
        /// <summary>Uses of the lightswitches count.</summary>
        public int LightSwitchUses { get { return lightSwitchUses; } }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(goalUpdatePanel);
            Assert.IsNotNull(goalUpdateHeadlineText);
            Assert.IsNotNull(goalUpdateContentText);
        }

        // Use this for initialization
        void Start()
        {
            GameManager.instance.LevelManager = this;
            GameManager.instance.SetupPlayerSettings();
            goalUpdatePanel.SetActive(false);
            currentGoal = GetComponent<PlotManager>().Init();
            // reset achievements:
            collectedRunes = 0;
            openedDoors = 0;
            lightSwitchUses = 0;
            // start stopwatch:
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ShowCurrentGoal();
            }


            // biofeedback readings update
            // ...
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
        #endregion


        // test -----------------
        public void EndLevel()
        {
            stopwatch.Stop();
            gameTime = stopwatch.Elapsed;
            GameManager.instance.LevelHasEnded();
        }
    }
}
