using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Player
{
    /// <summary>
    /// Component that manages interaction with interactable game objects.
    /// </summary>
    public class InteractionController : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private int rayLength = 20;
        [SerializeField] private GameObject activeObject;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (activeObject != null && Input.GetKeyDown(KeyCode.E))
            {
                if(activeObject.tag == "Door")
                {
                    activeObject.GetComponentInParent<Door>().Interact();
                }
            }
        }

        // FixedUpdate is called every fixed framerate frame, if the MonoBehaviour is enabled
        private void FixedUpdate()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.blue);

            if (Physics.Raycast(ray, out hit, rayLength, layerMask, QueryTriggerInteraction.Ignore))
            {
                activeObject = hit.collider.gameObject;
            }
            else
            {
                activeObject = null;
            }
        }
        #endregion
    }
}
