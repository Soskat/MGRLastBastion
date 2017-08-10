using UnityEngine;
using UnityEngine.UI;
using LastBastion.Game.SurveySystem;
using System.IO;
using System.Collections.Generic;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Survey scene behaviour.
    /// </summary>
    public class SurveyManager : MonoBehaviour
    {

        #region Private fields
        [SerializeField] private Button endSceneButton;
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private string filePath;
        public Survey survey;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            endSceneButton.onClick.AddListener(() => { GameManager.instance.LevelHasEnded(); });
            backToMainMenuButton.onClick.AddListener(() => { GameManager.instance.BackToMainMenu(); });

            filePath = Application.dataPath + "/Resources/TextData/survey.json";
            survey = LoadSurveyQuestionnaireFromFile(filePath);
            if (survey == null)
            {
                survey = CreateTestData();
                SaveSurveyToFile(survey, filePath);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Private methods
        /// <summary>
        /// Loads survey questions from a file with specific file path.
        /// </summary>
        /// <param name="filePath">Path of the file with survey questionnaire</param>
        private Survey LoadSurveyQuestionnaireFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                //Debug.Log("Loading survey data from a file...");
                string dataAsText = File.ReadAllText(filePath);
                return JsonUtility.FromJson<Survey>(dataAsText);
            }
            else return null;
        }

        /// <summary>
        /// Saves survey data to a file with specified file path.
        /// </summary>
        /// <param name="intro">Intro text data to save</param>
        /// <param name="filePath">Path of the file</param>
        private void SaveSurveyToFile(Survey survey, string filePath)
        {
            string dataAsJson = JsonUtility.ToJson(survey, true);
            File.WriteAllText(filePath, dataAsJson);
            //Debug.Log("Saved survey data to a file...");
        }

        /// <summary>
        /// Creates test survey data.
        /// </summary>
        /// <returns>Test survey data</returns>
        private Survey CreateTestData()
        {
            Survey survey = new Survey();
            survey.Metrics.Add(new Question(1, "Question #1 (metrics) - QuestionType.Sex", QuestionType.Sex));
            survey.Metrics.Add(new Question(2, "Question #2 (metrics) - QuestionType.Age", QuestionType.Age));
            survey.Metrics.Add(new Question(3, "Question #3 (metrics) - QuestionType.PlayRoutine", QuestionType.PlayRoutine));
            survey.GameEvaluation.Add(new Question(4, "Question #4 (game evaluation) - QuestionType.TrueFalse", QuestionType.TrueFalse));
            survey.GameEvaluation.Add(new Question(5, "Question #5 (game evaluation) - QuestionType.Scale", QuestionType.Scale));
            survey.GameEvaluation.Add(new Question(6, "Question #6 (game evaluation) - QuestionType.AorB", QuestionType.AorB));
            survey.GameEvaluation.Add(new Question(7, "Question #7 (game evaluation) - QuestionType.Open", QuestionType.Open));
            return survey;
        }
        #endregion
    }
}
