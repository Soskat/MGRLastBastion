using LastBastion.Game.Managers;
using LastBastion.Game.SurveySystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LastBastion.Game.UIControllers
{
    /// <summary>
    /// Component that manages behaviour of QuestionPage game object.
    /// </summary>
    public class QuestionsPageController : MonoBehaviour
    {
        #region Private fields
        /// <summary>'Next page' button.</summary>
        [SerializeField] private Button nextButton;
        /// <summary>'Previous page' button.</summary>
        [SerializeField] private Button backButton;
        /// <summary>List of y positions of the question panels.</summary>
        [SerializeField] private List<float> yPositions;
        /// <summary>List of assigned question panels.</summary>
        [SerializeField] private List<QuestionPanelController> questions;
        /// <summary>Determines how many questions can be displayed on page.</summary>
        private int questionSpaceUnits = 7;
        #endregion


        #region Public fields & properties
        /// <summary>'Next page' button.</summary>
        public GameObject NextButton { get { return nextButton.gameObject; } }
        /// <summary>'Back page' button.</summary>
        public GameObject BackButton { get { return backButton.gameObject; } }
        /// <summary>Questions assigned to this page.</summary>
        public List<QuestionPanelController> Questions { get { return questions; } }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            nextButton.onClick.AddListener(() => { GetComponentInParent<QuestionnairePanelController>().NextPage(); });
            backButton.onClick.AddListener(() => { GetComponentInParent<QuestionnairePanelController>().PreviousPage(); });
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Adds new question panel to questions page.
        /// </summary>
        /// <param name="question">Question data</param>
        /// <param name="yPositionIndex">Index of element in QuestionsPageController.yPositions list</param>
        private void AddQuestionPanel(Question question, int yPositionIndex)
        {
            GameObject questionPanel;
            // question panel hasn't got a dropdown menu -> open question:
            if (question.AnswerType == QuestionType.Open)
            {
                questionPanel = Instantiate(Resources.Load("UIElements/QuestionPanel_Open") as GameObject, GetComponent<RectTransform>().transform);
                questionPanel.GetComponent<QuestionPanelController>().UpdatePanel(question);
                questionPanel.GetComponent<RectTransform>().localPosition.Set(0, yPositions[yPositionIndex] = 40, 0);
            }
            // question panel has got a dropdown menu -> closed question:
            else
            {
                questionPanel = Instantiate(Resources.Load("UIElements/QuestionPanel") as GameObject, GetComponent<RectTransform>().transform);
                questionPanel.GetComponent<QuestionPanelController>().UpdatePanel(question);
                questionPanel.GetComponent<RectTransform>().localPosition = new Vector2(0, questionPanel.GetComponent<RectTransform>().localPosition.y + yPositions[yPositionIndex]);
            }
            questions.Add(questionPanel.GetComponent<QuestionPanelController>());
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Creates questions page filled with specific range of survey questions.
        /// </summary>
        /// <param name="currentQuestion">Index of element of survey questions list to start with</param>
        /// <returns>Current index of element of survey questions list</returns>
        public int CreateQuestionsPage(int currentQuestion)
        {
            while (questionSpaceUnits > 0 && currentQuestion < GameManager.instance.SurveyManager.Survey.Questions.Count)
            {
                // choose dropdown menu based on AnswerType:
                int yPositionIndex = yPositions.Count - questionSpaceUnits;
                int questionSpaceCost = 0;
                if (GameManager.instance.SurveyManager.Survey.Questions[currentQuestion].AnswerType == QuestionType.Open) questionSpaceCost = 3;
                else questionSpaceCost = 1;

                // if there's enough questionSpaceUnit for next question, continue:
                if (questionSpaceUnits >= questionSpaceCost)
                {
                    // create question panel:
                    AddQuestionPanel(GameManager.instance.SurveyManager.Survey.Questions[currentQuestion], yPositionIndex);
                    currentQuestion++;
                }
                questionSpaceUnits -= questionSpaceCost;
            }
            return currentQuestion;
        }
        #endregion
    }
}
