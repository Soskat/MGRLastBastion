using LastBastion.Game.Managers;
using LastBastion.Game.ObjectInteraction;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


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
        [SerializeField] private Transform focusPoint;
        [SerializeField] private bool isFocused;
        //private Transform oldTransform;
        #endregion


        #region Public fields & properties
        /// <summary>Is player focused on interaction with object?</summary>
        public bool IsFocused { get { return isFocused; } }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            focusPoint = GameObject.FindGameObjectWithTag("FocusPoint").transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (activeObject != null && Input.GetKeyDown(KeyCode.E))
            {
                if (!isFocused)
                {
                    if (activeObject.tag == "Interactive")
                    {
                        // interact with the object:
                        activeObject.GetComponentInParent<IInteractiveObject>().Interact();
                    }
                    else if (activeObject.tag == "Focusable")
                    {
                        // pick up the object to take a look at it:
                        isFocused = true;
                        LevelManager.instance.Player.GetComponent<FirstPersonController>().IsFocused = true;
                        activeObject.GetComponentInParent<FocusableObject>().PickUp(focusPoint);
                    }
                }
                else
                {
                    // put down the object:
                    isFocused = false;
                    LevelManager.instance.Player.GetComponent<FirstPersonController>().IsFocused = false;
                    activeObject.GetComponentInParent<FocusableObject>().PutDown();
                }

            }
        }

        // FixedUpdate is called every fixed framerate frame, if the MonoBehaviour is enabled
        private void FixedUpdate()
        {
            if (isFocused) return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * GameManager.instance.Assets.InteractionRange, Color.blue);

            if (Physics.Raycast(ray, out hit, GameManager.instance.Assets.InteractionRange, layerMask, QueryTriggerInteraction.Ignore))
            {
                // turn off highlight in former activeObject:
                if (activeObject != null && activeObject.GetComponent<Highlighter>() != null) activeObject.GetComponent<Highlighter>().TurnOffHighlight();
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
