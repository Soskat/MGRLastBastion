using LastBastion.Game.Managers;
using System;
using UnityEngine;


namespace LastBastion.Game.Plot
{
    /// <summary>
    /// Component taht represents single plot goal.
    /// </summary>
    public class PlotGoal : MonoBehaviour
    {
        #region Private fields
        /// <summary>Assigned <see cref="Goal"/> component.</summary>
        [SerializeField] private Goal goal;
        /// <summary>Does this plot goal need previous goal to be triggered?</summary>
        [SerializeField] private bool needPreviousGoal = false;
        /// <summary>Is this plot goal a trigger?</summary>
        [SerializeField] private bool isTrigger = false;
        /// <summary>Has plot goal been activated?</summary>
        private bool wasActivated;
        #endregion


        #region Public fields & properties
        /// <summary>The plot goal.</summary>
        public Goal Goal { get { return goal; } }
        /// <summary>Informs that trigger has been activated.</summary>
        public Action Triggered { get; set; }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        protected void Start()
        {
            wasActivated = false;
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Updates the goal.
        /// </summary>
        /// <param name="newGoal">New goal data</param>
        public void UpdateGoal(Goal newGoal)
        {
            goal = newGoal;
        }

        /// <summary>
        /// Activates the plot goal.
        /// </summary>
        public void Activate()
        {
            // if goal requires the previous plot goal to be active, check if this condition is fullfilled:
            if (needPreviousGoal && (LevelManager.instance.CurrentGoal.Weight + 1) != goal.Weight) return;
            // activate the goal:
            if (!wasActivated)
            {
                LevelManager.instance.UpdatePlotGoal(goal);
                wasActivated = true;
                // if plot goal is a trigger, inform that it has been triggered:
                if (isTrigger) Triggered();
            }
        }
        #endregion
    }
}