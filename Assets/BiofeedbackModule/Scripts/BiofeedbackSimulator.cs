using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Biofeedback
{
    /// <summary>
    /// Component that generates biofeedback data for simulation purposes.
    /// </summary>
    public class BiofeedbackSimulator : MonoBehaviour
    {
        #region Private fields
        /// <summary>Is simulator enabled?</summary>
        [SerializeField] private bool isEnabled;
        /// <summary>Tresholds for HR data.</summary>
        [SerializeField] private TripleTreshold hrLevel;
        /// <summary>Tresholds for GSR data.</summary>
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
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            CurrentHr = AverageHr;
            CurrentGsr = AverageGsr;
            GameManager.instance.BBModule.BiofeedbackDataChanged += () => UpdateBiofeedbackVariables();
            if (isEnabled) GameManager.instance.BBModule.UpdateAverageBiofeedbackData(AverageHr, AverageGsr);
        }

        // Update is called once per frame
        void Update()
        {
            if (isEnabled) GameManager.instance.BBModule.UpdateCurrentBiofeedbackData(CurrentHr, CurrentGsr);

            // manual test:
            if (Input.GetKey(KeyCode.KeypadPlus)) CurrentHr = (++CurrentHr > 200) ? 200 : CurrentHr;
            if (Input.GetKey(KeyCode.KeypadMinus)) CurrentHr = (--CurrentHr < 0) ? 0 : CurrentHr;
            if (Input.GetKey(KeyCode.Keypad8)) CurrentGsr = (++CurrentGsr > 200) ? 200 : CurrentGsr;
            if (Input.GetKey(KeyCode.Keypad2)) CurrentGsr = (--CurrentGsr < 0) ? 0 : CurrentGsr;
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Updates simulator's variables.
        /// </summary>
        private void UpdateBiofeedbackVariables()
        {
            HrModifier = GameManager.instance.BBModule.HrModifier;
            HrState = GameManager.instance.BBModule.HrState;
            GsrModifier = GameManager.instance.BBModule.GsrModifier;
            GsrState = GameManager.instance.BBModule.GsrState;
        }
        #endregion
    }
}
