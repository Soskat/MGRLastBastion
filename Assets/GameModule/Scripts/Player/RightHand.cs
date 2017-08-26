using LastBastion.Analytics;
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
        [SerializeField] private int timeSinceLastBlink = 0;
        [SerializeField] private int timeSinceLastBlinkToDeath = 0;
        private Flashlight flashlight;
        private Animator animator;
        private int flashlightHideAnimState;
        private int flashlightDrawAnimState;
        private int flashlightReviveAnimState;
        private float deltaTime;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        new void Start()
        {
            base.Start();
            flashlight = GetComponentInChildren<Flashlight>();
            animator = GetComponent<Animator>();
            flashlightHideAnimState = Animator.StringToHash("HideFlashlight");
            flashlightDrawAnimState = Animator.StringToHash("DrawFlashlight");
            flashlightReviveAnimState = Animator.StringToHash("FlashlightRevive");
            // calculate current fps value:
            deltaTime = 1.0f / Time.deltaTime;

            //// if biofeedback is off set up the blink events at random time:
            //if (GameManager.instance.BiofeedbackMode == BiofeedbackMode.BiofeedbackOFF || !GameManager.instance.BBModule.IsEnabled)
            //{
            //    StartCoroutine(BlinkFlashlight());
            //    StartCoroutine(BlinkFlashlightToDeath());
            //}
            //// if biofeedback is on set up initial counters values:
            //else
            //{
            //    timeSinceLastBlink = GetRandomSecondsShortRange() * (int)deltaTime;
            //    timeSinceLastBlinkToDeath = GetRandomSecondsLongRange() * (int)deltaTime;
            //}
        }

        // Update is called once per frame
        new void Update()
        {
            // if level outro is playing, skip all calculations:
            if (LevelManager.instance.IsOutroOn)
            {
                // if flashilight was turned on before outro has started, turn it off:
                if (flashlight.LightOn && !flashlight.IsBusy)
                {
                    flashlight.IsBusy = true;
                    StartCoroutine(TurnOffFlashlightOnOutro());
                }
                if (flashlight.IsDead)
                {
                    StartCoroutine(HideFlashlightOnOutro());
                }
                return;
            }

            // player is focused on object in his hands - skip all calculations:
            if (LevelManager.instance.Player.GetComponent<InteractionController>().IsFocused) return;

            base.Update();

            // manage game input: -------------------------------------------------------------
            if (Input.GetKeyDown(KeyCode.R)) SwitchLight();

            //// update game mechanics based on player's current arousal:
            //if (GameManager.instance.BiofeedbackMode == BiofeedbackMode.BiofeedbackON && GameManager.instance.BBModule.IsEnabled)
            //{
            //    deltaTime = 1.0f / Time.deltaTime;
            //    switch (GameManager.instance.BBModule.ArousalState)
            //    {
            //        case DataState.High:
            //            if (flashlight.LightOn && timeSinceLastBlink > 0) timeSinceLastBlink--;
            //            if (flashlight.LightOn && !flashlight.IsBusy && timeSinceLastBlink <= 0)
            //            {
            //                StartCoroutine(flashlight.Blink(true));
            //                timeSinceLastBlink = GetRandomSecondsShortRange() * (int)deltaTime;
            //            }
            //            break;

            //        case DataState.Medium:
            //        case DataState.Low:
            //            if (flashlight.LightOn && timeSinceLastBlinkToDeath > 0) timeSinceLastBlinkToDeath--;
            //            if (flashlight.LightOn && !flashlight.IsBusy && timeSinceLastBlinkToDeath <= 0)
            //            {
            //                StartCoroutine(flashlight.BlinkToDeath());
            //                timeSinceLastBlinkToDeath = GetRandomSecondsLongRange() * (int)deltaTime;
            //            }
            //            break;

            //        default: break;
            //    }
            //}

            // manual test: ---------------------------
            if (GameManager.instance.DebugMode)
            {
                if (Input.GetKeyDown(KeyCode.B) && !flashlight.IsBusy) StartCoroutine(flashlight.Blink(true));
                if (Input.GetKeyDown(KeyCode.N) && !flashlight.IsBusy) StartCoroutine(flashlight.BlinkToDeath());
            }
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
            if (!LevelManager.instance.IsOutroOn) flashlight.PlaySwitchSound();
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
            yield return new WaitForSeconds(GetRandomSecondsShortRange());
            StartCoroutine(flashlight.Blink(true));
            StartCoroutine(BlinkFlashlight());
        }

        /// <summary>
        /// Coroutine that triggers flashlight blinking to death from time to time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator BlinkFlashlightToDeath()
        {
            yield return new WaitForSeconds(GetRandomSecondsLongRange());
            StartCoroutine(flashlight.BlinkToDeath());
            StartCoroutine(BlinkFlashlightToDeath());
        }

        /// <summary>
        /// Gets random seconds from range [30, 90).
        /// </summary>
        /// <returns>Time in seconds</returns>
        private int GetRandomSecondsShortRange()
        {
            return Random.Range(30, 90);
        }

        /// <summary>
        /// Gets random seconds from range [120, 180).
        /// </summary>
        /// <returns>Time in seconds</returns>
        private int GetRandomSecondsLongRange()
        {
            return Random.Range(120, 180);
        }

        /// <summary>
        /// Turns off flashlight after outro has started playing.
        /// </summary>
        /// <returns></returns>
        private IEnumerator TurnOffFlashlightOnOutro()
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
            StartCoroutine(flashlight.BlinkToDeath());
        }

        /// <summary>
        /// Hides flashlight during outro after small delay.
        /// </summary>
        /// <returns></returns>
        private IEnumerator HideFlashlightOnOutro()
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
            animator.applyRootMotion = false;
            animator.Play(flashlightHideAnimState);
            flashlight.IsDead = false;
        }
        #endregion
    }
}
