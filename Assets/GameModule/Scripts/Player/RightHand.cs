using LastBastion.Biofeedback;
using LastBastion.Game.Managers;
using LastBastion.Game.ObjectInteraction;
using System.Collections;
using UnityEngine;


namespace LastBastion.Game.Player
{
    /// <summary>
    /// Component that represents Player's right hand.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class RightHand : Hand
    {
        #region Private fields
        //[SerializeField] private int timeSinceLastBlink = 0;
        //[SerializeField] private int timeSinceLastBlinkToDeath = 0;
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

            // to uncomment -------------------------------------------------------------------------------
            //if (!GameManager.instance.BBModule.IsEnabled)
            //{
            //    StartCoroutine(BlinkFlashlight());
            //    StartCoroutine(BlinkFlashlightToDeath());
            //}
            //else
            //{
            //    timeSinceLastBlink = GetRandomShortTime() * 100;
            //    timeSinceLastBlinkToDeath = GetRandomLongTime() * 100;
            //}
        }

        // Update is called once per frame
        new void Update()
        {
            base.Update();

            //// update game mechanics based on current player's arousal:
            //if (GameManager.instance.BBModule.IsEnabled)
            //{
            //    //...
            //}

            // move it above ---------------------------------------------------------------------
            //switch (player.ArousalCurrentState)
            //{
            //    case DataState.High:
            //        break;

            //    case DataState.Medium:
            //        if (flashlight.LightOn && timeSinceLastBlink > 0) timeSinceLastBlink--;

            //        if (player.ArousalCurrentModifier < 1.0)
            //        {
            //            if (flashlight.LightOn && !flashlight.IsBusy && timeSinceLastBlink == 0)
            //            {
            //                StartCoroutine(flashlight.Blink(true));
            //                timeSinceLastBlink = GetRandomShortTime() * 100;
            //            }
            //        }
            //        break;

            //    case DataState.Low:
            //        if (flashlight.LightOn && timeSinceLastBlinkToDeath > 0) timeSinceLastBlinkToDeath--;

            //        if (flashlight.LightOn && !flashlight.IsBusy && timeSinceLastBlinkToDeath == 0)
            //        {
            //            StartCoroutine(flashlight.BlinkToDeath());
            //            timeSinceLastBlinkToDeath = GetRandomLongTime() * 100;
            //        }
            //        break;

            //    default: break;
            //}


            // manual test: ---------------------------
            if (Input.GetKeyDown(KeyCode.B) && !flashlight.IsBusy) StartCoroutine(flashlight.Blink(true));
            if (Input.GetKeyDown(KeyCode.N) && !flashlight.IsBusy) StartCoroutine(flashlight.BlinkToDeath());
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Switches the light of the flashlight.
        /// </summary>
        public void SwitchLight()
        {
            // change it later to use actual biofeedback data:
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


        #region Private methods
        /// <summary>
        /// Coroutine that triggers flashlight blinking from time to time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator BlinkFlashlight()
        {
            int counter = GetRandomShortTime();
            yield return new WaitForSeconds(counter);
            //doBlink = true;
            StartCoroutine(BlinkFlashlight());
        }

        /// <summary>
        /// Coroutine that triggers flashlight blinking to death from time to time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator BlinkFlashlightToDeath()
        {
            int counter = GetRandomLongTime();
            yield return new WaitForSeconds(counter);
            //doBlink = true;
            StartCoroutine(BlinkFlashlightToDeath());
        }

        /// <summary>
        /// Gets random int time from range [20, 40).
        /// </summary>
        /// <returns>Time in seconds</returns>
        private int GetRandomShortTime()
        {
            return Random.Range(20, 40);
        }

        /// <summary>
        /// Gets random int time from range [50, 70).
        /// </summary>
        /// <returns>Time in seconds</returns>
        private int GetRandomLongTime()
        {
            return Random.Range(50, 70);
        }
        #endregion
    }
}
