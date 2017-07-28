using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LastBastion.Game
{
    /// <summary>
    /// Component that represents light source behaviour.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class LightSource : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool isOn = false;
        [SerializeField] private bool isBroken = false;
        [SerializeField] private bool isDead = false;
        [SerializeField] private GameObject lightBulb;
        [SerializeField] private AudioClip staticBuzzSound;
        [SerializeField] private AudioClip explodeSound;
        [SerializeField] private AudioClip brokenIgnitorSound;
        private AudioSource audioSource;
        private Animator animator;
        private ParticleSystem sparksBurst;
        private Light lightSource;
        //private int turnOffAnim;
        //private int explodeAnim;

        private int turnOffTrigger;
        private int explodeTrigger;
        private int isOnBool;

        private bool isBusy = false;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(lightBulb);
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
            lightSource = GetComponentInChildren<Light>();
            sparksBurst = GetComponentInChildren<ParticleSystem>();
            //turnOffAnim = Animator.StringToHash("TurnOff");
            //explodeAnim = Animator.StringToHash("Explode");

            turnOffTrigger = Animator.StringToHash("TurnOffTrigger");
            explodeTrigger = Animator.StringToHash("ExplodeTrigger");
            isOnBool = Animator.StringToHash("IsOn");

            // turn the light on or off:
            //if (isOn) SetLightMode(true);
            //else SetLightMode(false);
            if (isOn) animator.SetBool(isOnBool, true);
            else animator.SetBool(isOnBool, false);
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

        /// <summary>
        /// Sets light mode to turned-on or turned-off.
        /// </summary>
        /// <param name="turnOn">Is the light turned on?</param>
        private void SetLightMode(bool turnOn)
        {
            if (turnOn)
            {
                lightBulb.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
                lightSource.intensity = 10f;
            }
            else
            {
                lightBulb.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
                lightSource.intensity = 0f;
            }
        }

        /// <summary>
        /// Simulates the process of turning on the light after few blinks.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LightsUp()
        {
            isBusy = true;

            animator.enabled = false;

            // simulate few light blinks:
            int blinks = Random.Range(1, 4);
            for(int i = 0; i < blinks; i++)
            {
                SetLightMode(true);
                lightBulb.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
                PlayBrokenIgnitorSound();
                yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
                SetLightMode(false);
                lightBulb.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
                yield return new WaitForSeconds(Random.Range(0.7f, 1f));
            }
            // finally turn the light on:
            SetLightMode(true);
            PlayBrokenIgnitorSound();
            SetBuzzingOn();

            animator.enabled = true;

            isBusy = false;
            // set the isOn flag to true:
            isOn = true;
            animator.SetBool(isOnBool, true);
        }

        /// <summary>
        /// Simulates light blinking.
        /// </summary>
        /// <param name="frequency">Blink frequency parameter</param>
        /// <returns></returns>
        private IEnumerator Blink(float frequency)
        {
            SetLightMode(false);
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f) * frequency);
            SetLightMode(true);
            PlayBrokenIgnitorSound();
            // start new blink:
            StartCoroutine(Blink(Random.Range(5f, 20f)));
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Turns on the light.
        /// </summary>
        public void TurnOnTheLight()
        {
            if (!isDead && !isBusy && !isOn)
            {
                if (isBroken)
                {
                    isDead = true;
                    //animator.Play(explodeAnim);
                    animator.applyRootMotion = false;
                    animator.SetTrigger(explodeTrigger);
                    animator.SetBool(isOnBool, false);
                }
                else
                {
                    StartCoroutine(LightsUp());
                }
            }
        }

        /// <summary>
        /// Turns off the light.
        /// </summary>
        public void TurnOffTheLight()
        {
            if (!isDead && !isBusy && isOn)
            {
                StopAllCoroutines();
                //animator.Play(turnOffAnim);
                animator.applyRootMotion = false;
                animator.SetTrigger(turnOffTrigger);
                isOn = false;
                animator.SetBool(isOnBool, false);
            }
        }

        /// <summary>
        /// Simulates blinking of the light.
        /// </summary>
        public void StartBlinking()
        {
            StartCoroutine(Blink(Random.Range(5f, 20f)));
        }

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

        /// <summary>
        /// Resets Animator.applyRootMotion flag.
        /// </summary>
        public void ResetRootMotion()
        {
            animator.applyRootMotion = true;
        }
        #endregion
    }
}
