using LastBastion.Game.Managers;
using LastBastion.Game.SurveySystem;
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
        private int currentPageIndex;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            currentPageIndex = 0;
            pages[0].gameObject.SetActive(true);
            for(int i = 1; i < pages.Count; i++) pages[i].gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Private methods
        private void CreateQuestionnaire()
        {
            Survey survey = GetComponentInParent<SurveyManager>().Survey;
            int questionsCreated = 0;
            while(questionsCreated < survey.Questions.Count)
            {
                GameObject page = Instantiate(Resources.Load("UIElements/QuestionsPage") as GameObject);
                page.transform.parent = transform;
                pages.Add(page.GetComponent<QuestionsPageController>());
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
            currentPageIndex = (++currentPageIndex >= pages.Count) ? 0 : currentPageIndex;
            pages[currentPageIndex].gameObject.SetActive(false);
        }

        /// <summary>
        /// Go to previous questions page.
        /// </summary>
        public void PreviousPage()
        {
            pages[currentPageIndex].gameObject.SetActive(false);
            currentPageIndex = (--currentPageIndex < 0) ? (pages.Count - 1) : currentPageIndex;
            pages[currentPageIndex].gameObject.SetActive(false);
        }
        #endregion
    }
}
