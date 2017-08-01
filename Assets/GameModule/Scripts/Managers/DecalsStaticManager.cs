using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that represents static decals that exist on scene.
    /// </summary>
    public class DecalsStaticManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private GameObject decalsLight;
        [SerializeField] private GameObject decalsHard;
        [SerializeField] private bool wasActivated = false;
        #endregion


        #region Public fields & properties
        /// <summary>Has the decals manager been activated?</summary>
        public bool WasActivated
        {
            get { return wasActivated; }
            set { wasActivated = value; }
        }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            if (decalsLight == null) Assert.IsNotNull(decalsHard);
            if (decalsHard == null) Assert.IsNotNull(decalsLight);
        }

        // Use this for initialization
        void Start()
        {
            if (decalsLight != null) decalsLight.SetActive(false);
            if (decalsHard != null) decalsHard.SetActive(false);
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Activates decals set based on current player's biofeedback state.
        /// </summary>
        public void ActivateDecals()
        {
            if (WasActivated) return;
            // activate decals set based on player's biofeedback:
            switch (GameManager.instance.PlayerBiofeedback.ArousalCurrentState)
            {
                case Biofeedback.DataState.High:
                    if (decalsHard != null) decalsHard.SetActive(true);
                    else if (decalsLight != null) decalsLight.SetActive(true);
                    wasActivated = true;
                    break;

                case Biofeedback.DataState.Medium:
                case Biofeedback.DataState.Low:
                    if (decalsLight != null) decalsLight.SetActive(true);
                    else if (decalsHard != null) decalsHard.SetActive(true);
                    wasActivated = true;
                    break;

                default: break;
            }
        }
        #endregion
    }
}
