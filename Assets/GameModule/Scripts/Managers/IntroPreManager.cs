using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Intro scene behaviour.
    /// </summary>
    public class IntroPreManager : MonoBehaviour
    {

        #region Private fields
        [SerializeField] private Button endSceneButton;
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private GameObject calibrationLabel;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(endSceneButton);
            Assert.IsNotNull(backToMainMenuButton);
            Assert.IsNotNull(calibrationLabel);
        }

        // Use this for initialization
        void Start()
        {
            endSceneButton.onClick.AddListener(() => { GameManager.instance.LevelHasEnded(); });
            endSceneButton.enabled = false;
            backToMainMenuButton.onClick.AddListener(() => { GameManager.instance.BackToMainMenu(); });

            // start calibration data:
            if (GameManager.instance.BBModule.IsBandPaired) GameManager.instance.BBModule.CalibrateBandData();
            calibrationLabel.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameManager.instance.BBModule.IsCalibrationOn)
            {
                endSceneButton.enabled = true;
                calibrationLabel.SetActive(false);
            }
        }
        #endregion
    }
}