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
        public void UpdatePanel(Question _question)
        {
            question = _question;
            questionContent.text = question.Content;
            // open question:
            if (question.AnswerType == QuestionType.Open) answerHolder = GetComponentInChildren<InputField>().gameObject;
            // closed question:
            else if(answerHolderObject != null)
            {
                switch (question.AnswerType)
                {
                    case QuestionType.Age:
                        answerHolder = Instantiate(Resources.Load("UIElements/Dropdown_Age") as GameObject, answerHolderObject.GetComponent<RectTransform>().transform);
                        break;

                    case QuestionType.AorB:
                        answerHolder = Instantiate(Resources.Load("UIElements/Dropdown_AorB") as GameObject, answerHolderObject.GetComponent<RectTransform>().transform);
                        break;

                    case QuestionType.PlayRoutine:
                        answerHolder = Instantiate(Resources.Load("UIElements/Dropdown_PlayRoutine") as GameObject, answerHolderObject.GetComponent<RectTransform>().transform);
                        break;

                    case QuestionType.Scale:
                        answerHolder = Instantiate(Resources.Load("UIElements/Dropdown_Scale") as GameObject, answerHolderObject.GetComponent<RectTransform>().transform);
                        break;

                    case QuestionType.Sex:
                        answerHolder = Instantiate(Resources.Load("UIElements/Dropdown_Sex") as GameObject, answerHolderObject.GetComponent<RectTransform>().transform);
                        break;

                    case QuestionType.TrueFalse:
                        answerHolder = Instantiate(Resources.Load("UIElements/Dropdown_TrueFalse") as GameObject, answerHolderObject.GetComponent<RectTransform>().transform);
                        break;

                    default: break;
                }
                answerHolder.GetComponent<Dropdown>().onValueChanged.AddListener(
                    delegate { GetComponentInParent<QuestionnairePanelController>().OnDropdonwValueChanged(answerHolder.GetComponent<Dropdown>()); }
                );
            }
        }

        /// <summary>
        /// Saves the answer.
        /// </summary>
        public void SaveAnswer()
        {
            if (question.AnswerType == QuestionType.Open) question.Answer = answerHolder.GetComponent<InputField>().text;
            // value - 1 because first option is always empty:
            else question.Answer = (answerHolder.GetComponent<Dropdown>().value - 1).ToString();
        }
        #endregion
    }
}
