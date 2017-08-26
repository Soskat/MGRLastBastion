using UnityEngine;


namespace LastBastion.Game.SurveySystem
{
    /// <summary>
    /// Components that informs if assigned to game object Dropdown component has recorded change of selected value.
    /// </summary>
    public class AnswerRecord : MonoBehaviour
    {
        #region Public fields
        /// <summary>Has question been answered?</summary>
        public bool WasAnswered;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            WasAnswered = false;
        }
        #endregion
    }
}
