using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game
{
    public class Flashlight : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool lightOn = false;
        [SerializeField] private GameObject light;
        #endregion


        #region Public fields & properties
        /// <summary>Is light turned on?</summary>
        public bool LightOn { get { return lightOn; } }
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(light);
        }

        // Use this for initialization
        void Start()
        {
            light.SetActive(false);
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
            if (lightOn) light.SetActive(true);
            else light.SetActive(false);
        }
        #endregion
    }
}
