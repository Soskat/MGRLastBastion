using LastBastion.Biofeedback;
using LastBastion.Game.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.Player
{
    /// <summary>
    /// Component that manages Player biofeedback behaviour.
    /// </summary>
    public class BiofeedbackController : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private float arousalCurrentModifier;
        [SerializeField] private DataState arousalCurrentState;
        [SerializeField] private float arousalOldModifier;
        [SerializeField] private DataState arousalOldState;
        [SerializeField] private AudioClip heartSound;
        [SerializeField] private AudioSource biofeedbackAudio;
        #endregion


        #region Public fields & properties
        /// <summary>Current arousal modifier.</summary>
        public float ArousalCurrentModifier { get { return arousalCurrentModifier; } }
        /// <summary>Current arousal state based on <see cref="arousalModifier"/>.</summary>
        public DataState ArousalCurrentState { get { return arousalCurrentState; } }
        /// <summary>Old arousal modifier.</summary>
        public float ArousalOldtModifier { get { return arousalOldModifier; } }
        /// <summary>Old arousal state based on <see cref="arousalModifier"/>.</summary>
        public DataState ArousalOldState { get { return arousalOldState; } }
        #endregion
        

        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(heartSound);
            Assert.IsNotNull(biofeedbackAudio);
        }

        // Use this for initialization
        void Start()
        {
            GameManager.instance.BBModule.BiofeedbackDataChanged += () => UpdatePlayerState();
            biofeedbackAudio.clip = heartSound;
            // biofeedback is off:
            if (!GameManager.instance.BBModule.IsEnabled) StartCoroutine(Heartbeat());
        }

        // Update is called once per frame
        void Update()
        {
            // biofeedback on:
            if (GameManager.instance.BBModule.IsEnabled)
            {
                if (GameManager.instance.BBModule.ArousalState == DataState.High)
                {
                    if (GameManager.instance.AnalyticsEnabled) LevelManager.instance.AddGameEvent(Analytics.EventType.Heartbeat);
                    StartPlayingSound();
                }
                else StopPlayingSound();
            }
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Updates Player's biofeedback state.
        /// </summary>
        /// <param name="data"></param>
        private void UpdatePlayerState()
        {
            // save current arousal as old:
            arousalOldModifier = arousalCurrentModifier;
            arousalOldState = arousalCurrentState;
            // update current arousal:
            arousalCurrentModifier = GameManager.instance.BBModule.ArousalModifier;
            arousalCurrentState = GameManager.instance.BBModule.ArousalState;
        }

        /// <summary>
        /// Starts playing biofeedback sound.
        /// </summary>
        private void StartPlayingSound()
        {
            biofeedbackAudio.Play();
            // save info about event:
            if (GameManager.instance.AnalyticsEnabled) LevelManager.instance.AddGameEvent(Analytics.EventType.Heartbeat);
        }

        /// <summary>
        /// Stops playing biofeedback sound.
        /// </summary>
        private void StopPlayingSound()
        {
            if (biofeedbackAudio.isPlaying) biofeedbackAudio.Stop();
        }

        /// <summary>
        /// Activates and deactivates shaking hand simulation.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Heartbeat()
        {
            yield return new WaitForSeconds(Random.Range(60, 120));
            StartPlayingSound();
            yield return new WaitForSeconds(Random.Range(30, 90));
            StopPlayingSound();
            StartCoroutine(Heartbeat());
        }
        #endregion
    }
}