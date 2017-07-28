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
        [SerializeField] private bool turnOnLights = false;
        [SerializeField] private bool turnOffLights = false;
        [SerializeField] private List<LightSource> lights;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            lights.AddRange(GetComponentsInChildren<LightSource>());
        }

        // Update is called once per frame
        void Update()
        {
            // tests: -------------------------------
            if (Input.GetKeyDown(KeyCode.Z) || turnOnLights)
            {
                turnOnLights = false;
                // turn on all lights:
                foreach(LightSource light in lights)
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

            if (Input.GetKeyDown(KeyCode.X) || turnOffLights)
            {
                turnOffLights = false;
                // turn off all lights:
                foreach (LightSource light in lights)
                {
                    light.TurnOffTheLight();
                }
            }
        }
        #endregion
    }
}
