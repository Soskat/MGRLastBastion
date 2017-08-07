using LastBastion.Game.Plot;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    public class PlotManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private List<PlotGoal> plotGoals;
        [SerializeField] private string plotGoalsPath;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            plotGoalsPath = Application.dataPath + "/GameModule/MainPlot/plot_goals.txt";
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Public methods
        public PlotGoal Init()
        {
            FileStream goalsSource = File.Open(plotGoalsPath, FileMode.OpenOrCreate);

            return new PlotGoal();
        }
        #endregion


        #region Private test methods
        private void CreateTestData()
        {
            List<PlotGoal> goals = new List<PlotGoal>();
            goals.Add(new PlotGoal());
        }
        #endregion
    }
}
