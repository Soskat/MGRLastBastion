using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents pickable, focusable objects.
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    public class FocusableObject : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Quaternion focusedRotation;
        [SerializeField] private Vector3 focusedScale;
        private Transform originalParent;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Vector3 originalScale;
        private int originalLayer;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            SetOriginalPositionAndRotation();
            originalScale = transform.localScale;
            originalParent = transform.parent;
            originalLayer = gameObject.layer;
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Saves current position and rotation as original ones.
        /// </summary>
        public void SetOriginalPositionAndRotation()
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
        }

        /// <summary>
        /// Picks object up.
        /// </summary>
        public virtual void PickUp(Transform newTransform)
        {
            // transform object to newTransform:
            transform.parent = newTransform.parent;
            transform.position = newTransform.position;
            transform.localRotation = focusedRotation;
            transform.localScale = focusedScale;
            // change layer of game object and turn off highlight in focus mode:
            gameObject.layer = GameManager.instance.IgnoreLightLayer;
            GetComponent<Highlighter>().SetHighlightBlockade();
            // do the same to game object's children:
            foreach (Transform child in transform)
            {
                child.gameObject.layer = GameManager.instance.IgnoreLightLayer;
                if (child.gameObject.GetComponent<Highlighter>() != null) child.gameObject.GetComponent<Highlighter>().SetHighlightBlockade();
            }
        }
        
        /// <summary>
        /// Puts object down.
        /// </summary>
        public virtual void PutDown()
        {
            // back to original settings:
            transform.parent = originalParent;
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            transform.localScale = originalScale;
            // change back layer of game object and turn on highlight:
            gameObject.layer = originalLayer;
            GetComponent<Highlighter>().ResetHighlightBlockade();
            // do the same to game object's children:
            foreach (Transform child in transform)
            {
                child.gameObject.layer = gameObject.layer;
                if (child.gameObject.GetComponent<Highlighter>() != null) child.gameObject.GetComponent<Highlighter>().ResetHighlightBlockade();
            }
        }
        #endregion
    }

}
