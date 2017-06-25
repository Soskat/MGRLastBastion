using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game
{
    /// <summary>
    /// Component that manages flashlight behaviour.
    /// </summary>
    public class Flashlight : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool lightOn = false;
        [SerializeField] private GameObject lightRay;
        #endregion


        #region Public fields & properties
        /// <summary>Is light turned on?</summary>
        public bool LightOn { get { return lightOn; } }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(lightRay);
        }

        // Use this for initialization
        void Start()
        {
            lightRay.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Switches the light of the flashlight.
        /// </summary>
        public void SwitchLight()
        {
            lightOn = (lightOn) ? false : true;
            if (lightOn) lightRay.SetActive(true);
            else lightRay.SetActive(false);
        }
        #endregion
    }
}
