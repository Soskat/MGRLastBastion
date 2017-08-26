using LastBastion.Game.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that manages flashlight behaviour.
    /// </summary>
    public class Flashlight : MonoBehaviour
    {
        #region Private fields
        /// <summary>Is light on?</summary>
        [SerializeField] private bool lightOn = false;
        /// <summary>Game object that represents the flashlight's light ray.</summary>
        [SerializeField] private GameObject lightRay;
        /// <summary>Sound of switching flashlight on.</summary>
        [SerializeField] private AudioClip switchOnSound;
        /// <summary>Sound of switching flashlight off.</summary>
        [SerializeField] private AudioClip switchOffSound;
        /// <summary>Sound of flashlight hitting something.</summary>
        [SerializeField] private AudioClip hitSound;
        /// <summary>Assigned audio source.</summary>
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
        /// Turns on the light.
        /// </summary>
        public void TurnOnLight()
        {
            lightOn = true;
            lightRay.SetActive(true);
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
        /// <param name="isFinallyLightOn">Is light turned on after blinking?</param>
        /// <returns></returns>
        public IEnumerator Blink(bool isFinallyLightOn)
        {
            // save info about event:
            if (GameManager.instance.AnalyticsEnabled) LevelManager.instance.AddGameEvent(Analytics.EventType.Flashlight);
            // animate blinking:
            IsBusy = true;
            {
                if (isFinallyLightOn)
                {
                    lightRay.SetActive(false);

                    // activate static decals:
                    if (GameManager.instance.ActiveRoom != null) GameManager.instance.ActiveRoom.GetComponentInChildren<DecalsStaticManager>().ActivateDecals();

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
            // save info about event:
            if (GameManager.instance.AnalyticsEnabled) LevelManager.instance.AddGameEvent(Analytics.EventType.Flashlight);
            // animate blinking to death:
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

                // activate static decals:
                if (GameManager.instance.ActiveRoom != null) GameManager.instance.ActiveRoom.GetComponentInChildren<DecalsStaticManager>().ActivateDecals();
            }
            IsBusy = false;
            yield return null;
        }
        #endregion
    }
}