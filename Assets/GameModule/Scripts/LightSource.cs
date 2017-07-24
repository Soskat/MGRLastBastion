using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LastBastion.Game
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class LightSource : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private AudioClip staticBuzzSound;
        [SerializeField] private AudioClip explodeSound;
        [SerializeField] private AudioClip brokenIgnitorSound;
        private AudioSource audioSource;
        private Animator animator;
        private ParticleSystem sparksBurst;
        private int turnOnAnim;
        private int turnOffAnim;
        private int explodeAnim;
        private bool isBusy = false;
        #endregion


        #region Public fields & properties

        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(staticBuzzSound);
            Assert.IsNotNull(explodeSound);
            Assert.IsNotNull(brokenIgnitorSound);
        }

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = staticBuzzSound;
            audioSource.playOnAwake = false;
            animator = GetComponent<Animator>();
            sparksBurst = GetComponentInChildren<ParticleSystem>();
            turnOnAnim = Animator.StringToHash("TurnOn");
            turnOffAnim = Animator.StringToHash("TurnOff");
            explodeAnim = Animator.StringToHash("Explode");
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Private methods
        /// <summary>
        /// Plays specific sound.
        /// </summary>
        /// <param name="sound">Sound to play</param>
        private void PlaySound(AudioClip sound)
        {
            audioSource.PlayOneShot(sound);
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Plays the sparks particle system.
        /// </summary>
        public void SparksBurst()
        {
            sparksBurst.Play();
        }

        /// <summary>
        /// Plays the sound of broken ignitor.
        /// </summary>
        public void PlayBrokenIgnitorSound()
        {
            PlaySound(brokenIgnitorSound);
        }

        /// <summary>
        /// Plays the sound of exploding light bulbs.
        /// </summary>
        public void PlayExplodeSound()
        {
            PlaySound(explodeSound);
        }

        /// <summary>
        /// Sets isBusy on.
        /// </summary>
        public void SetBusyOn()
        {
            isBusy = true;
        }

        /// <summary>
        /// Sets isBusy off.
        /// </summary>
        public void SetBusyOff()
        {
            isBusy = false;
        }

        /// <summary>
        /// Plays the static buzz sound.
        /// </summary>
        public void SetBuzzingOn()
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }

        /// <summary>
        /// Stops playing the static buzz sound.
        /// </summary>
        public void SetBuzzingOff()
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }
        #endregion
    }
}
