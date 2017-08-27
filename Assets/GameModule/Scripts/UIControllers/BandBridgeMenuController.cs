using LastBastion.Game.Managers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace LastBastion.Game.UIControllers
{
    /// <summary>
    /// Component that manages BandBridge UI menu logic.
    /// </summary>
    public class BandBridgeMenuController : MonoBehaviour
    {

        #region Private fields
        /// <summary>Game object of list view for connected MS Band devices.</summary>
        [SerializeField] private GameObject listView;
        /// <summary>Paired MS Band name label.</summary>
        [SerializeField] private Text pairedBandMenuLabel;
        /// <summary>Host name <see cref="InputField"/>.</summary>
        [SerializeField] private InputField hostNameInput;
        /// <summary>Service port <see cref="InputField"/>.</summary>
        [SerializeField] private InputField servicePortInput;
        /// <summary><see cref="ListController"/> component of <see cref="listView"/> object.</summary>
        private ListController listController;
        #endregion


        #region Public properties
        /// <summary>Controller of UI list.</summary>
        public ListController ListController { get { return listController; } }
        /// <summary>Paired Band label.</summary>
        public string PairedBand
        {
            get { return pairedBandMenuLabel.text; }
            set { pairedBandMenuLabel.text = value; }
        }
        /// <summary>Host name input field.</summary>
        public string HostName
        {
            get { return hostNameInput.text; }
            set { hostNameInput.text = value; }
        }
        /// <summary>Service port input field.</summary>
        public string ServicePort
        {
            get { return servicePortInput.text; }
            set { servicePortInput.text = value; }
        }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(listView);
            Assert.IsNotNull(pairedBandMenuLabel);
            Assert.IsNotNull(hostNameInput);
            Assert.IsNotNull(servicePortInput);
        }

        // Use this for initialization
        void Start()
        {
            listController = listView.GetComponent<ListController>();
            HostName = GameManager.instance.BBModule.RemoteHostName;
            ServicePort = GameManager.instance.BBModule.RemoteServicePort.ToString();
            PairedBand = GameManager.instance.BBModule.PairedBand.ToString();
        }
        #endregion
    }
}