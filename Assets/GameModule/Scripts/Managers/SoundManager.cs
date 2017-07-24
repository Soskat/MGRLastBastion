using LastBastion.Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages sound playing logic based on player's biofeedback.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool isEnabled = false;
        [SerializeField] private float startDelay = 10f;
        [SerializeField] private List<AudioClip> soundsHard;
        [SerializeField] private List<AudioClip> soundsLight;
        private BiofeedbackController playerBiofeedback;
        private GameObject choosenSoundSource;
        private AudioClip choosenAudioClip;
        private bool isBusy = false;
        #endregion


        #region Public fields & properties
        /// <summary>Is this sound manager (area) enabled?</summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            if (isEnabled) GameManager.instance.ActiveSoundArea = this;
            playerBiofeedback = GameManager.instance.Player.GetComponent<BiofeedbackController>();
            StartCoroutine(CooldownTimer(startDelay * 1.5f));
        }

        // Update is called once per frame
        void Update()
        {
            if (isEnabled && !isBusy)
            {
                if (GameManager.instance.BBModule.IsEnabled)
                {
                    if (playerBiofeedback.ArousalCurrentState == Biofeedback.DataState.High)
                    {
                        // play light sound:
                        choosenAudioClip = soundsLight[Random.Range(0, soundsLight.Count)];
                    }
                    else
                    {
                        // play hard sound:
                        choosenAudioClip = soundsHard[Random.Range(0, soundsHard.Count)];
                    }
                }
                else
                {
                    // play sounds at random time - but still choose the best awailable audio source
                    int x = Random.Range(0, 2);
                    if (x == 0) choosenAudioClip = soundsLight[Random.Range(0, soundsLight.Count)];
                    else choosenAudioClip = soundsHard[Random.Range(0, soundsHard.Count)];
                }
                
                choosenSoundSource = FindBestSoundSource();
                if (choosenSoundSource.GetComponent<SoundTrigger>() != null) choosenSoundSource.GetComponent<SoundTrigger>().PlaySound();
                else choosenSoundSource.GetComponent<AudioSource>().PlayOneShot(choosenAudioClip);
                StartCoroutine(CooldownTimer(startDelay));
            }

            // test:
            if (isEnabled) Debug.DrawLine(GameManager.instance.Player.transform.position, FindBestSoundSource().transform.position, Color.cyan);
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
                Vector3 playerToSoundSource = child.transform.position - GameManager.instance.Player.transform.position;
                // sound source is behind player:
                if (Vector3.Dot(playerToSoundSource, GameManager.instance.Player.transform.forward) <= 0)
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
