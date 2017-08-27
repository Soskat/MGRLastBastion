using LastBastion.Game.Managers;
using UnityEngine;

namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that manages logic for highlighting objects.
    /// </summary>
    public class Highlighter : MonoBehaviour
    {
        #region Protected fields
        /// <summary>Is mouse over this game object?</summary>
        protected bool isMouseOver = false;
        /// <summary>Is game object in player's range?</summary>
        protected bool isInRange = false;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            SetNormalColor();
        }

        // Update is called once per frame
        public void Update()
        {
            if (!isMouseOver)
            {
                if ((transform.position - LevelManager.instance.Player.transform.position).magnitude <= GameManager.instance.Assets.HintRange)
                {
                    if (!isInRange)
                    {
                        isInRange = true;
                        ManageHintColor();
                    }
                }
                else
                {
                    if (isInRange)
                    {
                        isInRange = false;
                        ManageHintColor();
                    }
                }
            }
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Sets or removes hint color from game object's material.
        /// </summary>
        private void ManageHintColor()
        {
            if (isInRange) SetHintColor();
            else SetNormalColor();
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Applies hint color to the object.
        /// </summary>
        public virtual void SetHintColor() { }

        /// <summary>
        /// Applies highlight color to the object.
        /// </summary>
        public virtual void SetHighlightColor() { }

        /// <summary>
        /// Applies normal color to the object.
        /// </summary>
        public virtual void SetNormalColor() { }
        
        /// <summary>
        /// Turns on the highlight of the selected object.
        /// </summary>
        public void TurnOnHighlight()
        {
            isMouseOver = true;
            SetHighlightColor();
        }

        /// <summary>
        /// Turns off the highlight of the previously object.
        /// </summary>
        public void TurnOffHighlight()
        {
            isMouseOver = false;
            ManageHintColor();
        }

        /// <summary>
        /// Turns off highlight status of the focused object.
        /// </summary>
        public void SetHighlightBlockade()
        {
            SetNormalColor();
        }

        /// <summary>
        /// Reset highlight status of the previously focused object.
        /// </summary>
        public void ResetHighlightBlockade()
        {
            if (isMouseOver) SetHighlightColor();
            else ManageHintColor();
        }
        #endregion
    }
}