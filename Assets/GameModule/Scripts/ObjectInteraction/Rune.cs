using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents collectable rune object.
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    public class Rune : FocusableObject
    {
        #region Public methods
        /// <summary>
        /// Picks object up.
        /// </summary>
        public override void PutDown()
        {
            // inform that new rune was found:
            LevelManager.instance.FoundRune();
            // destroy this game object:
            Destroy(gameObject);
        }
        #endregion
    }
}
