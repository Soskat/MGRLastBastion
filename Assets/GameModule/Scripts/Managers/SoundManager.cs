using LastBastion.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages logic of playing sounds in area based on player's biofeedback.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class SoundManager : MonoBehaviour
    {
        #region Private fields
        /// <summary>Is sound manager active?</summary>
        [SerializeField] private bool isActive = false;
        /// <summary>Is sound manager busy?</summary>
        [SerializeField] private bool isBusy = false;
        /// <summary>Start delay for playing any sounds.</summary>
        [SerializeField] private float startDelay = 10f;
        /// <summary>List of hard version of sounds for area.</summary>
        [SerializeField] private List<AudioClip> soundsHard;
        /// <summary>List of light version of sounds for area.</summary>
        [SerializeField] private List<AudioClip> soundsLight;
        /// <summary>Chosen sound source to play sound.</summary>
        private GameObject chosenSoundSource;
        /// <summary>Chosen audio clip to play.</summary>
        private AudioClip chosenAudioClip;
        /// <summary>Sound manager's cooldown time.</summary>
        private float cooldownTime = 1f;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            StartCoroutine(CooldownTimer(startDelay * 1.5f));
        }

        // Update is called once per frame
        void Update()
        {
            // if level outro is playing, skip all calculations:
            if (LevelManager.instance.IsOutroOn) return;
            
            // debug mode:
            if (GameManager.instance.DebugMode && isActive && isBusy)
            {
                Debug.DrawLine(LevelManager.instance.Player.transform.position, FindBestSoundSource().transform.position, Color.green);
            }

            // if sound manager is active and not working/waiting till cooldown ends:
            if (isActive && !isBusy)
            {
                // biofeedback ON:
                if (GameManager.instance.BiofeedbackMode == BiofeedbackMode.BiofeedbackON && GameManager.instance.BBModule.IsEnabled)
                {
                    if (GameManager.instance.BBModule.ArousalState == Biofeedback.DataState.High)
                    {
                        // play light sound:
                        chosenAudioClip = soundsLight[Random.Range(0, soundsLight.Count)];
                    }
                    else
                    {
                        // play hard sound:
                        chosenAudioClip = soundsHard[Random.Range(0, soundsHard.Count)];
                    }
                    cooldownTime = startDelay * 2f * GameManager.instance.BBModule.ArousalModifier;
                }
                // biofeedback OFF:
                else
                {
                    // play sounds at random time - but still choose the best available audio source:
                    int x = Random.Range(0, 2);
                    if (x == 0) chosenAudioClip = soundsLight[Random.Range(0, soundsLight.Count)];
                    else chosenAudioClip = soundsHard[Random.Range(0, soundsHard.Count)];
                    cooldownTime = startDelay * Random.Range(1.0f, 3.0f);
                }
                
                chosenSoundSource = FindBestSoundSource();
                if (chosenSoundSource.GetComponent<SoundTrigger>() != null) chosenSoundSource.GetComponent<SoundTrigger>().PlaySound();
                else chosenSoundSource.GetComponent<AudioSource>().PlayOneShot(chosenAudioClip);
                // start cooldown timer:
                StartCoroutine(CooldownTimer(cooldownTime));
                
                // save info about event:
                if (GameManager.instance.AnalyticsEnabled) LevelManager.instance.AddGameEvent(Analytics.EventType.Sound);
            }
        }

        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            isActive = true;
            isBusy = false;
        }

        // OnTriggerExit is called when the Collider other has stopped touching the trigger
        private void OnTriggerExit(Collider other)
        {
            isActive = false;
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Finds best available game object with audio source component.
        /// </summary>
        /// <returns>Game object with audio source component</returns>
        private GameObject FindBestSoundSource()
        {
            // choose the best sound source (which is one behind the Player and the closest to him):
            GameObject soundSource = null, soundSourceSecond = null;
            float minDistance = 100f, minDistanceSecond = 100f;
            foreach(Transform child in transform)
            {
                Vector3 playerToSoundSource = child.transform.position - LevelManager.instance.Player.transform.position;
                // sound source is behind player:
                if (Vector3.Dot(playerToSoundSource, LevelManager.instance.Player.transform.forward) <= 0)
                {
                    if (playerToSoundSource.magnitude < minDistance)
                    {
                        soundSource = child.gameObject;
                        minDistance = playerToSoundSource.magnitude;
                    }
                }
                // if sound source is in front of player:
                else
                {
                    if (playerToSoundSource.magnitude < minDistanceSecond)
                    {
                        soundSourceSecond = child.gameObject;
                        minDistanceSecond = playerToSoundSource.magnitude;
                    }
                }
            }

            // choose the best sound source:
            if (soundSource != null) return soundSource;
            else return soundSourceSecond;
        }

        /// <summary>
        /// Sets <see cref="isBusy"/> flag to true for specific period of time.
        /// </summary>
        /// <param name="cooldownTime">Time to wait</param>
        /// <returns></returns>
        private IEnumerator CooldownTimer(float cooldownTime)
        {
            isBusy = true;
            yield return new WaitForSeconds(cooldownTime);
            isBusy = false;
        }
        #endregion
    }
}
