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
        #endregion


        #region Public fields & properties
        /// <summary>The plot goal.</summary>
        public Goal Goal { get { return goal; } }
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
        #endregion
    }
}
