using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game
{
    /// <summary>
    /// Component that switches the GameManager.ActiveRoom object based on player's current position.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class RoomActivator : MonoBehaviour
    {
        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player") GameManager.instance.ActiveRoom = gameObject;
        }
        #endregion
    }
}
