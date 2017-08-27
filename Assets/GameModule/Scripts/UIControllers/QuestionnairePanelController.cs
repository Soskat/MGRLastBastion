using LastBastion.Game.Managers;
using LastBastion.Game.SurveySystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LastBastion.Game.UIControllers
{
    /// <summary>
    /// Component that manages behaviour of QuestionnairePanel game object.
    /// </summary>
    public class QuestionnairePanelController : MonoBehaviour
    {
        #region Private fields
        /// <summary>List of pages with questions.</summary>
        [SerializeField] private List<QuestionsPageController> pages;
        /// <summary>Amount of given answers.</summary>
        [SerializeField] private int givenAnswers = 0;
        /// <summary>Index of current page.</summary>
        private int currentPageIndex;
        #endregion


        #region Public fields & properties
        /// <summary>Amount of given answers.</summary>
        public int GivenAnswers { get { return givenAnswers; } }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            // create questionnaire:
            CreateQuestionnaire();
            // update pages visibility:
            currentPageIndex = 0;
            pages[0].gameObject.SetActive(true);
            for(int i = 1; i < pages.Count; i++) pages[i].gameObject.SetActive(false);
            // turn off the first BackButton and the last NextButton:
            pages[0].BackButton.SetActive(false);
            pages[pages.Count - 1].NextButton.SetActive(false);
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Creates the survey questionnaire.
        /// </summary>
        private void CreateQuestionnaire()
        {
            int questionsCreated = 0;
            while(questionsCreated < GameManager.instance.SurveyManager.Survey.Questions.Count)
            {
                // create new questions page and add it to transform children:
                GameObject page = Instantiate(Resources.Load("UIElements/QuestionsPage") as GameObject);
                page.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>().transform);
                page.GetComponent<RectTransform>().localPosition = Vector2.zero;
                page.GetComponent<RectTransform>().localScale = Vector3.one;
                pages.Add(page.GetComponent<QuestionsPageController>());
                // create questions in the new page:
                questionsCreated = page.GetComponent<QuestionsPageController>().CreateQuestionsPage(questionsCreated);
            }
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Go to next questions page.
        /// </summary>
        public void NextPage()
        {
            pages[currentPageIndex].gameObject.SetActive(false);
            currentPageIndex = (++currentPageIndex >= pages.Count) ? pages.Count - 1 : currentPageIndex;
            pages[currentPageIndex].gameObject.SetActive(true);
        }

        /// <summary>
        /// Go to previous questions page.
        /// </summary>
        public void PreviousPage()
        {
            pages[currentPageIndex].gameObject.SetActive(false);
            currentPageIndex = (--currentPageIndex < 0) ? 0 : currentPageIndex;
            pages[currentPageIndex].gameObject.SetActive(true);
        }

        /// <summary>
        /// Prevents from switching to empty option from dropdown menu.
        /// </summary>
        /// <param name="dropdown">The dropdown menu</param>
        public void OnDropdonwValueChanged(Dropdown dropdown)
        {
            if (dropdown.options[0].text == "")
            {
                dropdown.options.RemoveAt(0);
                dropdown.value -= 1;
            }
            // inform that answer was given:
            if (!dropdown.gameObject.GetComponent<AnswerRecord>().WasAnswered)
            {
                dropdown.gameObject.GetComponent<AnswerRecord>().WasAnswered = true;
                givenAnswers++;
                if (givenAnswers == GameManager.instance.SurveyManager.Survey.Questions.Count - 1) GameManager.instance.SurveyManager.SetActiveEndSceneButton(true);
            }
        }

        /// <summary>
        /// Gets all answered questions assigned to survey questionnaire.
        /// </summary>
        /// <returns>List of questions</returns>
        public List<Question> GetSurveyAnswers()
        {
            List<Question> questions = new List<Question>();
            foreach(QuestionsPageController qpc in pages)
            {
                foreach(QuestionPanelController qpcr in qpc.Questions)
                {
                    qpcr.SaveAnswer();
                    questions.Add(qpcr.Question);
                }
            }
            return questions;
        }
        #endregion
    }
}
