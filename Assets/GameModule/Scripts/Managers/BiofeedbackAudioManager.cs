using LastBastion.Biofeedback;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Player's biofeedback audio behaviour.
    /// </summary>
    public class BiofeedbackAudioManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private AudioClip biofeedbackSound;
        [SerializeField] private AudioSource biofeedbackAudio;
        #endregion
        

        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(biofeedbackSound);
            Assert.IsNotNull(biofeedbackAudio);
        }

        // Use this for initialization
        void Start()
        {
            biofeedbackAudio.clip = biofeedbackSound;
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
        /// Starts playing biofeedback sound.
        /// </summary>
        private void StartPlayingSound()
        {
            if (!biofeedbackAudio.isPlaying) biofeedbackAudio.Play();
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