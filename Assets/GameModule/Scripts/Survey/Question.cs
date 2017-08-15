using System;


namespace LastBastion.Game.SurveySystem
{
    /// <summary>
    /// Represents a single question from a survey questionnaire.
    /// </summary>
    [Serializable]
    public class Question
    {
        #region Public fields
        /// <summary>ID of the question.</summary>
        public int ID;
        /// <summary>Content of the question.</summary>
        public string Content;
        /// <summary>The type of the answer.</summary>
        public QuestionType AnswerType;
        /// <summary>The answer to the question.</summary>
        public string Answer;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of class <see cref="Question"/>.
        /// </summary>
        public Question() : this(-1, "Tee or coffe", QuestionType.TrueFalse) { }

        /// <summary>
        /// Creates an instance of class <see cref="Question"/>.
        /// </summary>
        /// <param name="id">ID of the question</param>
        /// <param name="content">Content of the question</param>
        /// <param name="answerType">The type of the answer</param>
        public Question(int id, string content, QuestionType answerType)
        {
            ID = id;
            Content = content;
            AnswerType = answerType;
        }
        #endregion
    }
}
