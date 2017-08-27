using LastBastion.Game.Plot;
using UnityEngine;
using UnityEngine.Assertions;

namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents behaviour of the door switch.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class DoorSwitch : MonoBehaviour, IInteractiveObject
    {
        #region Private fields
        /// <summary>Assigned <see cref="Door"/> component.</summary>
        [SerializeField] private Door door;
        /// <summary>Sound of pushing the button.</summary>
        [SerializeField] private AudioClip pushButtonSound;
        /// <summary>Sound of buzzer alarm.</summary>
        [SerializeField] private AudioClip buzzerSound;
        /// <summary>Has <see cref="PlotGoal"/> component need to have a trigger?</summary>
        [SerializeField] private bool hasTriggeringGoal = false;
        /// <summary>Other <see cref="PlotGoal"/> component that triggers assigned plot goal.</summary>
        [SerializeField] private PlotGoal triggeringGoal;
        /// <summary>Assigned <see cref="Animator"/> component.</summary>
        private Animator animator;
        /// <summary>Assigned <see cref="AudioSource"/> component.</summary>
        private AudioSource audioSource;
        /// <summary>ID of animator's trigger that initiates pushing button animation.</summary>
        private int pushedButtonTrigger;
        /// <summary>Is button busy?</summary>
        private bool isBusy = false;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(door);
            Assert.IsNotNull(pushButtonSound);
            Assert.IsNotNull(buzzerSound);
            if (hasTriggeringGoal) Assert.IsNotNull(triggeringGoal);
        }

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            pushedButtonTrigger = Animator.StringToHash("PushedButton");
            audioSource = GetComponent<AudioSource>();
            if (hasTriggeringGoal)
            {
                triggeringGoal.Triggered += () => {
                    // if door is already open, activate next plot goal:
                    if (!door.IsLocked && GetComponent<PlotGoal>() != null) GetComponent<PlotGoal>().Activate();
                };
            }
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Implementation of <see cref="IInteractiveObject"/> interface method.
        /// </summary>
        public void Interact()
        {
            if (!isBusy)
            {
                animator.SetTrigger(pushedButtonTrigger);
                // activate plot goal if any assigned:
                if (GetComponent<PlotGoal>() != null) GetComponent<PlotGoal>().Activate();
            }
        }

        /// <summary>
        /// Sets <see cref="isBusy"/> flag to true.
        /// </summary>
        public void SetBusyOn()
        {
            isBusy = true;
        }

        /// <summary>
        /// Sets <see cref="isBusy"/> flag to false.
        /// </summary>
        public void SetBusyOff()
        {
            isBusy = false;
        }

        /// <summary>
        /// Switches watched door state.
        /// </summary>
        public void SwitchDoorState()
        {
            if (door.IsLocked) door.IsLocked = false;
            else
            {
                if (!door.IsClosed) PlayBuzzerSound();
                else door.IsLocked = true;
            }
        }

        /// <summary>
        /// Plays sound of pushing the button.
        /// </summary>
        public void PlayPushSound()
        {
            audioSource.PlayOneShot(pushButtonSound);
        }

        /// <summary>
        /// Plays sound of opened door buzzer alarm.
        /// </summary>
        public void PlayBuzzerSound()
        {
            audioSource.PlayOneShot(buzzerSound);
        }
        #endregion
    }
}