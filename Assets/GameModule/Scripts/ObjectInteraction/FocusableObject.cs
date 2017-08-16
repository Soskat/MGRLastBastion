﻿using LastBastion.Game.Managers;
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
        // Use this for initialization
        void Start()
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
            // transform object to newTransform and change layer:
            transform.parent = newTransform.parent;
            transform.position = newTransform.position;
            transform.localRotation = focusedRotation;
            transform.localScale = focusedScale;
            gameObject.layer = GameManager.instance.IgnoreLightLayer;
            // turn off highlight in focuse mode:
            GetComponent<Highlighter>().SetHighlightBlockade();
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
            gameObject.layer = originalLayer;
            // turn on highlight back:
            GetComponent<Highlighter>().ResetHighlightBlockade();
        }
        #endregion
    }

}
