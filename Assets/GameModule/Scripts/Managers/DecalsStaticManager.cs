using LastBastion.Analytics;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages visibility of static decals that exist on scene.
    /// </summary>
    public class DecalsStaticManager : MonoBehaviour
    {
        #region Private fields
        /// <summary>Game object with light decals objects as children.</summary>
        [SerializeField] private GameObject decalsLight;
        /// <summary>Game object with hard decals objects as children.</summary>
        [SerializeField] private GameObject decalsHard;
        /// <summary>Has the decals manager been activated?</summary>
        [SerializeField] private bool wasActivated = false;
        #endregion


        #region Public fields & properties
        /// <summary>Has the decals manager been activated?</summary>
        public bool WasActivated
        {
            get { return wasActivated; }
            set { wasActivated = value; }
        }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(decalsLight);
        }

        // Use this for initialization
        void Start()
        {
            if (decalsLight != null) decalsLight.SetActive(false);
            if (decalsHard != null) decalsHard.SetActive(false);
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Activates sets of decals.
        /// </summary>
        public void ActivateDecals()
        {
            if (WasActivated || GameManager.instance.ActiveRoom.GetComponentInChildren<LightManager>().LightsOn) return;

            // biofeedback on:
            if (GameManager.instance.BiofeedbackMode == BiofeedbackMode.BiofeedbackON && GameManager.instance.BBModule.IsEnabled)
            {
                // activate decals set based on player's biofeedback:
                switch (GameManager.instance.BBModule.ArousalState)
                {
                    // if player's arousal is high, activate extra decals set:
                    case Biofeedback.DataState.High:
                        decalsLight.SetActive(true);
                        if (decalsHard != null) decalsHard.SetActive(true);
                        wasActivated = true;
                        break;

                    // activate decals set:
                    case Biofeedback.DataState.Medium:
                    case Biofeedback.DataState.Low:
                        decalsLight.SetActive(true);
                        wasActivated = true;
                        break;

                    default: break;
                }
            }
            // biofeedback off:
            else
            {
                // choose randomly decals set:
                int choice = Random.Range(0, 2);
                switch (choice)
                {
                    case 0:
                        // activate both sets:
                        decalsLight.SetActive(true);
                        if (decalsHard != null) decalsHard.SetActive(true);
                        wasActivated = true;
                        break;

                    case 1:
                        // activate only one set:
                        decalsLight.SetActive(true);
                        wasActivated = true;
                        break;
                }
            }

            // save info about event:
            if (GameManager.instance.AnalyticsEnabled) LevelManager.instance.AddGameEvent(Analytics.EventType.Decals);
        }
        #endregion
    }
}
