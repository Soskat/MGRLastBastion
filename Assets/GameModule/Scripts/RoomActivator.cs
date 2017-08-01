using LastBastion.Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastBastion.Game
{
    /// <summary>
    /// Component that switches the GameManager.ActiveRoom object based on player's current position.
    /// </summary>
    public class RoomActivator : MonoBehaviour
    {
        #region Private fileds
        [SerializeField] private bool isActive;
        #endregion


        #region MonoBehaviour methods
        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                isActive = true;
                GameManager.instance.ActiveRoom = gameObject;
            }
        }

        // OnTriggerExit is called when the Collider other has stopped touching the trigger
        private void OnTriggerExit(Collider other)
        {
            isActive = false;
        }
        #endregion
    }
}
