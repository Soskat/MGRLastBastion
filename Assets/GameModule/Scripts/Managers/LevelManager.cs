using LastBastion.Game.Plot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        // test:
        [SerializeField] private Text runesText;
        [SerializeField] private Text goalText;
        #endregion


        #region Public fields & properties
        /// <summary>Fixed max amount of the runes that player can find in this level.</summary>
        public int MaxRunesAmount { get { return maxRunesAmount; } }
        /// <summary>Current plot goal.</summary>
        public PlotGoal CurrentGoal { get { return currentGoal; } }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            GameManager.instance.LevelManager = this;
            //currentGoal = GetComponent<PlotManager>().Init();

            // test - update GUI:
            runesText.text = maxRunesAmount.ToString();
            //goalText.text = currentGoal.Goal.GoalContent;
        }

        // Update is called once per frame
        void Update()
        {

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
            }
        }
        #endregion
    }
}
