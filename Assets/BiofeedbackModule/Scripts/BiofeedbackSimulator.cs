using LastBastion.Game.Managers;
using System;
using UnityEngine;


namespace LastBastion.Biofeedback
{
    /// <summary>
    /// Component that simulates biofeedback data.
    /// </summary>
    public class BiofeedbackSimulator : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private TripleTreshold hrLevel;
        [SerializeField] private TripleTreshold gsrLevel;
        #endregion

        #region Public fields & properties
        /// <summary>Average HR value.</summary>
        [Range(50, 150)] public int AverageHr = 70;
        /// <summary>Current HR value.</summary>
        [Range(50, 150)] public int CurrentHr;
        /// <summary>Current value of HR modifier.</summary>
        public float HrModifier;
        /// <summary>Current HR state based on HR modifier.</summary>
        public DataState HrState;
        /// <summary>Average GSR value.</summary>
        [Range(10, 50)] public int AverageGsr = 20;
        /// <summary>Current GSR value.</summary>
        [Range(10, 50)] public int CurrentGsr;
        /// <summary>Current value of GSR modifier.</summary>
        public float GsrModifier;
        /// <summary>Current GSR state based on GSR modifier.</summary>
        public DataState GsrState;
        /// <summary>Informs that biofeedback data has changed.</summary>
        public Action<BiofeedbackData> BiofeedbackDataChanged;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            CurrentHr = AverageHr;
            CurrentGsr = AverageGsr;
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameManager.instance.BBModule.IsEnabled) NotifyAboutUpdate();
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Notifies about changes in biofeedback data.
        /// </summary>
        private void NotifyAboutUpdate()
        {
            // update HR data:
            HrModifier = CurrentHr / AverageHr;
            HrState = hrLevel.AssignState(HrModifier);
            // update GSR data:
            GsrModifier = CurrentGsr / AverageGsr;
            GsrState = gsrLevel.AssignState(GsrModifier);
            // inform that biofeedback data has changed:
            BiofeedbackDataChanged(new BiofeedbackData(HrModifier, GsrModifier, HrState, GsrState));
        }
        #endregion
    }
}
