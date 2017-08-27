//using UnityEngine;
//using UnityEngine.Assertions;
//using UnityEngine.UI;


//namespace LastBastion.Game.UIControllers
//{
//    /// <summary>
//    /// Component that manages SensorPanel UI logic.
//    /// </summary>
//    public class SensorPanelController : MonoBehaviour
//    {

//        #region Private fields
//        /// <summary>Paired MS Band label <see cref="Text"/> component.</summary>
//        [SerializeField] private Text pairedBandLabel;
//        /// <summary>HR reading value label <see cref="Text"/> component.</summary>
//        [SerializeField] private Text hrReadingLabel;
//        /// <summary>Average HR reading value label <see cref="Text"/> component.</summary>
//        [SerializeField] private Text averageHrLabel;
//        /// <summary>GSR reading value label <see cref="Text"/> component.</summary>
//        [SerializeField] private Text gsrReadingLabel;
//        /// <summary>Average GSR reading value label <see cref="Text"/> component.</summary>
//        [SerializeField] private Text averageGsrLabel;
//        #endregion


//        #region MonoBehaviour methods
//        // Awake is called when the script instance is being loaded
//        private void Awake()
//        {
//            Assert.IsNotNull(pairedBandLabel);
//            Assert.IsNotNull(hrReadingLabel);
//            Assert.IsNotNull(averageHrLabel);
//            Assert.IsNotNull(gsrReadingLabel);
//            Assert.IsNotNull(averageGsrLabel);
//        }

//        // Use this for initialization
//        private void Start()
//        {
//            ResetLabels();
//        }
//        #endregion


//        #region Public methods
//        /// <summary>
//        /// Resets labels values.
//        /// </summary>
//        public void ResetLabels()
//        {
//            pairedBandLabel.text = "-";
//            hrReadingLabel.text = "-";
//            averageHrLabel.text = "-";
//            gsrReadingLabel.text = "-";
//            averageGsrLabel.text = "-";
//        }

//        /// <summary>
//        /// Updates paired Band label with new text.
//        /// </summary>
//        /// <param name="newLabel">New label text</param>
//        public void UpdateBandLabel(string newLabel)
//        {
//            pairedBandLabel.text = newLabel;
//        }

//        /// <summary>
//        /// Updates values of average readings labels.
//        /// </summary>
//        /// <param name="hr">New average HR value</param>
//        /// <param name="gsr">New average GSR value</param>
//        public void UpdateAverageReadings(int hr, int gsr)
//        {
//            averageHrLabel.text = hr.ToString();
//            averageGsrLabel.text = gsr.ToString();
//        }

//        /// <summary>
//        /// Updates values of current readings labels.
//        /// </summary>
//        /// <param name="hr">New current HR value</param>
//        /// <param name="gsr">New current GSR value</param>
//        public void UpdateCurrentReadings(int hr, int gsr)
//        {
//            hrReadingLabel.text = hr.ToString();
//            gsrReadingLabel.text = gsr.ToString();
//        }
//        #endregion
//    }
//}