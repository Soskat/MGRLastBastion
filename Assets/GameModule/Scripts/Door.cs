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
        [SerializeField] private DoorType doorType;
        [SerializeField] private bool isLocked = false;
        [SerializeField] private bool isClosed = true;
        private bool isBusy = false;
        private Animator animator;
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
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Public methods
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
