using LastBastion.Game.Plot;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages the progress of game main plot.
    /// </summary>
    public class PlotManager : MonoBehaviour
    {
        #region Private fields
        /// <summary>List of clue game objects.</summary>
        [SerializeField] private List<PlotGoal> clueObjects;
        /// <summary>Plot goal that triggers orb rune holders.</summary>
        private Goal orbGoal;
        /// <summary>Final plot goal.</summary>
        private Goal lastGoal;
        #endregion


        #region Public fields & properties
        /// <summary>Plot goal that triggers orb rune holders.</summary>
        public Goal OrbGoal { get { return orbGoal; } }
        /// <summary>Final plot goal.</summary>
        public Goal LastGoal { get { return lastGoal; } }
        #endregion


        #region Public methods
        /// <summary>
        /// Initializes PlotManager system.
        /// </summary>
        /// <returns>First plot goal</returns>
        public Goal Init()
        {
            List<Goal> goals = GameManager.instance.Assets.LoadPlotGoals();
            if (goals.Count == 0) return null;
            // update all clue objects from scene with plot goals info:
            for(int i = 1, j = 0; i < goals.Count; i++, j++)
            {
                if (j >= clueObjects.Count) break;
                if (clueObjects[j] == null) break;
                clueObjects[j].UpdateGoal(goals[i]);
            }
            orbGoal = clueObjects[clueObjects.Count - 1].Goal;
            lastGoal = goals[goals.Count - 1];
            // return current plot goal:
            return goals[0];
        }
        #endregion
    }
}
