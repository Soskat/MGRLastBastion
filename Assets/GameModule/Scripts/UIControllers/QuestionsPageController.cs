using LastBastion.Game.Managers;
using LastBastion.Game.SurveySystem;
using System.Collections;
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
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [SerializeField] private List<float> yPositions;
        [SerializeField] private List<QuestionPanelController> questions;
        private int questionSpaceUnits = 7;                                 // determines how many questions can be displayed on page
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            nextButton.onClick.AddListener(() => { GetComponentInParent<QuestionnairePanelController>().NextPage(); });
            backButton.onClick.AddListener(() => { GetComponentInParent<QuestionnairePanelController>().PreviousPage(); });
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Private methods
        private void AddQuestionPanel(Question question, GameObject interactionPanel, int yPositionIndex)
        {
            GameObject questionPanel;
            // question panel has got a dropdown menu -> closed question:
            if (interactionPanel != null)
            {
                questionPanel = Instantiate(Resources.Load("UIElements/QuestionPanel") as GameObject, transform);
                questionPanel.GetComponent<QuestionPanelController>().UpdatePanel(question, interactionPanel);
                questionPanel.GetComponent<RectTransform>().position.Set(0, yPositions[yPositionIndex], 0);
            }
            // question panel hasn't got a dropdown menu -> open question:
            else
            {
                questionPanel = Instantiate(Resources.Load("UIElements/QuestionPanel_Open") as GameObject, transform);
                questionPanel.GetComponent<QuestionPanelController>().UpdatePanel(question);
                questionPanel.GetComponent<RectTransform>().position.Set(0, yPositions[yPositionIndex] = 40, 0);
            }
            questions.Add(questionPanel.GetComponent<QuestionPanelController>());

            Debug.Log("-- added a question");
        }
        #endregion


        #region Public methods
        public int CreateQuestionsPage(int currentQuestion)
        {
            while (questionSpaceUnits > 0 && currentQuestion < GameManager.instance.Survey.Questions.Count)
            {
                // choose dropdown menu based on AnswerType:
                GameObject interactionPanel = null;
                int yPositionIndex = 0;
                switch (GameManager.instance.Survey.Questions[currentQuestion].AnswerType)
                {
                    case QuestionType.Age:
                        interactionPanel = Instantiate(Resources.Load("UIElements/Dropdown_Age") as GameObject);
                        yPositionIndex = yPositions.Count - questionSpaceUnits;
                        questionSpaceUnits -= 1;
                        break;

                    case QuestionType.AorB:
                        interactionPanel = Instantiate(Resources.Load("UIElements/Dropdown_AorB") as GameObject);
                        yPositionIndex = yPositions.Count - questionSpaceUnits;
                        questionSpaceUnits -= 1;
                        break;

                    case QuestionType.Open:
                        // that question panel hasn't got dropdown menu, so there no need in assigning interactionPanel object
                        yPositionIndex = yPositions.Count - questionSpaceUnits;
                        questionSpaceUnits -= 3;
                        break;

                    case QuestionType.PlayRoutine:
                        interactionPanel = Instantiate(Resources.Load("UIElements/Dropdown_PlayRoutine") as GameObject);
                        yPositionIndex = yPositions.Count - questionSpaceUnits;
                        questionSpaceUnits -= 1;
                        break;

                    case QuestionType.Scale:
                        interactionPanel = Instantiate(Resources.Load("UIElements/Dropdown_Scale") as GameObject);
                        yPositionIndex = yPositions.Count - questionSpaceUnits;
                        questionSpaceUnits -= 1;
                        break;

                    case QuestionType.Sex:
                        interactionPanel = Instantiate(Resources.Load("UIElements/Dropdown_Sex") as GameObject);
                        yPositionIndex = yPositions.Count - questionSpaceUnits;
                        questionSpaceUnits -= 1;
                        break;

                    case QuestionType.TrueFalse:
                        interactionPanel = Instantiate(Resources.Load("UIElements/Dropdown_TrueFalse") as GameObject);
                        yPositionIndex = yPositions.Count - questionSpaceUnits;
                        questionSpaceUnits -= 1;
                        break;
                }
                // create question panel:
                AddQuestionPanel(GameManager.instance.Survey.Questions[currentQuestion], interactionPanel, yPositionIndex);
                currentQuestion++;
            }
            return currentQuestion;
        }
        #endregion
    }
}
