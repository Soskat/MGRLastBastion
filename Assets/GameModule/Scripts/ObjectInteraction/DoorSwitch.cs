﻿using UnityEngine;
using UnityEngine.Assertions;

namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents door switcher behaviour.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class DoorSwitch : MonoBehaviour, IInteractiveObject
    {
        #region Private fields
        [SerializeField] private Door door;
        [SerializeField] private AudioClip pushButtonSound;
        private Animator animator;
        private AudioSource audioSource;
        private int pushedButtonTrigger;
        private bool isBusy = false;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(door);
            Assert.IsNotNull(pushButtonSound);
        }

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            pushedButtonTrigger = Animator.StringToHash("PushedButton");
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

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
        public void SwitchDoorState()
        {
            if (door.IsLocked) door.IsLocked = false;
            else door.IsLocked = true;
        }

        /// <summary>
        /// Plays sound of pushing the button.
        /// </summary>
        public void PlayPushSound()
        {
            audioSource.PlayOneShot(pushButtonSound);
        }
        #endregion
    }
}