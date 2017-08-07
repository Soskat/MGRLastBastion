using System;
using System.Collections.Generic;


namespace LastBastion.Game.Plot
{
    /// <summary>
    /// Class that represents a plot goals set.
    /// </summary>
    [Serializable]
    public class GoalsSet
    {
        #region Public fields
        /// <summary>The list of goals.</summary>
        public List<Goal> Goals;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of <see cref="GoalsSet"/> class.
        /// </summary>
        public GoalsSet() : this(new List<Goal>()) { }

        /// <summary>
        /// Creates an instance of <see cref="GoalsSet"/> class.
        /// </summary>
        /// <param name="goals">List of the goals to create a set</param>
        public GoalsSet(List<Goal> goals)
        {
            Goals = goals;
        }
        #endregion
    }
}
