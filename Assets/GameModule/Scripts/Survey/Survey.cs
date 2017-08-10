using System;
using System.Collections.Generic;


namespace LastBastion.Game.SurveySystem
{
    /// <summary>
    /// Represents a survey questionnaire.
    /// </summary>
    [Serializable]
    public class Survey
    {
        #region Public fields
        /// <summary>Questions about the player.</summary>
        public List<Question> Metrics;
        /// <summary>Questions about the game.</summary>
        public List<Question> GameEvaluation;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of class <see cref="Survey"/>.
        /// </summary>
        public Survey()
        {
            Metrics = new List<Question>();
            GameEvaluation = new List<Question>();
        }
        #endregion
    }
}
