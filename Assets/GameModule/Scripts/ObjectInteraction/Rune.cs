using LastBastion.Game.Managers;
using LastBastion.Game.Plot;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents collectable rune object.
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    public class Rune : FocusableObject
    {
        #region Private fields
        //[SerializeField] private Runes runeType;
        [SerializeField] private RuneHolder rune;
        #endregion


        //#region Public fields & properties
        ///// <summary>Type of the rune.</summary>
        //public Runes RuneType { get { return runeType; } }
        //#endregion


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
