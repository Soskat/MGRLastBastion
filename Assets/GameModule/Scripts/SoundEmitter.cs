using LastBastion.Game.Managers;
using LastBastion.Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool isBackgroundSound = false;
        [SerializeField] private float minDistance = 3f;
        [SerializeField] private float maxDistance = 7f;
        [SerializeField] private float interactionDistance = 5f;
        [SerializeField] private float cooldownTime = 30f;
        [SerializeField] private List<AudioClip> sounds;
        private bool isBusy = false;
        private float distance;
        private float newDistance;
        private AudioSource audioSource;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            // set up some settings for audio source:
            audioSource.spatialBlend = 1.0f;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            if (isBackgroundSound)
            {
                audioSource.loop = true;
                audioSource.playOnAwake = true;
                audioSource.clip = GetRandomSound();
                audioSource.Play();
            }
            else
            {
                audioSource.loop = false;
                audioSource.playOnAwake = false;
            }
            // set up other variables:
            distance = maxDistance;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isBusy)
            {
                // check if player is in range:
                newDistance = (transform.position - GameManager.instance.Player.transform.position).magnitude;
                if (newDistance <= interactionDistance)
                {
                    // distance has started to increase - prepare to playing a sound:
                    if (newDistance > distance)
                    {
                        float delay;
                        // biofeedback module ON:
                        if (GameManager.instance.BBModule.IsEnabled)
                        {
                            // the more player is anxious, the closer to him sound plays:
                            delay = 2f * GameManager.instance.Player.GetComponent<BiofeedbackController>().ArousalCurrentModifier;
                        }
                        // biofeedback module OFF:
                        else delay = Random.Range(1.0f, 3.0f);
                        isBusy = true;
                        StartCoroutine(TriggerSound(delay));
                    }
                    else
                    {
                        distance = newDistance;
                    }
                }
                else
                {
                    distance = maxDistance;
                }
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                PlaySound();
                isBusy = true;
                StartCoroutine(CooldownTimer(cooldownTime));
            }
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Gets random audio clip from a list.
        /// </summary>
        /// <returns>Random audio clip</returns>
        private AudioClip GetRandomSound()
        {
            if (sounds.Count > 1) return sounds[Random.Range(0, sounds.Count)];
            else if (sounds.Count == 1) return sounds[0];
            else return null;
        }

        /// <summary>
        /// Plays one shot of random audio clip after given delay.
        /// </summary>
        /// <param name="delay">The delay</param>
        /// <returns></returns>
        private IEnumerator TriggerSound(float delay)
        {
            yield return new WaitForSeconds(delay);
            // play random sound:
            PlaySound();
            // reset variables:
            distance = maxDistance;
            StartCoroutine(CooldownTimer(cooldownTime));
        }

        /// <summary>
        /// Simple cooldown timer.
        /// </summary>
        /// <param name="cooldown"></param>
        /// <returns></returns>
        private IEnumerator CooldownTimer(float cooldown)
        {
            yield return new WaitForSeconds(cooldown);
            isBusy = false;
            yield return null;
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Plays random sound.
        /// </summary>
        public void PlaySound()
        {
            AudioClip randomSound = GetRandomSound();
            if (randomSound != null) audioSource.PlayOneShot(randomSound);
        }
        #endregion
    }
}
