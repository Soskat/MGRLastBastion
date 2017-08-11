using LastBastion.Game.Managers;
using LastBastion.Game.SurveySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.UIControllers
{
    /// <summary>
    /// Component that manages behaviour of QuestionnairePanel game object.
    /// </summary>
    public class QuestionnairePanelController : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private List<QuestionsPageController> pages;
        [SerializeField] private int givenAnswers = 0;
        private int currentPageIndex;
        #endregion


        #region Public fields & properties
        /// <summary>Given answers count.</summary>
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

        // Update is called once per frame
        void Update()
        {

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
        /// Changes if player answered to all of the survey questions.
        /// </summary>
        /// <param name="selectedIndex">Index of selected answer in dropdown menu</param>
        public void ChangeAnswer(int selectedIndex)
        {
            if (selectedIndex > 0) givenAnswers++;
            else givenAnswers--;

            if (givenAnswers == GameManager.instance.SurveyManager.Survey.Questions.Count) GameManager.instance.SurveyManager.SetActiveEndSceneButton(true);
            else GameManager.instance.SurveyManager.SetActiveEndSceneButton(false);
        }
        #endregion
    }
}
