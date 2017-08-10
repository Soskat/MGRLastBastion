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
        private Survey survey;
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
            for (int i = 0; i < 3; i++)
            {
                survey.Metrics.Add(new Question("Question #" + i, QuestionType.Age));
                survey.GameEvaluation.Add(new Question("Question #" + i, QuestionType.Scale));
            }
            return survey;
        }
        #endregion
    }
}
