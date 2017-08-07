using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Plot
{
    /// <summary>
    /// Component taht represents single plot goal.
    /// </summary>
    public class PlotGoal : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Goal goal;
        #endregion


        #region Public fields & properties
        /// <summary>The plot goal.</summary>
        public Goal Goal { get { return goal; } }
        #endregion


        //#region MonoBehaviour methods
        //// Use this for initialization
        //void Start()
        //{

        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
        //#endregion

        //#region Public methods
        ///// <summary>
        ///// Updates the goal properties.
        ///// </summary>
        ///// <param name="weight">Weight of the goal</param>
        ///// <param name="content">Content of the goal</param>
        //public void UpdateGoal(int weight, string content)
        //{
        //    goalWeight = weight;
        //    goalContent = content;
        //}
        //#endregion
    }
}
