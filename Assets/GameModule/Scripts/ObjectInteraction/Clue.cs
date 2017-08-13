using LastBastion.Game.Managers;
using LastBastion.Game.Plot;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents plot clue.
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
            LevelManager.instance.UpdatePlotGoal(GetComponent<PlotGoal>().Goal);
        }
        #endregion
    }
}
