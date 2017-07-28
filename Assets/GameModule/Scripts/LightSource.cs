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
    public class LightSource : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool isOn = false;
        [SerializeField] private bool isBroken = false;
        [SerializeField] private bool isDead = false;
        [SerializeField] private GameObject lightBulb;
        [SerializeField] private Color explodeColor;
        [SerializeField] private AudioClip staticBuzzSound;
        [SerializeField] private AudioClip explodeSound;
        [SerializeField] private AudioClip brokenIgnitorSound;
        private AudioSource audioSource;
        private ParticleSystem sparksBurst;
        private Light lightSource;
        private bool isBusy = false;
        private float hue;
        private float saturation;
        private float value;
        #endregion


        #region Public fields & properties
        /// <summary>Is light broken?</summary>
        public bool IsBroken { get { return isBroken; } }
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
            lightSource = GetComponentInChildren<Light>();
            sparksBurst = GetComponentInChildren<ParticleSystem>();

            // turn the light on or off:
            SetLightMode(isOn);
        }
        #endregion


        #region Private methods
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
        private IEnumerator LightTurnOn()
        {
            isBusy = true;
            // simulate few light blinks:
            int blinks = Random.Range(1, 4);
            for(int i = 0; i < blinks; i++)
            {
                SetLightMode(true);
                PlayBrokenIgnitorSound();
                yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
                SetLightMode(false);
                yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
            }
            // finally turn the light on:
            SetLightMode(true);
            PlayBrokenIgnitorSound();
            SetBuzzingOn();
            isBusy = false;
        }

        /// <summary>
        /// Simulates light blinking.
        /// </summary>
        /// <param name="frequency">Blink frequency parameter</param>
        /// <returns></returns>
        private IEnumerator Blink(float frequency)
        {
            SetLightMode(false);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            SetLightMode(true);
            PlayBrokenIgnitorSound();
            yield return new WaitForSeconds(Random.Range(0.3f, 1f) * frequency);
            // start new blink:
            StartCoroutine(Blink(Random.Range(5f, 20f)));
        }
        
        /// <summary>
        /// Simulates the process of turning off the light.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LightTurnOff()
        {
            isBusy = true;
            SetBuzzingOff();
            lightSource.intensity = 0f;
            isBusy = false;
            // slowly extinguish lightbulb emission:
            float step = 0.02f;
            Color.RGBToHSV(lightBulb.GetComponent<Renderer>().material.GetColor("_EmissionColor"), out hue, out saturation, out value);
            while (value > 0.0f)
            {
                value -= step;
                lightBulb.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.HSVToRGB(hue, saturation, value));
                yield return null;
            }
        }

        /// <summary>
        /// Simulates the process of exploding of the broken lightbulb.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Explode()
        {
            isBusy = true;
            PlayBrokenIgnitorSound();
            // simulate lightbulb warm-up:
            SetLightMode(true);
            lightSource.intensity = 20f;
            lightBulb.GetComponent<Renderer>().material.SetColor("_EmissionColor", explodeColor);
            yield return new WaitForSeconds(0.1f);
            // explode:
            lightSource.intensity = 0f;
            SparksBurst();
            PlayExplodeSound();
            // extinguish lightbulb emission:
            float step = 0.02f;
            Color.RGBToHSV(lightBulb.GetComponent<Renderer>().material.GetColor("_EmissionColor"), out hue, out saturation, out value);
            while (value > 0.0f)
            {
                value -= step;
                lightBulb.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.HSVToRGB(hue, saturation, value));
                yield return null;
            }
            isBusy = false;
        }

        /// <summary>
        /// Plays the sparks particle system.
        /// </summary>
        private void SparksBurst()
        {
            sparksBurst.Play();
        }

        /// <summary>
        /// Plays specific sound.
        /// </summary>
        /// <param name="sound">Sound to play</param>
        private void PlaySound(AudioClip sound)
        {
            audioSource.PlayOneShot(sound);
        }

        /// <summary>
        /// Plays the sound of broken ignitor.
        /// </summary>
        private void PlayBrokenIgnitorSound()
        {
            PlaySound(brokenIgnitorSound);
        }

        /// <summary>
        /// Plays the sound of exploding light bulbs.
        /// </summary>
        private void PlayExplodeSound()
        {
            PlaySound(explodeSound);
        }

        /// <summary>
        /// Plays the static buzz sound.
        /// </summary>
        private void SetBuzzingOn()
        {
            audioSource.Play();
        }

        /// <summary>
        /// Stops playing the static buzz sound.
        /// </summary>
        private void SetBuzzingOff()
        {
            if (audioSource.isPlaying) audioSource.Stop();
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
                StopAllCoroutines();
                if (isBroken)
                {
                    StartCoroutine(Explode());
                    isDead = true;
                }
                else
                {
                    StartCoroutine(LightTurnOn());
                    isOn = true;
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
                StartCoroutine(LightTurnOff());
                isOn = false;
            }
        }

        /// <summary>
        /// Simulates blinking of the light.
        /// </summary>
        public void StartBlinking()
        {
            StartCoroutine(Blink(Random.Range(1f, 12f)));
        }
        #endregion
    }
}
