namespace LastBastion.Biofeedback
{
    /// <summary>
    /// Class that represents a packet of biofeedback data for game mechanics.
    /// </summary>
    public class BiofeedbackData
    {
        #region Public fields
        /// <summary>Current value of HR modifier.</summary>
        public float HrModifier;
        /// <summary>Current HR state based on <see cref="HrModifier"/>.</summary>
        public DataState HrState;
        /// <summary>Current value of GSR modifier.</summary>
        public float GsrModifier;
        /// <summary>Current GSR state based on <see cref="GsrModifier"/>.</summary>
        public DataState GsrState;
        /// <summary>Current value of arousal modifier.</summary>
        public float ArousalModifier;
        /// <summary>Current arousal state based on <see cref="ArousalModifier"/>.</summary>
        public DataState ArousalState;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of <see cref="BiofeedbackData"/>
        /// </summary>
        /// <param name="hrModifier">Current value of HR modifier</param>
        /// <param name="hrState">Current HR state based on HR modifier</param>
        /// <param name="gsrModifier">Current value of GSR modifier</param>
        /// <param name="gsrState">Current GSR state based on GSR modifier</param>
        public BiofeedbackData(float hrModifier, DataState hrState, float gsrModifier, DataState gsrState, float arousalModifier, DataState arousalState)
        {
            HrModifier = hrModifier;
            HrState = hrState;
            GsrModifier = gsrModifier;
            GsrState = gsrState;
            ArousalModifier = arousalModifier;
            ArousalState = arousalState;
        }
        #endregion
    }
}
