using LastBastion.Analytics;
using LastBastion.Biofeedback;
using LastBastion.Game.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LastBastion.Game.Player
{
    /// <summary>
    /// Component that manages Player behaviour.
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private float arousalCurrentModifier;
        [SerializeField] private DataState arousalCurrentState;
        [SerializeField] private float arousalOldModifier;
        [SerializeField] private DataState arousalOldState;
        [SerializeField] private bool isFlashlightEquipped = true;
        [SerializeField] private AudioClip heartSound;
        [SerializeField] private AudioClip breathSound;
        [SerializeField] private AudioSource biofeedbackAudio;
        [SerializeField] private bool playHeartSound;
        [SerializeField] private bool playBreathSound;
        private bool heartSoundOn = false;
        private bool breathSoundOn = false;
        #endregion


        #region Public fields & properties
        /// <summary>Current arousal modifier.</summary>
        public float ArousalCurrentModifier { get { return arousalCurrentModifier; } }
        /// <summary>Current arousal state based on <see cref="arousalModifier"/>.</summary>
        public DataState ArousalCurrentState { get { return arousalCurrentState; } }
        /// <summary>Old arousal modifier.</summary>
        public float ArousalOldtModifier { get { return arousalOldModifier; } }
        /// <summary>Old arousal state based on <see cref="arousalModifier"/>.</summary>
        public DataState ArousalOldState { get { return arousalOldState; } }
        #endregion


        #region Public actions
        /// <summary>Informs that player want to switch light in the flashlight</summary>
        public Action SwitchLight;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(heartSound);
            Assert.IsNotNull(breathSound);
            Assert.IsNotNull(biofeedbackAudio);
        }

        // Use this for initialization
        void Start()
        {
            GameManager.instance.BBModule.BiofeedbackDataChanged += data => UpdatePlayerState(data);            
        }

        // Update is called once per frame
        void Update()
        {
            // manage game input: -------------------------------------------------------------

            if (isFlashlightEquipped && Input.GetKeyDown(KeyCode.R))
            {
                SwitchLight();
            }


            // activate mechanics based on biofeedback if module is enabled -------------------

            if (GameManager.instance.BBModule.IsEnabled)
            {
                if (ArousalCurrentState == DataState.High)
                {
                    playBreathSound = true;
                    playHeartSound = false;
                }
                else if (ArousalCurrentState == DataState.Medium && ArousalOldState == DataState.High)
                {
                    playBreathSound = false;
                    if (arousalCurrentModifier > 1.0f) playHeartSound = true;
                    else playHeartSound = false;
                }
                else
                {
                    playBreathSound = false;
                    playHeartSound = false;
                }
            }
            // randomise events:
            else
            {
                // ...
            }


            // Update player mechanics: -------------------------------------------------------

            if (playHeartSound)
            {
                if (!heartSoundOn)
                {
                    PlaySound(heartSound);
                    heartSoundOn = true;
                }
            }
            else if (heartSoundOn)
            {
                biofeedbackAudio.Stop();
                heartSoundOn = false;
            }
            
            if (playBreathSound)
            {
                if (!breathSoundOn)
                {
                    PlaySound(breathSound);
                    breathSoundOn = true;
                }
            }
            else if (breathSoundOn)
            {
                biofeedbackAudio.Stop();
                breathSoundOn = false;
            }
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Updates Player's biofeedback state.
        /// </summary>
        /// <param name="data"></param>
        private void UpdatePlayerState(BiofeedbackData data)
        {
            arousalOldModifier = arousalCurrentModifier;
            arousalOldState = arousalCurrentState;
            arousalCurrentModifier = data.ArousalModifier;
            arousalCurrentState = data.ArousalState;
        }
        
        private void PlaySound(AudioClip clip)
        {
            if (biofeedbackAudio.isPlaying) biofeedbackAudio.Stop();
            biofeedbackAudio.clip = clip;
            biofeedbackAudio.Play();
        }
        #endregion
    }
}