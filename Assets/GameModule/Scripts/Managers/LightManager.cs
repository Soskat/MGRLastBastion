using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages lights switching logic based on player's biofeedback.
    /// </summary>
    public class LightManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private float lightIntensity = 10f;
        [SerializeField] private List<LightSource> lights;
        private bool lightsOn = false;
        #endregion
        

        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            lights.AddRange(GetComponentsInChildren<LightSource>());
            foreach (LightSource light in lights)
            {
                light.MaxLightIntensity = lightIntensity;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // tests: -------------------------------
            if (Input.GetKeyDown(KeyCode.Z)) SwitchLights();

            if (Input.GetKeyDown(KeyCode.X) && lightsOn) ExplodeLights();
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Sets lights ON or OFF based on input parameter.
        /// </summary>
        /// <param name="lightMode">Light mode</param>
        private void SetLightsMode(bool lightMode)
        {
            // turn on the lights:
            if (lightMode)
            {
                foreach (LightSource light in lights)
                {
                    light.TurnOnTheLight();
                }
                // choose randomly which light will blink:
                int index = Random.Range(0, lights.Count);
                while (true)
                {
                    if (!lights[index].IsBroken) break;
                    else index = Random.Range(0, lights.Count);
                }
                lights[index].StartBlinking();
            }
            // turn off the lights:
            else
            {
                foreach (LightSource light in lights)
                {
                    light.TurnOffTheLight();
                }
            }
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Switches the lights ON/OFF state.
        /// </summary>
        public void SwitchLights()
        {
            lightsOn = lightsOn ? false : true;
            SetLightsMode(lightsOn);
        }

        /// <summary>
        /// Makes all child lights to explode.
        /// </summary>
        public void ExplodeLights()
        {
            foreach (LightSource light in lights)
            {
                light.ExplodeLight();
            }
        }
        #endregion
    }
}
