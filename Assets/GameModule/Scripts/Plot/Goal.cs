using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Plot
{
    [Serializable]
    public class Goal
    {
        #region Public fields
        /// <summary>The weight of the goal.</summary>
        public int GoalWeight;
        /// <summary>The content of the goal.</summary>
        public string GoalContent;
        #endregion


        //#region Public fields & properties
        ///// <summary>The weight of the goal.</summary>
        //public int GoalWeight { get { return goalWeight; } }
        ///// <summary>The content of the goal.</summary>
        //public string GoalContent { get { return goalContent; } }
        //#endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of <see cref="Goal"/> class.
        /// </summary>
        public Goal()
        {
            GoalWeight = -1;
            GoalContent = "<empty>";
        }

        /// <summary>
        /// Creates an instance of <see cref="Goal"/> class.
        /// </summary>
        /// <param name="weight">Weight of the goal</param>
        /// <param name="content">Content of the goal</param>
        public Goal(int weight, string content)
        {
            GoalWeight = weight;
            GoalContent = content;
        }
        #endregion


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
