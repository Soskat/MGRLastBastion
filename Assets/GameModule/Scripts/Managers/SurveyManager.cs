using UnityEngine;
using UnityEngine.UI;
using LastBastion.Game.SurveySystem;
using System.IO;
using LastBastion.Analytics;
using LastBastion.Game.UIControllers;
using UnityEngine.Assertions;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Survey scene behaviour.
    /// </summary>
    public class SurveyManager : MonoBehaviour
    {
        #region Private fields
        /// <summary>Button object that ends current scene.</summary>
        [SerializeField] private Button endSceneButton;
        /// <summary>Button object that returns to main menu scene.</summary>
        [SerializeField] private Button backToMainMenuButton;
        /// <summary>Component that represents questionnaire.</summary>
        [SerializeField] private QuestionnairePanelController questionnairePC;
        /// <summary>Path to a file with survey questions.</summary>
        [SerializeField] private string filePath;
        /// <summary>The survey with evaluation questions.</summary>
        private Survey survey;
        #endregion


        #region Public fields & properties
        /// <summary>The survey with evaluation questions.</summary>
        public Survey Survey { get { return survey; } }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(questionnairePC);
            // load survey questions from a file:
            filePath = Application.streamingAssetsPath + "/Resources/TextData/survey.json";
            survey = LoadSurveyQuestionnaireFromFile(filePath);
            if (survey == null)
            {
                survey = CreateTestData();
                SaveSurveyToFile(survey, filePath);
            }
            GameManager.instance.SurveyManager = this;
        }

        // Use this for initialization
        void Start()
        {
            // set up actions of buttons:
            endSceneButton.onClick.AddListener(() =>
            {
                if (GameManager.instance.AnalyticsEnabled) DataManager.AddSurveyAnswers(questionnairePC.GetSurveyAnswers());
                GameManager.instance.LoadNextLevel();
            });
            endSceneButton.gameObject.SetActive(false);
            if (GameManager.instance.DebugMode)
            {
                backToMainMenuButton.onClick.AddListener(() =>
                {
                    if (GameManager.instance.AnalyticsEnabled) DataManager.AddSurveyAnswers(questionnairePC.GetSurveyAnswers());
                    GameManager.instance.BackToMainMenu();
                });
            }
            else backToMainMenuButton.gameObject.SetActive(false);
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
        /// Saves survey questions to a file with specified file path.
        /// </summary>
        /// <param name="survey">Survey questions to save</param>
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
            survey.Questions.Add(new Question(1, "Question #1 (metrics) - QuestionType.Sex", QuestionType.Sex));
            survey.Questions.Add(new Question(2, "Question #2 (metrics) - QuestionType.Age", QuestionType.Age));
            survey.Questions.Add(new Question(3, "Question #3 (metrics) - QuestionType.PlayRoutine", QuestionType.PlayRoutine));
            survey.Questions.Add(new Question(4, "Question #4 (game evaluation) - QuestionType.TrueFalse", QuestionType.TrueFalse));
            survey.Questions.Add(new Question(5, "Question #5 (game evaluation) - QuestionType.Scale", QuestionType.Scale));
            survey.Questions.Add(new Question(6, "Question #6 (game evaluation) - QuestionType.AorB", QuestionType.AorB));
            survey.Questions.Add(new Question(7, "Question #7 (game evaluation) - QuestionType.Open", QuestionType.Open));
            return survey;
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Manages visibility of EndSceneButton game object.
        /// </summary>
        /// <param name="isActive">Is object visible (active)?</param>
        public void SetActiveEndSceneButton(bool isActive)
        {
            endSceneButton.gameObject.SetActive(isActive);
        }
        #endregion
    }
}
