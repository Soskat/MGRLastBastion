using System;
using System.Collections.Generic;


namespace LastBastion.Game.Plot
{
    /// <summary>
    /// Class that represents a plot goals set.
    /// </summary>
    [Serializable]
    public class Goals
    {
        #region Public fields
        /// <summary>The set of goals.</summary>
        public List<Goal> Set;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of <see cref="Goals"/> class.
        /// </summary>
        public Goals() : this(new List<Goal>()) { }

        /// <summary>
        /// Creates an instance of <see cref="Goals"/> class.
        /// </summary>
        /// <param name="goals">List of the goals to create a set</param>
        public Goals(List<Goal> goals)
        {
            Set = goals;
        }
        #endregion
    }
}
