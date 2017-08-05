using LastBastion.Game.Managers;
using LastBastion.Game.ObjectInteraction;
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
                if(activeObject.tag == "Interactive")
                {
                    activeObject.GetComponentInParent<IInteractiveObject>().Interact();
                }
            }
        }

        // FixedUpdate is called every fixed framerate frame, if the MonoBehaviour is enabled
        private void FixedUpdate()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * GameManager.instance.Assets.InteractionRange, Color.blue);

            if (Physics.Raycast(ray, out hit, GameManager.instance.Assets.InteractionRange, layerMask, QueryTriggerInteraction.Ignore))
            {
                // turn off highlight in former activeObject:
                if (activeObject != null) activeObject.GetComponent<Highlighter>().TurnOffHighlight();
                // manage highlight in new activeObject:
                activeObject = hit.collider.gameObject;
                activeObject.GetComponent<Highlighter>().TurnOnHighlight();
            }
            else if (activeObject != null)
            {
                activeObject.GetComponent<Highlighter>().TurnOffHighlight();
                activeObject = null;
            }
        }
        #endregion
    }
}
