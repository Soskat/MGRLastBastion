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
        /// <summary>List of questions.</summary>
        public List<Question> Questions;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of class <see cref="Survey"/>.
        /// </summary>
        public Survey() : this(new List<Question>()) { }

        /// <summary>
        /// Creates an instance of class <see cref="Survey"/>.
        /// </summary>
        /// <param name="questions">The list of questions</param>
        public Survey(List<Question> questions)
        {
            Questions = questions;
        }
        #endregion
    }
}
