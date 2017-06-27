using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game
{
    /// <summary>
    /// Component that manages flashlight behaviour.
    /// </summary>
    public class Flashlight : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool lightOn = false;
        [SerializeField] private GameObject lightRay;
        [SerializeField] private AudioClip switchOnSound;
        [SerializeField] private AudioClip switchOffSound;
        [SerializeField] private AudioClip hitSound;
        private AudioSource audioSource;
        #endregion


        #region Public fields & properties
        /// <summary>Is light turned on?</summary>
        public bool LightOn { get { return lightOn; } }
        /// <summary>Is flashlight dead?</summary>
        public bool IsDead = false;
        /// <summary>Is flashlight busy?</summary>
        public bool IsBusy = false;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(lightRay);
            Assert.IsNotNull(switchOnSound);
            Assert.IsNotNull(switchOffSound);
            Assert.IsNotNull(hitSound);
        }

        // Use this for initialization
        void Start()
        {
            lightRay.SetActive(false);
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Switches the light of the flashlight.
        /// </summary>
        public void SwitchLight()
        {
            if (!IsBusy)
            {
                lightOn = (lightOn) ? false : true;
                if (lightOn)
                {
                    lightRay.SetActive(true);
                    if (audioSource != null) audioSource.PlayOneShot(switchOnSound);
                }
                else
                {
                    lightRay.SetActive(false);
                    if (audioSource != null) audioSource.PlayOneShot(switchOffSound);
                }
            }
        }

        /// <summary>
        /// Turns off the light.
        /// </summary>
        public void TurnOffLight()
        {
            lightOn = false;
            lightRay.SetActive(false);
        }

        /// <summary>
        /// Turns on the light.
        /// </summary>
        public void TurnOnLight()
        {
            lightOn = true;
            lightRay.SetActive(true);
        }

        /// <summary>
        /// Plays sound when player switches light.
        /// </summary>
        public void PlaySwitchSound()
        {
            if (audioSource != null) audioSource.PlayOneShot(switchOnSound);
        }

        /// <summary>
        /// Plays sound when flashlight hit something.
        /// </summary>
        public void PlayHitSound()
        {
            if (audioSource != null) audioSource.PlayOneShot(hitSound);
        }

        /// <summary>
        /// Simulates light blinking.
        /// </summary>
        /// <returns></returns>
        public IEnumerator Blink(bool isFinallyLightOn)
        {
            IsBusy = true;
            {
                if (isFinallyLightOn)
                {
                    lightRay.SetActive(false);
                    yield return new WaitForSeconds(0.2f);
                    lightRay.SetActive(true);
                    lightOn = true;
                }
                else
                {
                    lightRay.SetActive(true);
                    yield return new WaitForSeconds(0.2f);
                    lightRay.SetActive(false);
                    lightOn = false;
                }
            }
            IsBusy = false;
            yield return null;
        }

        /// <summary>
        /// Simulates light blinking until it has completely shut off.
        /// </summary>
        /// <returns></returns>
        public IEnumerator BlinkToDeath()
        {
            IsBusy = true;
            {
                lightRay.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                lightRay.SetActive(true);
                yield return new WaitForSeconds(0.6f);
                lightRay.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                lightRay.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                lightRay.SetActive(false);
                lightOn = false;
                IsDead = true;
            }
            IsBusy = false;
            yield return null;
        }
        #endregion
    }
}
