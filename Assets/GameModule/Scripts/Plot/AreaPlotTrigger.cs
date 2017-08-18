using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.Plot
{
    /// <summary>
    /// Component that represents the area that trigger progress in main plot.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(PlotGoal))]
    public class AreaPlotTrigger : MonoBehaviour
    {
        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        // OnTriggerEnter is called when the Collider other enters the trigger.
        private void OnTriggerEnter(Collider other)
        {
            // inform that new clue was found:
            LevelManager.instance.UpdatePlotGoal(GetComponent<PlotGoal>().Goal);
        }
        #endregion
    }
}
