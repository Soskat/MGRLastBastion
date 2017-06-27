using System;


namespace LastBastion.Biofeedback
{
    /// <summary>
    /// Class that represents set with three categories.
    /// </summary>
    [Serializable]
    public class TripleTreshold
    {
        #region Public properties
        /// <summary>Maximum value of <see cref="DataState.Low"/> state.</summary>
        public float Low = 0.7f;
        /// <summary>Maximum value of <see cref="DataState.Medium"/> state.</summary>
        public float Medium = 1.3f;
        /// <summary>Maximum value of <see cref="DataState.High"/> state.</summary>
        public float High = 2.0f;
        #endregion


        #region Public methods
        /// <summary>
        /// Assign <see cref="DataState"/> stated to given value.
        /// </summary>
        /// <param name="value">Input value</param>
        /// <returns>Assigned <see cref="DataState"/> state</returns>
        public DataState AssignState(float value)
        {
            if (value <= Low) return DataState.Low;
            else if (value <= Medium) return DataState.Medium;
            else return DataState.High;
        }
        #endregion
    }
}
