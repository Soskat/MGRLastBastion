using LastBastion.Game.Plot;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents a main plot clue.
    /// </summary>
    [RequireComponent(typeof(PlotGoal))]
    public class Clue : FocusableObject
    {
        #region Public methods
        /// <summary>
        /// Picks object up.
        /// </summary>
        public override void PickUp(Transform newTransform)
        {
            base.PickUp(newTransform);
            // inform that new clue was found:
            GetComponent<PlotGoal>().Activate();
        }
        #endregion
    }
}
