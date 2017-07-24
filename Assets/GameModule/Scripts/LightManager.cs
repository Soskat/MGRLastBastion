using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game
{
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
            if (turnOnLights)
            {
                Debug.Log("Turn ON lights");
                turnOnLights = false;
                // turn on all lights:
                foreach(LightSource light in lights)
                {
                    light.TurnOnTheLight();
                }
                // choose randomly which light will blink:
                lights[Random.Range(0, lights.Count)].StartBlinking();
            }

            if (turnOffLights)
            {
                Debug.Log("Turn OFF lights");
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
