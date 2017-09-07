using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents light source behaviour.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class LightSource : MonoBehaviour
    {
        #region Private fields
        /// <summary>Is light turned on?</summary>
        [SerializeField] private bool isOn = false;
        /// <summary>Is light broken?</summary>
        [SerializeField] private bool isBroken = false;
        /// <summary>Is light dead?</summary>
        [SerializeField] private bool isDead = false;
        /// <summary>Maximum value of light's intensity.</summary>
        [SerializeField] private float maxLightIntensity = 5f;
        /// <summary>Bulb game object.</summary>
        [SerializeField] private GameObject lightBulb;
        /// <summary>Color of the exploding light.</summary>
        [SerializeField] private Color explodeColor;
        /// <summary>Sound of the static buzz.</summary>
        [SerializeField] private AudioClip staticBuzzSound;
        /// <summary>Sound of the bulb explosion.</summary>
        [SerializeField] private AudioClip explodeSound;
        /// <summary>Sound of the broken ignitor.</summary>
        [SerializeField] private AudioClip brokenIgnitorSound;
        /// <summary>Assigned <see cref="AudioSource"/> component.</summary>
        private AudioSource audioSource;
        /// <summary>Assigned <see cref="ParticleSystem"/> component.</summary>
        private ParticleSystem sparksBurst;
        /// <summary>Assigned <see cref="Light"/> component.</summary>
        private Light lightSource;
        /// <summary>Is light busy?</summary>
        private bool isBusy = false;
        /// <summary>Hue component of the bulb color.</summary>
        private float hue;
        /// <summary>Saturation component of the bulb color.</summary>
        private float saturation;
        /// <summary>Calue component of the bulb color.</summary>
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
                lightSource.intensity = maxLightIntensity;
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
            int blinks = RandomNumberGenerator.Range(1, 4);
            for(int i = 0; i < blinks; i++)
            {
                SetLightMode(true);
                PlayBrokenIgnitorSound();
                yield return new WaitForSeconds(RandomNumberGenerator.Range(0.1f, 0.2f));
                SetLightMode(false);
                yield return new WaitForSeconds(RandomNumberGenerator.Range(0.2f, 0.4f));
            }
            // finally turn the light on:
            SetLightMode(true);
            PlayBrokenIgnitorSound();
            SetBuzzingOn();
            isBusy = false;
        }

        /// <summary>
        /// Simulates the process of exploding of the broken lightbulb.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LightTurnOnBroken()
        {
            isBusy = true;
            SetBuzzingOff();
            PlayBrokenIgnitorSound();
            // simulate lightbulb warm-up:
            SetLightMode(true);
            lightSource.intensity = maxLightIntensity * 2;
            lightBulb.GetComponent<Renderer>().material.SetColor("_EmissionColor", explodeColor);
            yield return new WaitForSeconds(0.1f);
            // explode:
            StartCoroutine(ExplodeLightbulb());
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
        /// Simulates light blink.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Blink()
        {
            isBusy = true;
            SetLightMode(false);
            yield return new WaitForSeconds(RandomNumberGenerator.Range(0.1f, 0.3f));
            SetLightMode(true);
            PlayBrokenIgnitorSound();
            isBusy = false;
        }

        /// <summary>
        /// Simulates the process of the lightbulb explosion.
        /// </summary>
        private IEnumerator ExplodeLightbulb()
        {
            isBusy = true;
            SetBuzzingOff();
            // explode lightbulb:
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
            if (sparksBurst != null) sparksBurst.Play();
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
                if (isBroken)
                {
                    StartCoroutine(LightTurnOnBroken());
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
                StartCoroutine(LightTurnOff());
                isOn = false;
            }
        }

        /// <summary>
        /// Performs single blink of the light.
        /// </summary>
        public void DoBlink()
        {
            if (isOn) StartCoroutine(Blink());
        }

        /// <summary>
        /// Explodes the light.
        /// </summary>
        public void ExplodeLight()
        {
            if(!isDead && !isBusy && isOn)
            {
                isBroken = true;
                isOn = false;
                StartCoroutine(ExplodeLightbulb());
            }
        }
        #endregion
    }
}
