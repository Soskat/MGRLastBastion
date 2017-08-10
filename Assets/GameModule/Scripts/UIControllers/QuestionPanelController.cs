using LastBastion.Game.SurveySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace LastBastion.Game.UIControllers
{
    /// <summary>
    /// Component that manages behaviour of QuestionPanel game object.
    /// </summary>
    public class QuestionPanelController : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private QuestionPanelType questionPanelType;
        [SerializeField] private Text questionContent;
        [SerializeField] private GameObject answerHolderObject;
        private GameObject answerHolder;
        private Question question;
        #endregion


        #region Public fields & properties
        /// <summary>The question assigned to the question panel.</summary>
        public Question Question { get { return question; } }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(questionContent);
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Updates contents the question panel.
        /// </summary>
        /// <param name="_question">The question</param>
        /// <param name="dropdown">Dropdown menu prefab</param>
        public void UpdatePanel(Question _question, GameObject dropdown = null)
        {
            question = _question;
            questionContent.text = question.Content;
            if(questionPanelType == QuestionPanelType.Closed && dropdown != null && answerHolderObject != null)
            {
                answerHolder = Instantiate(dropdown, transform);
                answerHolder.GetComponent<RectTransform>().localPosition = answerHolderObject.GetComponent<RectTransform>().localPosition;
                answerHolder.GetComponent<RectTransform>().sizeDelta = answerHolderObject.GetComponent<RectTransform>().sizeDelta;
            }
            else if (questionPanelType == QuestionPanelType.Closed)
            {
                answerHolder = GetComponentInChildren<InputField>().gameObject;
            }
        }

        /// <summary>
        /// Saves the answer.
        /// </summary>
        public void SaveAnswer()
        {
            switch (questionPanelType)
            {
                case QuestionPanelType.Closed:
                    // value - 1 because first option is always empty:
                    question.Answer = (answerHolder.GetComponent<Dropdown>().value - 1).ToString();
                    break;

                case QuestionPanelType.Open:
                    question.Answer = answerHolder.GetComponent<InputField>().text;
                    break;
            }
        }
        #endregion
    }
}
