using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that manages highlighting interactive game object.
    /// </summary>
    public class HighlighterColoured : Highlighter
    {
        #region Public methods
        /// <summary>
        /// Applies hint color to the object.
        /// </summary>
        public override void SetHintColor()
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", GameManager.instance.Assets.HintColor);
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }

        /// <summary>
        /// Applies highlight color to the object.
        /// </summary>
        public override void SetHighlightColor()
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", GameManager.instance.Assets.HighlightColor);
            GetComponent<Renderer>().material.SetColor("_Color", GameManager.instance.Assets.HighlightColor);
        }

        /// <summary>
        /// Applies normal color to the object.
        /// </summary>
        public override void SetNormalColor()
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
        #endregion
    }
}
