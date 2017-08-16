using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that manages highlighting interactive game object.
    /// </summary>
    public class HighlighterOutlined : Highlighter
    {
        #region Private fields
        [SerializeField][Range(0.0f, 5.0f)] private float outlineWidth;
        #endregion
        

        #region Public methods
        /// <summary>
        /// Applies hint color to the object.
        /// </summary>
        public override void SetHintColor()
        {
            GetComponent<Renderer>().material.SetColor("_OutlineColor", GameManager.instance.Assets.HintColor);
            GetComponent<Renderer>().material.SetFloat("_Outline", outlineWidth);
        }

        /// <summary>
        /// Applies highlight color to the object.
        /// </summary>
        public override void SetHighlightColor()
        {
            GetComponent<Renderer>().material.SetColor("_OutlineColor", GameManager.instance.Assets.HighlightColor);
            GetComponent<Renderer>().material.SetFloat("_Outline", outlineWidth);
        }

        /// <summary>
        /// Applies normal color to the object.
        /// </summary>
        public override void SetNormalColor()
        {
            GetComponent<Renderer>().material.SetFloat("_Outline", 0f);
        }
        #endregion
    }
}