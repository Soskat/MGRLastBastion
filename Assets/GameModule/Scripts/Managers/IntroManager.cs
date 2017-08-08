using LastBastion.Game.Plot;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Intro scene behaviour.
    /// </summary>
    public class IntroManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Button endSceneButton;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private GameObject calibrationLabel;
        [SerializeField] private Text introTextUI;
        [SerializeField] private string introFilePath;
        [SerializeField] private IntroText introText;
        private bool menuOn;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(endSceneButton);
            Assert.IsNotNull(menuPanel);
            Assert.IsNotNull(resumeButton);
            Assert.IsNotNull(backToMainMenuButton);
            Assert.IsNotNull(calibrationLabel);
            Assert.IsNotNull(introTextUI);
        }

        // Use this for initialization
        void Start()
        {
            // set up in-game menu:
            resumeButton.onClick.AddListener(() => { menuOn = false; menuPanel.SetActive(menuOn); });
            backToMainMenuButton.onClick.AddListener(() => { StopAllCoroutines();  GameManager.instance.BackToMainMenu(); });
            menuOn = false;
            menuPanel.SetActive(menuOn);
            // set up end-scene button:
            endSceneButton.onClick.AddListener(() => { GameManager.instance.LevelHasEnded(); });
            endSceneButton.gameObject.SetActive(false);
            
            // start calibration data:
            if (GameManager.instance.BBModule.IsBandPaired) GameManager.instance.BBModule.CalibrateBandData();
            calibrationLabel.SetActive(true);


            // load intro text:
            introFilePath = Application.dataPath + "/GameModule/MainPlot/intro.json";
            introText = LoadIntroTextFromFile(introFilePath);
            if (introText == null)
            {
                // file with intro text doesn't exist - create new intro text data:
                introText = new IntroText(CreateTestData());
                SaveGoalsDataToFile(introText, introFilePath);
            }
            // play intro:
            StartCoroutine(PlayIntroduction());
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuOn = menuOn ? false : true;
                menuPanel.SetActive(menuOn);
            }

            if (!GameManager.instance.BBModule.IsCalibrationOn)
            {
                calibrationLabel.SetActive(false);
                endSceneButton.gameObject.SetActive(true);
            }
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Loads intro text data from a file with specific file path.
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        /// <returns>Intro text data</returns>
        private IntroText LoadIntroTextFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                Debug.Log("Loading intro text data from a file...");
                string dataAsText = File.ReadAllText(filePath);
                return JsonUtility.FromJson<IntroText>(dataAsText);
            }
            else return null;
        }

        /// <summary>
        /// Saves intro text data to a file with specified file path.
        /// </summary>
        /// <param name="intro">Intro text data to save</param>
        /// <param name="filePath">Path of the file</param>
        private void SaveGoalsDataToFile(IntroText intro, string filePath)
        {
            string dataAsJson = JsonUtility.ToJson(intro, true);
            File.WriteAllText(filePath, dataAsJson);
            Debug.Log("Saved intro text data to a file...");
        }

        /// <summary>
        /// Creates list of intro lines.
        /// </summary>
        /// <returns>List of intro lines</returns>
        private List<IntroLine> CreateTestData()
        {
            List<IntroLine> lines = new List<IntroLine>();
            lines.Add(new IntroLine(1.0f, "Dear friend,\\nI hope this letter finds you in good health. I know you're not one for pleasantries, so let me cut straight to the chase."));
            lines.Add(new IntroLine(1.0f, "During my last investigation I had to look into one of the many cults of Boston's underbelly. We were chasing a murderer that left his victims with eyes burnt out, their hands tied as if a mockery of praying."));
            lines.Add(new IntroLine(1.0f, "The book you will find in the package was found in his base of operations. I glanced into it briefly. Judging by the sketches, it might be his Codex or some way of planning his murders. I tried googling the script used in the book and concluded it's written entirely in ancient Sumerian."));
            lines.Add(new IntroLine(1.0f, "Since you are the only person that I know of that could try and decipher this tome, I turn to you. If you find any information that can help us apprehend the killer, please contact me immediately."));
            lines.Add(new IntroLine(1.0f, "I will owe you yet another favor and the city of Boston - eternal grattitude.\\n\\nBest regards,\\nJohn Murphy"));
            lines.Add(new IntroLine(1.0f, "PS As an incentive I'm sending you a bottle of your favorite vintage. If anyone asks - you didn't get it from me"));
            lines.Add(new IntroLine(2.0f, "Poor Jill..."));
            lines.Add(new IntroLine(1.0f, "I have felt it earlier that this book is somehow cursed."));
            lines.Add(new IntroLine(1.0f, "Because of it they locked you up in this miserable asylum."));
            lines.Add(new IntroLine(1.0f, "And now, all of the sudden, you have disappered and they have just closed this place for good..."));
            lines.Add(new IntroLine(1.0f, "Something isn't right, I can feel it deep in my bones."));
            lines.Add(new IntroLine(1.0f, "And I will find out what it is."));
            return lines;
        }


        private IEnumerator PlayIntroduction()
        {
            float elapsedTime;
            foreach (IntroLine line in introText.Content)
            {
                // slowly show text:
                introTextUI.text = line.Text;
                elapsedTime = 0f;
                while (introTextUI.GetComponent<CanvasGroup>().alpha < 1)
                {
                    elapsedTime += Time.deltaTime;
                    introTextUI.GetComponent<CanvasGroup>().alpha = Mathf.Clamp01(0.0f + (elapsedTime / 1.5f));
                    yield return null;
                }
                // wait for few seconds:
                yield return new WaitForSeconds(line.Duration);
                // slowly fade out text:
                elapsedTime = 0f;
                while (introTextUI.GetComponent<CanvasGroup>().alpha > 0)
                {
                    elapsedTime += Time.deltaTime;
                    introTextUI.GetComponent<CanvasGroup>().alpha = Mathf.Clamp01(1.0f - (elapsedTime / 2.0f));
                    yield return null;
                }
            }
            yield return null;
        }
        #endregion
    }
}
