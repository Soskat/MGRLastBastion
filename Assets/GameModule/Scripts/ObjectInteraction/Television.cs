using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents television behaviour.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class Television : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool isOn = false; // ------------------- for testing
        [SerializeField] private AudioClip staticBuzzSound;
        [SerializeField] private List<AudioClip> channelSounds;
        [SerializeField]
        private AudioSource channelAudioSource;
        private int channelSoundIndex = 0;
        private AudioSource audioSource;
        private Light monitorLight;
        private Material material;
        private Color glassColor;
        private float hue;
        private float saturation;
        private float value;
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
        }

        // Update is called once per frame
        void Update()
        {
            // for testing purposes:
            if (Input.GetKeyDown(KeyCode.X))
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
