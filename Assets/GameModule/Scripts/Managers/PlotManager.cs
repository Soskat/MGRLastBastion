using LastBastion.Game.Plot;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    public class PlotManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private List<PlotGoal> plotGoals;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Public methods
        /// <summary>
        /// Initializes PlotManager system.
        /// </summary>
        /// <returns>Current plot goal</returns>
        public Goal Init()
        {
            if (GameManager.instance.Assets.Goals.Count == 0) return null;
            // update all clue objects from scene with plot goals info:
            for(int i = 1, j = 0; i < GameManager.instance.Assets.Goals.Count; i++, j++)
            {
                if (j >= plotGoals.Count) break;
                plotGoals[j].UpdateGoal(GameManager.instance.Assets.Goals[i]);
            }
            // return current plot goal:
            return GameManager.instance.Assets.Goals[0];
        }
        #endregion
    }
}
