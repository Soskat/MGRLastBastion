using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that switches the GameManager.ActiveRoom object based on player's current position.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class RoomActivator : MonoBehaviour
    {
        #region Private fields
        /// <summary>Inform the <see cref="GameManager"/> that room has been activated?</summary>
        [SerializeField] private bool informGameManager = true;
        /// <summary>Has room been activated?</summary>
        private bool wasActivated;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            GetComponent<BoxCollider>().isTrigger = true;
            wasActivated = false;
        }

        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                // set this room as active:
                if (informGameManager) GameManager.instance.ActiveRoom = gameObject;

                // if this room is activated for the first time, update achievement counter:
                if (!wasActivated)
                {
                    LevelManager.instance.SearchedRoom();
                    wasActivated = true;
                }
            }
        }
        #endregion
    }
}
