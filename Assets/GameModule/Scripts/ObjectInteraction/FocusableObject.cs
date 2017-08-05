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
        private Vector3 originPosition;
        private Quaternion originRotation;
        #endregion

        
        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            originPosition = transform.position;
            originRotation = transform.rotation;
            originalParent = transform.parent;
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Picks object up.
        /// </summary>
        public virtual void PickUp(Transform newTransform)
        {
            transform.parent = newTransform.parent;
            transform.position = newTransform.position;
            transform.rotation = newTransform.rotation;
        }

        /// <summary>
        /// Puts object down.
        /// </summary>
        public virtual void PutDown()
        {
            transform.parent = originalParent;
            transform.position = originPosition;
            transform.rotation = originRotation;
        }
        #endregion
    }

}
