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
        //public Goal Init()
        //{
        //    goalsData = LoadGoalsDataFromFile(plotGoalsPath);
        //    if (goalsData == null)
        //    {

        //    }
        //}
        #endregion
    }
}
