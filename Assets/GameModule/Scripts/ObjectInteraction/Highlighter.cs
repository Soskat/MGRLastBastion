using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that manages higlighting interactive game object.
    /// </summary>
    public class Highlighter : MonoBehaviour
    {
        #region Private fields
        [SerializeField][Range(0.0f, 2.0f)] private float outlineWidth;
        private bool isMouseOver = false;
        private bool isInRange = false;
        #endregion


        #region MonoBehaviour methods
        // Update is called once per frame
        void Update()
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


        #region Public methods
        /// <summary>
        /// Turns on the highlight of the object.
        /// </summary>
        public void TurnOnHighlight()
        {
            isMouseOver = true;
            GetComponent<Renderer>().material.SetColor("_OutlineColor", GameManager.instance.Assets.HighlightColor);
            GetComponent<Renderer>().material.SetFloat("_Outline", outlineWidth);
        }

        /// <summary>
        /// Turns off the highlight of the object.
        /// </summary>
        public void TurnOffHighlight()
        {
            isMouseOver = false;
            ManageHintColor();
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Sets or removes hint color from game object's material.
        /// </summary>
        private void ManageHintColor()
        {
            if (isInRange)
            {
                GetComponent<Renderer>().material.SetColor("_OutlineColor", GameManager.instance.Assets.HintColor);
                GetComponent<Renderer>().material.SetFloat("_Outline", outlineWidth);
            }
            else
            {
                GetComponent<Renderer>().material.SetFloat("_Outline", 0.0f);
            }
        }
        #endregion
    }
}