using LastBastion.Analytics;
using LastBastion.Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents television behaviour.
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class Television : MonoBehaviour
    {
        #region Private fields
        /// <summary>Activation range.</summary>
        [SerializeField] private float activationRange = 15f;
        /// <summary>Distant to occur the TV-on event (second activation range).</summary>
        [SerializeField] private float distanceToEvent = 4f;
        /// <summary>Sound of tv's static buzz.</summary>
        [SerializeField] private AudioClip staticBuzzSound;
        /// <summary>List of channel sounds.</summary>
        [SerializeField] private List<AudioClip> channelSounds;
        /// <summary>Audio source component that plays channel sounds.</summary>
        [SerializeField] private AudioSource channelAudioSource;
        /// <summary>Index of currently used channel sound.</summary>
        private int channelSoundIndex = 0;
        /// <summary>Assigned audio source.</summary>
        private AudioSource audioSource;
        /// <summary>Monitor light source.</summary>
        private Light monitorLight;
        /// <summary>Assigned material.</summary>
        private Material material;
        /// <summary>Color of the tv's monitor glass.</summary>
        private Color glassColor;
        /// <summary>Hue component of the glass color.</summary>
        private float hue;
        /// <summary>Saturation component of the glass color.</summary>
        private float saturation;
        /// <summary>Value component of the glass color.</summary>
        private float value;
        /// <summary>Is tv turned on?</summary>
        private bool isOn;
        /// <summary>Has tv been activated?</summary>
        private bool wasActivated;
        /// <summary>Can tv be turned on?</summary>
        private bool canTurnOn;
        /// <summary>Can tv be turned off?</summary>
        private bool canTurnOff;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(staticBuzzSound);
            Assert.IsNotNull(channelAudioSource);
        }

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = staticBuzzSound;
            // set up turned-off mode:
            monitorLight = GetComponentInChildren<Light>();
            material = GetComponent<Renderer>().material;
            glassColor = material.GetColor("_EmissionColor");
            TurnOffTV();
            // set up collider trigger:
            GetComponent<SphereCollider>().isTrigger = true;
            GetComponent<SphereCollider>().radius = activationRange;
            isOn = false;
            wasActivated = false;
            canTurnOn = false;
            canTurnOff = false;
        }

        // Update is called once per frame
        void Update()
        {
            // for testing purposes:
            if (GameManager.instance.DebugMode && Input.GetKeyDown(KeyCode.X))
            {
                if (isOn)
                {
                    TurnOnTV();
                    isOn = false;
                }
                else
                {
                    TurnOffTV();
                    isOn = true;
                }
            }
        }

        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (!wasActivated)
                {
                    // biofeedback ON:
                    if (GameManager.instance.BiofeedbackMode == BiofeedbackMode.BiofeedbackON && GameManager.instance.BBModule.IsEnabled)
                    {
                        GetComponent<SphereCollider>().radius += GameManager.instance.BBModule.ArousalModifier * distanceToEvent;
                    }
                    // biofeedback OFF:
                    else
                    {
                        GetComponent<SphereCollider>().radius += Random.Range(0.1f, 1.5f) * distanceToEvent;
                    }
                    wasActivated = true;
                    canTurnOn = true;
                }
            }
        }

        // OnTriggerExit is called when the Collider other has stopped touching the trigger
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (canTurnOff)
                {
                    TurnOffTV();
                    canTurnOff = false;
                }
                if (canTurnOn)
                {
                    // turn on TV:
                    TurnOnTV();
                    // set new radius to sphere trigger:
                    // biofeedback ON:
                    if (GameManager.instance.BiofeedbackMode == BiofeedbackMode.BiofeedbackON && GameManager.instance.BBModule.IsEnabled)
                    {
                        GetComponent<SphereCollider>().radius += GameManager.instance.BBModule.ArousalModifier * activationRange * 2f;
                    }
                    // biofeedback OFF:
                    else
                    {
                        GetComponent<SphereCollider>().radius += Random.Range(0.1f, 1.5f) * activationRange * 2f;
                    }
                    canTurnOn = false;
                    canTurnOff = true;
                }
            }
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Turns off the television.
        /// </summary>
        private void TurnOffTV()
        {
            // stop all coroutines:
            StopAllCoroutines();
            // turn off the light:
            monitorLight.gameObject.SetActive(false);
            material.SetColor("_EmissionColor", Color.black);
            // stop playing sounds if needed:
            audioSource.Stop();
            channelAudioSource.Stop();
        }

        /// <summary>
        /// Turns on the television.
        /// </summary>
        private void TurnOnTV()
        {
            // save info about event:
            if (GameManager.instance.AnalyticsEnabled) LevelManager.instance.AddGameEvent(Analytics.EventType.TV);

            // enable light:
            monitorLight.gameObject.SetActive(true);
            monitorLight.intensity = 1.5f;
            // change monitor material color:
            material.SetColor("_EmissionColor", glassColor);
            material.SetFloat("_EmissionColorUI", monitorLight.intensity - 1f);
            // start blinking:
            StartCoroutine(Blink());
            // start playing sounds:
            audioSource.Play();
            StartCoroutine(ChannelSounds());
        }

        /// <summary>
        /// Coroutine that simulates blinking of the television monitor.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Blink()
        {
            // get current hue, saturation and value of material color:
            Color.RGBToHSV(material.GetColor("_EmissionColor"), out hue, out saturation, out value);
            // set new light intensity and color value:
            monitorLight.intensity = Random.Range(1.0f, 2.0f);
            value = monitorLight.intensity - 1f;
            // set new material color:
            material.SetColor("_EmissionColor", Color.HSVToRGB(hue, saturation, value));
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            StartCoroutine(Blink());
        }

        /// <summary>
        /// Coroutine that plays tv channel sounds from time to time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChannelSounds()
        {
            channelAudioSource.PlayOneShot(channelSounds[channelSoundIndex]);
            channelSoundIndex++;
            if (channelSoundIndex >= channelSounds.Count) channelSoundIndex = 0;
            yield return new WaitForSeconds(Random.Range(4.0f, 6.0f));
            StartCoroutine(ChannelSounds());
        }
        #endregion
    }
}