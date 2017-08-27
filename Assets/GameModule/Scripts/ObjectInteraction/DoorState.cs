using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents door state.
    /// </summary>
    public class DoorState : MonoBehaviour
    {
        #region Private fields
        /// <summary>Has door been opened?</summary>
        [SerializeField] private bool wasOpened;
        #endregion


        #region Public fields & properties
        /// <summary>Has door been opened?</summary>
        public bool WasOpened { get { return wasOpened; } }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            wasOpened = false;
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Sets wasOpened to true.
        /// </summary>
        public void OpenDoor()
        {
            wasOpened = true;
        }
        #endregion
    }
}