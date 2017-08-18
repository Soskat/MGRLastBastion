using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.Plot
{
    /// <summary>
    /// Component taht represents single plot goal.
    /// </summary>
    public class PlotGoal : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Goal goal;
        private bool wasActivated;
        #endregion


        #region Public fields & properties
        /// <summary>The plot goal.</summary>
        public Goal Goal { get { return goal; } }
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
            if (!wasActivated)
            {
                LevelManager.instance.UpdatePlotGoal(goal);
                wasActivated = true;
            }
        }
        #endregion
    }
}
