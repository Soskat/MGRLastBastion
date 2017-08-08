using LastBastion.Game.Plot;
using System.Collections;
using System.Collections.Generic;
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
        //[SerializeField] private int collectedRunes = 0;
        [SerializeField] private int maxRunesAmount = 0;
        [SerializeField] private PlotGoal currentGoal;
        [SerializeField] private GameObject goalUpdatePanel;
        [SerializeField] private Text goalUpdateHeadlineText;
        [SerializeField] private Text goalUpdateContentText;

        // test:
        [SerializeField] private Text runesText;
        [SerializeField] private Text goalText;
        private int goalCount;
        #endregion


        #region Public fields & properties
        /// <summary>Fixed max amount of the runes that player can find in this level.</summary>
        public int MaxRunesAmount { get { return maxRunesAmount; } }
        /// <summary>Current plot goal.</summary>
        public PlotGoal CurrentGoal { get { return currentGoal; } }
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
            goalUpdatePanel.SetActive(false);
            //currentGoal = GetComponent<PlotManager>().Init();

            // test - update GUI:
            runesText.text = maxRunesAmount.ToString();
            //goalText.text = currentGoal.Goal.GoalContent;

            goalCount = 0;
            //StartCoroutine(ShowUpdatedGoal("Goal update", "goal #" + goalCount));
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                goalCount++;
                StopAllCoroutines();
                StartCoroutine(ShowPlotInfoPanel("Goal update", "goal #" + goalCount));
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StopAllCoroutines();
                StartCoroutine(ShowPlotInfoPanel("Goal", "goal #" + goalCount));
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
            maxRunesAmount++;
            // test - update GUI:
            runesText.text = maxRunesAmount.ToString();

            StopAllCoroutines();
            if (maxRunesAmount > 1) StartCoroutine(ShowPlotInfoPanel("Rune found", "You have collected " + maxRunesAmount + " runes"));
            else StartCoroutine(ShowPlotInfoPanel("Rune found", "You have collected " + maxRunesAmount + " rune"));
        }

        /// <summary>
        /// Updates current plot goal.
        /// </summary>
        /// <param name="newGoal">The new goal</param>
        public void UpdatePlotGoal(PlotGoal newGoal)
        {
            if (newGoal.Goal.GoalWeight > currentGoal.Goal.GoalWeight)
            {
                currentGoal = newGoal;
                // test - update GUI:
                //goalText.text = currentGoal.Goal.GoalContent;
                StopAllCoroutines();
                StartCoroutine(ShowPlotInfoPanel("Goal update", "goal #" + goalCount));
            }
        }

        /// <summary>
        /// Shows current plot goal.
        /// </summary>
        public void ShowCurrentGoal()
        {
            StopAllCoroutines();
            StartCoroutine(ShowPlotInfoPanel("Goal", "goal #" + goalCount));
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
    }
}
