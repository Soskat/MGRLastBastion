using LastBastion.Biofeedback;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace LastBastion.Game
{
    [RequireComponent(typeof(Animator))]
    public class RightHand : Hand
    {
        #region Private fields
        [SerializeField] private bool doBlink;
        [SerializeField] private bool doBlinkToDeath;
        private Flashlight flashlight;
        private Animator animator;
        private int flashlightHideAnimState;
        private int flashlightDrawAnimState;
        private int flashlightReviveAnimState;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
        }

        // Use this for initialization
        new void Start()
        {
            base.Start();
            flashlight = GetComponentInChildren<Flashlight>();
            animator = GetComponent<Animator>();
            flashlightHideAnimState = Animator.StringToHash("HideFlashlight");
            flashlightDrawAnimState = Animator.StringToHash("DrawFlashlight");
            flashlightReviveAnimState = Animator.StringToHash("FlashlightRevive");
            player.SwitchLight += SwitchLight;
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();

            // TO DO: make it reacts to player's biofeedback:
            // ...
            if (doBlink && !flashlight.IsBusy)
            {
                doBlink = false;
                StartCoroutine(flashlight.Blink(true));
            }
            if (doBlinkToDeath && !flashlight.IsBusy)
            {
                doBlinkToDeath = false;
                StartCoroutine(flashlight.BlinkToDeath());
            }
            //if (flashlight.LightOn && player.ArousalState == DataState.Low && player.ArousalModifier < 0.5)
            //{
            //    StartCoroutine(flashlight.BlinkToDeath());
            //}
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Switches the light of the flashlight.
        /// </summary>
        public void SwitchLight()
        {
            if (flashlight.IsDead)
            {
                animator.applyRootMotion = false;
                animator.Play(flashlightReviveAnimState);

                flashlight.IsDead = false;
            }
            else if (!flashlight.IsBusy)
            {
                animator.applyRootMotion = false;
                if (flashlight.LightOn) animator.Play(flashlightHideAnimState);
                else animator.Play(flashlightDrawAnimState);
            }
        }

        /// <summary>
        /// Blinks the light of the flashlight.
        /// </summary>
        public void BlinkLight()
        {
            StartCoroutine(flashlight.Blink(false));
        }

        /// <summary>
        /// Turns on the light of the flashlight.
        /// </summary>
        public void TurnOnLight()
        {
            flashlight.TurnOnLight();
        }

        /// <summary>
        /// Turns off the light of the flashlight.
        /// </summary>
        public void TurnOffLight()
        {
            flashlight.TurnOffLight();
        }

        /// <summary>
        /// Plays sound when flashlight hit something.
        /// </summary>
        public void PlayFlashlightHitSound()
        {
            flashlight.PlayHitSound();
        }

        /// <summary>
        /// Plays sound when player switches light.
        /// </summary>
        public void PlayFlashlightSwitchSound()
        {
            flashlight.PlaySwitchSound();
        }

        /// <summary>
        /// Resets animator settings.
        /// </summary>
        public void ResetAnimatorState()
        {
            animator.applyRootMotion = true;
        }
        #endregion
    }
}
