using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game
{
    /// <summary>
    /// Component that represents door behaviour.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour
    {
        #region Provate fields
        [SerializeField] private bool isLocked = false;
        [SerializeField] private bool isClosed = true;
        private bool isBusy = false;
        private Animator animator;
        private int openDoorTrigger;
        private int closeDoorTrigger;
        private int tryDoorTrigger;
        #endregion


        #region Public fields & properties
        /// <summary>Is door busy?</summary>
        public bool IsBusy { get { return isBusy; } }
        /// <summary>Is door locked?</summary>
        public bool IsLocked { get { return isLocked; } }
        /// <summary>Is door locked?</summary>
        public bool IsClosed { get { return isClosed; } }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            openDoorTrigger = Animator.StringToHash("OpenTheDoor");
            closeDoorTrigger = Animator.StringToHash("CloseTheDoor");
            tryDoorTrigger = Animator.StringToHash("TryTheDoor");
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Private methods
        /// <summary>
        /// Opens the door.
        /// </summary>
        private void OpenDoor()
        {
            animator.SetTrigger(openDoorTrigger);
            isClosed = false;
        }

        /// <summary>
        /// Closes the door.
        /// </summary>
        private void CloseDoor()
        {
            animator.SetTrigger(closeDoorTrigger);
            isClosed = true;
        }

        /// <summary>
        /// Tries the lock in the door.
        /// </summary>
        private void LockedDoor()
        {
            animator.SetTrigger(tryDoorTrigger);
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Interacts with the door.
        /// </summary>
        public void Interact()
        {
            if (isLocked) LockedDoor();
            else
            {
                if (isClosed) OpenDoor();
                else CloseDoor();
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
        #endregion
    }
}
