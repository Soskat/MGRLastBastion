using LastBastion.Game.Managers;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents collectable rune object.
    /// </summary>
    [RequireComponent(typeof(Highlighter))]
    public class RuneClue : FocusableObject
    {
        #region Private fields
        [SerializeField] private RuneHolder runeSign;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(runeSign);
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Picks object up.
        /// </summary>
        public override void PutDown()
        {
            // inform that new rune was found:
            LevelManager.instance.FoundRune();
            // enable rune holder:
            runeSign.EnableInteractivity();
            // destroy this game object:
            Destroy(gameObject);
        }
        #endregion
    }
}
