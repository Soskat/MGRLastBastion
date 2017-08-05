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
        public override void PutDown()
        {
            // informa that new rune was found:
            GameManager.instance.LevelManager.FoundRune();
            // destroy this game object:
            Destroy(gameObject);
        }
        #endregion
    }
}
