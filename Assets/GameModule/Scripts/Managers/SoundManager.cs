using LastBastion.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LastBastion.Game;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages sounds playing logic based on player's biofeedback.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class SoundManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool isActive = false;
        [SerializeField] private bool isBusy = false;
        [SerializeField] private float startDelay = 10f;
        [SerializeField] private List<AudioClip> soundsHard;
        [SerializeField] private List<AudioClip> soundsLight;
        private GameObject choosenSoundSource;
        private AudioClip choosenAudioClip;
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

            if (isActive && !isBusy)
            {
                // biofeedback ON:
                if (GameManager.instance.BiofeedbackMode == BiofeedbackMode.BiofeedbackON && GameManager.instance.BBModule.IsEnabled)
                {
                    if (GameManager.instance.BBModule.ArousalState == Biofeedback.DataState.High)
                    {
                        // play light sound:
                        choosenAudioClip = soundsLight[Random.Range(0, soundsLight.Count)];
                    }
                    else
                    {
                        // play hard sound:
                        choosenAudioClip = soundsHard[Random.Range(0, soundsHard.Count)];
                    }
                    cooldownTime = startDelay * GameManager.instance.BBModule.ArousalModifier;
                }
                // biofeedback OFF:
                else
                {
                    // play sounds at random time - but still choose the best awailable audio source
                    int x = Random.Range(0, 2);
                    if (x == 0) choosenAudioClip = soundsLight[Random.Range(0, soundsLight.Count)];
                    else choosenAudioClip = soundsHard[Random.Range(0, soundsHard.Count)];
                    cooldownTime = startDelay * Random.Range(0.5f, 1.5f);
                }
                
                choosenSoundSource = FindBestSoundSource();
                if (choosenSoundSource.GetComponent<SoundTrigger>() != null) choosenSoundSource.GetComponent<SoundTrigger>().PlaySound();
                else choosenSoundSource.GetComponent<AudioSource>().PlayOneShot(choosenAudioClip);
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
        /// Finds best awailable game object with audio source.
        /// </summary>
        /// <returns>Game object with audio source component</returns>
        private GameObject FindBestSoundSource()
        {
            // choose the best sound source:
            // -> which is one behind the Player and the closest to him:
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
        /// Sets <see cref="isBusy"/> flag to true for specific time.
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
