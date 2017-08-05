using LastBastion.Game.Managers;
using LastBastion.Game.ObjectInteraction;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.ObjectInteraction
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class LightSwitch : MonoBehaviour, IInteractiveObject
    {
        #region Private fields
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private LightManager lightManager;
        [SerializeField] private AudioClip turnOnSound;
        [SerializeField] private AudioClip turnOffSound;
        private Animator animator;
        private AudioSource audioSource;
        private int switchButtonTrigger;
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
            }
        }

        /// <summary>
        /// Sets isBusy flag to true.
        /// </summary>
        public void SetBusyOn()
        {
            isBusy = true;
        }

        /// <summary>
        /// Sets isBusy flag to false.
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
            if (isEnabled) lightManager.SwitchLights();
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
