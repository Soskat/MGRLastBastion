using LastBastion.Game.Managers;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that manages behaviour of light switch object.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class LightSwitch : MonoBehaviour, IInteractiveObject
    {
        #region Private fields
        ///// <summary>Is light switch enabled?</summary>
        //[SerializeField] private bool isEnabled = true;
        /// <summary>Assigned <see cref="LightManager"/> component.</summary>
        [SerializeField] private LightManager lightManager;
        /// <summary>Sound of turning light on.</summary>
        [SerializeField] private AudioClip turnOnSound;
        /// <summary>Sound of turning light off.</summary>
        [SerializeField] private AudioClip turnOffSound;
        /// <summary>Assigned <see cref="Animator"/> component.</summary>
        private Animator animator;
        /// <summary>Assigned <see cref="AudioSource"/> component.</summary>
        private AudioSource audioSource;
        /// <summary>ID of animator's trigger that initiates switching button.</summary>
        private int switchButtonTrigger;
        /// <summary>Is light switch busy?</summary>
        private bool isBusy = false;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(lightManager);
            Assert.IsNotNull(turnOnSound);
            Assert.IsNotNull(turnOffSound);
        }

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            switchButtonTrigger = Animator.StringToHash("SwitchButton");
            audioSource = GetComponent<AudioSource>();
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
                animator.SetTrigger(switchButtonTrigger);
                // infrom that light switch was used:
                LevelManager.instance.UsedLightSwitch();
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
        public void SwitchLightState()
        {
            lightManager.SwitchLights();
            //if (isEnabled) lightManager.SwitchLights();
        }

        /// <summary>
        /// Plays sound of turning on the light.
        /// </summary>
        public void PlayTurnOnSound()
        {
            audioSource.PlayOneShot(turnOnSound);
        }

        /// <summary>
        /// Plays sound of turning off the light.
        /// </summary>
        public void PlayTurnOffSound()
        {
            audioSource.PlayOneShot(turnOffSound);
        }
        #endregion
    }
}