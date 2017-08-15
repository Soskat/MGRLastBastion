using System;


namespace LastBastion.Game.Plot
{
    /// <summary>
    /// Class that represents a plot goal.
    /// </summary>
    [Serializable]
    public class Goal
    {
        #region Public fields
        /// <summary>The weight of the goal.</summary>
        public int Weight;
        /// <summary>The content of the goal.</summary>
        public string Content;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of <see cref="Goal"/> class.
        /// </summary>
        public Goal() : this(-1, "<empty>") { }

        /// <summary>
        /// Creates an instance of <see cref="Goal"/> class.
        /// </summary>
        /// <param name="weight">Weight of the goal</param>
        /// <param name="content">Content of the goal</param>
        public Goal(int weight, string content)
        {
            Weight = weight;
            Content = content;
        }
        #endregion
    }
}
