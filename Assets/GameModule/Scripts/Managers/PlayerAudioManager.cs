using LastBastion.Analytics;
using LastBastion.Biofeedback;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Player's biofeedback audio behaviour.
    /// </summary>
    public class PlayerAudioManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private AudioClip biofeedbackSound;
        [SerializeField] private AudioClip ambientSound;
        [SerializeField] private AudioClip rumblingSound;
        [SerializeField] private AudioClip riserSound;
        [SerializeField] private AudioSource biofeedbackAudio;
        [SerializeField] private AudioSource ambientAudio;
        [SerializeField] private AudioSource outroRumblingAudio;
        [SerializeField] private AudioSource outroRiserAudio;
        #endregion


        #region Public fields & properties
        /// <summary>The lenght of riser audio clip.</summary>
        public float RiserSoundLenght { get { return riserSound.length; } }
        #endregion

        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(biofeedbackSound);
            Assert.IsNotNull(ambientSound);
            Assert.IsNotNull(rumblingSound);
            Assert.IsNotNull(riserSound);
            Assert.IsNotNull(biofeedbackAudio);
            Assert.IsNotNull(ambientAudio);
            Assert.IsNotNull(outroRumblingAudio);
            Assert.IsNotNull(outroRiserAudio);
        }

        // Use this for initialization
        void Start()
        {
            // set up all audio sources:
            biofeedbackAudio.clip = biofeedbackSound;
            biofeedbackAudio.playOnAwake = false;
            ambientAudio.clip = ambientSound;
            ambientAudio.playOnAwake = true;
            outroRumblingAudio.clip = rumblingSound;
            outroRumblingAudio.playOnAwake = false;
            outroRiserAudio.clip = riserSound;
            outroRiserAudio.playOnAwake = false;
            // biofeedback is off:
            if (GameManager.instance.BiofeedbackMode == BiofeedbackMode.BiofeedbackOFF || !GameManager.instance.BBModule.IsEnabled) StartCoroutine(Heartbeat());
        }

        // Update is called once per frame
        void Update()
        {
            // if outro is playing, skip all calculations:
            if (LevelManager.instance.IsOutroOn) return;

            // biofeedback on:
            if (GameManager.instance.BiofeedbackMode == BiofeedbackMode.BiofeedbackON && GameManager.instance.BBModule.IsEnabled)
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
            Debug.Log("Random heart beat event");
            StartPlayingSound();
            yield return new WaitForSeconds(Random.Range(30, 90));
            StopPlayingSound();
            StartCoroutine(Heartbeat());
        }

        // based on Boris1998 code from: https://forum.unity3d.com/threads/fade-out-audio-source.335031/
        /// <summary>
        /// Fades out audio source in given time.
        /// </summary>
        /// <param name="audioSource">Audio source to fade out</param>
        /// <param name="FadeTime">Range of time</param>
        /// <returns></returns>
        private static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;

        }
        #endregion

        #region Public methods
        /// <summary>
        /// Fades out biofeedback and ambient sounds.
        /// </summary>
        public void FadeOutBiofeedbackSounds()
        {
            float fadeTime = 2f;
            if (biofeedbackAudio.isPlaying) StartCoroutine(FadeOut(biofeedbackAudio, fadeTime));
            if (ambientAudio.isPlaying) StartCoroutine(FadeOut(ambientAudio, fadeTime));
        }

        /// <summary>
        /// Starts playing the rumbling sound.
        /// </summary>
        public void PlayRumblingSound()
        {
            outroRumblingAudio.Play();
        }

        /// <summary>
        /// Starts playing the riser sound.
        /// </summary>
        public void PlayRiserSound()
        {
            outroRiserAudio.PlayOneShot(riserSound);
            if (outroRumblingAudio.isPlaying) StartCoroutine(FadeOut(outroRumblingAudio, 7.5f));
            // launch delayed fade out camera event:
            StartCoroutine(LevelManager.instance.FadeOutCamera(9f));
        }
        #endregion
    }
}