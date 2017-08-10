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
    [RequireComponent(typeof(AudioSource))]
    public class IntroManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Button endSceneButton;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private Button skipIntroButton;
        [SerializeField] private GameObject calibrationLabel;
        [SerializeField] private Text introTextUI;
        [SerializeField] private string introFilePath;
        [SerializeField] private AudioClip metalGateOpeningSound;
        private IntroText introText;
        private AudioSource audioSource;
        private bool menuOn;
        private bool introHasEnded;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(endSceneButton);
            Assert.IsNotNull(menuPanel);
            Assert.IsNotNull(resumeButton);
            Assert.IsNotNull(backToMainMenuButton);
            Assert.IsNotNull(skipIntroButton);
            Assert.IsNotNull(calibrationLabel);
            Assert.IsNotNull(introTextUI);
            Assert.IsNotNull(metalGateOpeningSound);
        }

        // Use this for initialization
        void Start()
        {
            // set up in-game menu:
            resumeButton.onClick.AddListener(() => { menuOn = false; menuPanel.SetActive(menuOn); });
            backToMainMenuButton.onClick.AddListener(() => { StopAllCoroutines();  GameManager.instance.BackToMainMenu(); });
            skipIntroButton.onClick.AddListener(() => { GameManager.instance.LevelHasEnded(); });
            skipIntroButton.gameObject.SetActive(GameManager.instance.DebugMode);
            menuOn = false;
            menuPanel.SetActive(menuOn);
            // set up end-scene button:
            endSceneButton.onClick.AddListener(() => { GameManager.instance.LevelHasEnded(); });
            endSceneButton.gameObject.SetActive(false);
            
            // start calibration data:
            if (GameManager.instance.BBModule.IsBandPaired) GameManager.instance.BBModule.CalibrateBandData();
            calibrationLabel.SetActive(true);


            // load intro text:
            introFilePath = Application.dataPath + "/Resources/TextData/intro.json";
            introText = LoadIntroTextFromFile(introFilePath);
            if (introText == null)
            {
                // file with intro text doesn't exist - create new intro text data:
                introText = new IntroText(CreateTestData());
                SaveGoalsDataToFile(introText, introFilePath);
            }
            // play intro:
            //Debug.Log("Intro duration in sec: " + CalculateTextDuration(introText));    // ----------- test
            introHasEnded = false;
            float showOffTime = 1.3f, fadeTime = 2.5f;
            introTextUI.GetComponent<CanvasGroup>().alpha = 0.0f;
            StartCoroutine(PlayIntroduction(showOffTime, fadeTime));
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(PlayBackgroundSounds(CalculateTextDuration(introText) + introText.Content.Count * (showOffTime + fadeTime), 1.0f));
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuOn = menuOn ? false : true;
                menuPanel.SetActive(menuOn);
            }

            if (!GameManager.instance.BBModule.IsCalibrationOn && introHasEnded)
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
                //Debug.Log("Loading intro text data from a file...");
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
            //Debug.Log("Saved intro text data to a file...");
        }

        /// <summary>
        /// Creates test list of intro lines.
        /// </summary>
        /// <returns>List of intro lines</returns>
        private List<IntroLine> CreateTestData()
        {
            List<IntroLine> lines = new List<IntroLine>();
            lines.Add(new IntroLine(3.0f, 2.0f, "Dear friend,"));
            lines.Add(new IntroLine(1.0f, 3.0f, "I hope this letter finds you in good health."));
            lines.Add(new IntroLine(1.0f, 5.0f, "I know you're not one for pleasantries, so let me cut straight to the chase."));
            // -----------------------------------------------------------------------------------------------------------------------------------
            lines.Add(new IntroLine(1.5f, 5.0f, "During my last investigation I had to look into one of the many cults of Boston's underbelly."));
            lines.Add(new IntroLine(1.0f, 7.0f, "We were chasing a murderer that left his victims with eyes burnt out, their hands tied as if a mockery of praying."));
            // -----------------------------------------------------------------------------------------------------------------------------------
            lines.Add(new IntroLine(1.5f, 5.0f, "The book you will find in the package was found in his base of operations."));
            lines.Add(new IntroLine(1.0f, 6.0f, "I glanced into it briefly. Judging by the sketches, it might be his Codex or some way of planning his murders."));
            lines.Add(new IntroLine(1.0f, 6.0f, "I tried googling the script used in the book and concluded it's written entirely in ancient Sumerian."));
            // -----------------------------------------------------------------------------------------------------------------------------------
            lines.Add(new IntroLine(1.5f, 6.0f, "Since you are the only person that I know of that could try and decipher this tome, I turn to you."));
            lines.Add(new IntroLine(1.0f, 6.0f, "If you find any information that can help us apprehend the killer, please contact me immediately."));
            // -----------------------------------------------------------------------------------------------------------------------------------
            lines.Add(new IntroLine(1.5f, 5.0f, "I will owe you yet another favor and the city of Boston - eternal grattitude."));
            lines.Add(new IntroLine(1.0f, 3.0f, "Best regards, John Murphy"));
            // -----------------------------------------------------------------------------------------------------------------------------------
            lines.Add(new IntroLine(1.5f, 3.0f, "PS As an incentive I'm sending you a bottle of your favorite vintage."));
            lines.Add(new IntroLine(1.0f, 3.0f, "If anyone asks - you didn't get it from me"));
            // -----------------------------------------------------------------------------------------------------------------------------------
            lines.Add(new IntroLine(3.0f, 2.0f, "Poor Jill..."));
            lines.Add(new IntroLine(1.5f, 4.0f, "I have felt it earlier that this book is somehow cursed."));
            lines.Add(new IntroLine(1.5f, 4.0f, "Because of it they locked you up in this miserable asylum."));
            lines.Add(new IntroLine(1.5f, 4.0f, "And now, all of the sudden, you have disappered and they have just closed this place for good..."));
            lines.Add(new IntroLine(1.5f, 4.0f, "Something isn't right, I can feel it deep in my bones."));
            lines.Add(new IntroLine(3.0f, 6.0f, "And I will find out what it is."));
            return lines;
        }

        /// <summary>
        /// Slowly displays introduction text in parts.
        /// </summary>
        /// <param name="showOffTime">Time of showing off the text line</param>
        /// <param name="fadeTime">Time of fading the text line</param>
        /// <returns></returns>
        private IEnumerator PlayIntroduction(float showOffTime, float fadeTime)
        {
            float elapsedTime;
            foreach (IntroLine line in introText.Content)
            {
                yield return new WaitForSeconds(line.Cooldown);
                yield return _sync();
                // slowly show text:
                introTextUI.text = line.Text;
                elapsedTime = 0f;
                while (introTextUI.GetComponent<CanvasGroup>().alpha < 1)
                {
                    elapsedTime += Time.deltaTime;
                    introTextUI.GetComponent<CanvasGroup>().alpha = Mathf.Clamp01(0.0f + (elapsedTime / showOffTime));
                    yield return _sync();
                }
                // wait for few seconds:
                yield return new WaitForSeconds(line.Duration);
                yield return _sync();
                // slowly fade out text:
                elapsedTime = 0f;
                while (introTextUI.GetComponent<CanvasGroup>().alpha > 0)
                {
                    elapsedTime += Time.deltaTime;
                    introTextUI.GetComponent<CanvasGroup>().alpha = Mathf.Clamp01(1.0f - (elapsedTime / fadeTime));
                    yield return _sync();
                }
            }
        }

        /// <summary>
        /// Simulates footsteps on gravel sounds in the background.
        /// </summary>
        /// <param name="textDuration">Duration of introduction text</param>
        /// <param name="delay">Delay at the beginning</param>
        /// <returns></returns>
        private IEnumerator PlayBackgroundSounds(float textDuration, float delay)
        {
            int stepCount = (int)(textDuration - delay) - 2;
            //Debug.Log("Setp count: " + stepCount);  // -------------------- test
            yield return new WaitForSeconds(delay);
            for (int i = 0; i < stepCount; i++)
            {
                audioSource.PlayOneShot(GameManager.instance.Assets.GetFootstepOnGravelSound());
                yield return new WaitForSeconds(1.0f);
                yield return _sync();
            }
            audioSource.PlayOneShot(GameManager.instance.Assets.GetFootstepOnGravelSound());
            yield return new WaitForSeconds(0.8f);
            yield return _sync();
            audioSource.PlayOneShot(GameManager.instance.Assets.GetFootstepOnGravelSound());
            yield return new WaitForSeconds(0.7f);
            yield return _sync();
            audioSource.PlayOneShot(GameManager.instance.Assets.GetFootstepOnGravelSound());
            // play sound of opening the asylum metal gate:
            yield return new WaitForSeconds(1.5f);
            yield return _sync();
            audioSource.PlayOneShot(metalGateOpeningSound);
            yield return new WaitForSeconds(2.0f);
            introHasEnded = true;
        }

        /// <summary>
        /// Calculates duration time of display of the introduction text.
        /// </summary>
        /// <param name="introText">Introduction text data</param>
        /// <returns>Introduction text duration</returns>
        private float CalculateTextDuration(IntroText introText)
        {
            float duration = 0f;
            foreach(IntroLine line in introText.Content)
            {
                duration += line.Cooldown + line.Duration;
            }
            return duration;
        }

        // based on: https://forum.unity3d.com/threads/how-to-pause-any-coroutine-according-to-your-global-pause-state.68303/ by n0mad
        private Coroutine _sync()
        {
            return StartCoroutine(PauseRoutine());
        }

        private IEnumerator PauseRoutine()
        {
            while (menuOn)
            {
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForEndOfFrame();
        }

        #endregion
    }
}
