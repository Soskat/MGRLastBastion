using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Summary scene behaviours.
    /// </summary>
    public class SummaryManager : MonoBehaviour
    {
        #region Private fields
        /// <summary>Button object that ends current scene.</summary>
        [SerializeField] private Button endSceneButton;
        /// <summary>Button object that returns to main menu scene.</summary>
        [SerializeField] private Button backToMainMenuButton;
        /// <summary>Timer label text.</summary>
        [SerializeField] private Text timerText;
        /// <summary>Timer label text.</summary>
        [SerializeField] private Text nextLevelText;
        /// <summary>Achievement panel object with content in english.</summary>
        [SerializeField] private GameObject achievementPanelENG;
        /// <summary>Achievement panel object with content in polish.</summary>
        [SerializeField] private GameObject achievementPanelPL;
        /// <summary>Cooldown time.</summary>
        [SerializeField] private int secondsToGo = 120;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(timerText);
            Assert.IsNotNull(nextLevelText);
            Assert.IsNotNull(achievementPanelENG);
            Assert.IsNotNull(achievementPanelPL);
        }

        // Use this for initialization
        void Start()
        {
            // update buttons behaviour:
            if (GameManager.instance.DebugMode)
            {
                endSceneButton.onClick.AddListener(() => { GameManager.instance.LoadNextLevel(); });
                endSceneButton.gameObject.SetActive(true);
                backToMainMenuButton.onClick.AddListener(() => { GameManager.instance.BackToMainMenu(); });
                backToMainMenuButton.gameObject.SetActive(true);
            }
            else
            {
                endSceneButton.gameObject.SetActive(false);
                backToMainMenuButton.gameObject.SetActive(false);
            }

            // update GUI based on chosen game language:
            if (GameManager.instance.ChosenLanguage == GameLanguage.Polish)
            {
                achievementPanelENG.SetActive(false);
                achievementPanelPL.SetActive(true);
            }
            else
            {
                achievementPanelENG.SetActive(true);
                achievementPanelPL.SetActive(false);
            }

            // unlock cursor state after game level:
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // run stopwatch:
            StartCoroutine(Stopwatch());
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Simple coroutine that simulates timer.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Stopwatch()
        {
            int minutes;
            int seconds;
            for (int i = secondsToGo; i >= 0; i--)
            {
                minutes = i / 60;
                seconds = i - (minutes * 60);
                timerText.text = String.Format("{0:00}:{1:00}", minutes, seconds);
                yield return new WaitForSeconds(1.0f);
            }
            nextLevelText.text = "( Loading next scene )";
            timerText.gameObject.SetActive(false);
            // inform that level has ended:
            GameManager.instance.LoadNextLevel();
        }
        #endregion
    }
}