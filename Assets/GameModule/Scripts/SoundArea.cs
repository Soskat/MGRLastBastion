using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game
{
    public class SoundArea : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool isActive = true;
        #endregion


        #region Public fields & properties
        /// <summary>Is this sound area active?</summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            UpdateChildren();
        }        

        // Update is called once per frame
        void Update()
        {
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Switches isActive state of all object children.
        /// </summary>
        public void SwitchChildrenActiveStatus()
        {
            isActive = isActive ? false : true;
            UpdateChildren();
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Updates children isActive state.
        /// </summary>
        private void UpdateChildren()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(isActive);
            }
        }
        #endregion
    }
}
