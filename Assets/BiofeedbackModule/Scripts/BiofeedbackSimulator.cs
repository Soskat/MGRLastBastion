using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Biofeedback
{
    /// <summary>
    /// Component that simulates biofeedback data.
    /// </summary>
    public class BiofeedbackSimulator : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool isEnabled;
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
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            CurrentHr = AverageHr;
            CurrentGsr = AverageGsr;
            GameManager.instance.BBModule.BiofeedbackDataChanged += () => UpdateBiofeedbackVariables();
        }

        // Update is called once per frame
        void Update()
        {
            if (isEnabled) GameManager.instance.BBModule.UpdateBiofeedbackData(AverageHr, CurrentHr, AverageGsr, CurrentGsr);

            // just for test:
            if (Input.GetKey(KeyCode.KeypadPlus)) CurrentHr += 1;
            if (Input.GetKey(KeyCode.KeypadMinus)) CurrentHr -= 1;
            if (Input.GetKey(KeyCode.Keypad8)) CurrentGsr += 1;
            if (Input.GetKey(KeyCode.Keypad2)) CurrentGsr -= 1;
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
