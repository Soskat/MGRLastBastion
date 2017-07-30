using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages lights switching logic based on player's biofeedback.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class LightManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private bool isActive = false;
        [SerializeField] private float baseDelay = 10f;
        [SerializeField] private List<LightSource> lights;
        private bool lightsOn = false;
        private bool isBusy = false;
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
            if (isActive && !isBusy)
            {
                //// tests: -------------------------------
                //if (Input.GetKeyDown(KeyCode.Z)) SwitchLights();
                //if (Input.GetKeyDown(KeyCode.C) && lightsOn) ExplodeAllLights();

                isBusy = true;

                // biofeedback logic:
                //if (GameManager.instance.BBModule.IsEnabled)
                {
                    switch (GameManager.instance.PlayerBiofeedback.ArousalCurrentState)
                    {
                        case Biofeedback.DataState.High:
                            int choice = Random.Range(0, 2);
                            if (choice == 0) SwitchLights();
                            else BlinkAllLights();
                            Debug.Log("High");
                            break;

                        case Biofeedback.DataState.Medium:
                            ExplodeRandomLight();
                            Debug.Log("Medium");
                            break;

                        case Biofeedback.DataState.Low:
                            ExplodeAllLights();
                            Debug.Log("Low");
                            break;

                        default:
                            break;
                    }
                    // wait for next move:
                    StartCoroutine(CooldownTimer(GameManager.instance.PlayerBiofeedback.ArousalCurrentModifier * baseDelay));
                    Debug.Log(">> Wait for " + GameManager.instance.PlayerBiofeedback.ArousalCurrentModifier * baseDelay + " seconds");
                }
                //// randomly choose light event:
                //else
                //{
                //    int randomEvent = Random.Range(0, 4);
                //    switch (randomEvent)
                //    {
                //        case 0:
                //            SwitchLights();
                //            break;

                //        case 1:
                //            BlinkAllLights();
                //            break;

                //        case 2:
                //            ExplodeRandomLight();
                //            break;

                //        case 3:
                //            ExplodeAllLights();
                //            break;
                //    }
                //    // wait for next move:
                //    StartCoroutine(CooldownTimer(randomEvent + baseDelay));
                //}
            }
        }

        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            isActive = true;
        }

        // OnTriggerExit is called when the Collider other has stopped touching the trigger
        private void OnTriggerExit(Collider other)
        {
            isActive = false;
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

        /// <summary>
        /// Sets <see cref="isBusy"/> flag to true for specific time.
        /// </summary>
        /// <param name="cooldownTime">Time to wait</param>
        /// <returns></returns>
        private IEnumerator CooldownTimer(float cooldownTime)
        {
            //isBusy = true;
            yield return new WaitForSeconds(cooldownTime);
            isBusy = false;
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
        public void ExplodeAllLights()
        {
            foreach (LightSource light in lights)
            {
                light.ExplodeLight();
            }
        }

        /// <summary>
        /// Makes one random child light to explode.
        /// </summary>
        public void ExplodeRandomLight()
        {
            List<LightSource> temp = lights.Where(x => x.IsBroken == false).ToList();
            if (temp.Count > 0) lights[Random.Range(0, temp.Count)].ExplodeLight();
        }

        /// <summary>
        /// Makes all child lights to blink.
        /// </summary>
        public void BlinkAllLights()
        {
            foreach (LightSource light in lights) light.DoBlink();
        }
        #endregion
    }
}
