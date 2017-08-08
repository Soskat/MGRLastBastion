using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents pickable, focusable objects.
    /// </summary>
    public class FocusableObject : MonoBehaviour
    {
        #region Private fields
        private Transform originalParent;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private int originalLayer;
        #endregion

        
        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            originalParent = transform.parent;
            originalLayer = gameObject.layer;
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Picks object up.
        /// </summary>
        public virtual void PickUp(Transform newTransform)
        {
            // transform object to newTransform and change layer:
            transform.parent = newTransform.parent;
            transform.position = newTransform.position;
            transform.rotation = newTransform.rotation;
            gameObject.layer = GameManager.instance.IgnoreLightLayer;
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
            gameObject.layer = originalLayer;
        }
        #endregion
    }

}
