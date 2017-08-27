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
        /// <summary>Object's rotation in focus mode.</summary>
        [SerializeField]
        private Quaternion focusedRotation;
        /// <summary>Object's scale in focus mode.</summary>
        [SerializeField]
        private Vector3 focusedScale;
        /// <summary>Object's origin parent.</summary>
        private Transform originParent;
        /// <summary>Object's origin position.</summary>
        private Vector3 originPosition;
        /// <summary>Object's origin rotation.</summary>
        private Quaternion originRotation;
        /// <summary>Object's origin scale.</summary>
        private Vector3 originScale;
        /// <summary>Object's origin layer.</summary>
        private int originLayer;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            SetOriginPositionAndRotation();
            originScale = transform.localScale;
            originParent = transform.parent;
            originLayer = gameObject.layer;
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Saves current position and rotation as origin ones.
        /// </summary>
        public void SetOriginPositionAndRotation()
        {
            originPosition = transform.position;
            originRotation = transform.rotation;
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
            // back to origin settings:
            transform.parent = originParent;
            transform.position = originPosition;
            transform.rotation = originRotation;
            transform.localScale = originScale;
            // change back layer of game object and turn on highlight:
            gameObject.layer = originLayer;
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