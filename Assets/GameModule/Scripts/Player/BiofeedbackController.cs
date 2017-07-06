using LastBastion.Biofeedback;
using LastBastion.Game.Managers;
using System;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.Player
{
    /// <summary>
    /// Component that manages Player biofeedback behaviour.
    /// </summary>
    public class BiofeedbackController : MonoBehaviour
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
            switch (ArousalCurrentState)
            {
                case DataState.High:
                    playBreathSound = true;
                    playHeartSound = false;
                    break;

                case DataState.Medium:
                    playBreathSound = false;
                    if (arousalCurrentModifier > 1.0f) playHeartSound = true;
                    else playHeartSound = false;
                    break;

                case DataState.Low:
                    playBreathSound = false;
                    playHeartSound = false;
                    break;

                default: break;
            }

            //if (GameManager.instance.BBModule.IsEnabled)
            //{
            //    // ...
            //}
            //// randomise events:
            //else
            //{
            //    // ...
            //}


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
            // save current arousal as old:
            arousalOldModifier = arousalCurrentModifier;
            arousalOldState = arousalCurrentState;
            // update current arousal:
            arousalCurrentModifier = data.ArousalModifier;
            arousalCurrentState = data.ArousalState;
        }
        
        /// <summary>
        /// If biofeedback audio source is enabled, plays given clip.
        /// </summary>
        /// <param name="clip">Sound to play</param>
        private void PlaySound(AudioClip clip)
        {
            if (biofeedbackAudio.isPlaying) biofeedbackAudio.Stop();
            biofeedbackAudio.clip = clip;
            biofeedbackAudio.Play();
        }
        #endregion
    }
}