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
        [SerializeField] private bool lightsOn = false;
        [SerializeField] private bool lightsBroken = false;
        [SerializeField] private bool isBusy = false;
        [SerializeField] private bool isInRange = false;
        [SerializeField] private float range = 20f;
        [SerializeField] private float baseDelay = 10f;
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
            if (isInRange)
            {
                if ((transform.position - GameManager.instance.Player.transform.position).magnitude > range)
                {
                    isInRange = false;
                    if (lightsOn) SwitchLights();
                }
            }

            if (isActive && !lightsBroken && !isBusy)
            {
                //// tests: -------------------------------
                //if (Input.GetKeyDown(KeyCode.Z)) SwitchLights();
                //if (Input.GetKeyDown(KeyCode.C) && lightsOn) ExplodeAllLights();

                isBusy = true;

                // biofeedback logic:
                if (GameManager.instance.BBModule.IsEnabled)
                {
                    switch (GameManager.instance.PlayerBiofeedback.ArousalCurrentState)
                    {
                        case Biofeedback.DataState.High:
                            int choice = Random.Range(0, 3);
                            switch (choice)
                            {
                                case 0:
                                    SwitchLights();
                                    break;

                                case 1:
                                    BlinkRandomLight();
                                    break;

                                case 2:
                                    BlinkAllLights();
                                    break;
                            }
                            break;

                        case Biofeedback.DataState.Medium:
                            ExplodeRandomLight();
                            break;

                        case Biofeedback.DataState.Low:
                            ExplodeAllLights();
                            break;

                        default:
                            break;
                    }
                    // wait for next move:
                    float timeModifier = (GameManager.instance.PlayerBiofeedback.ArousalCurrentModifier > 0f) ? GameManager.instance.PlayerBiofeedback.ArousalCurrentModifier : 0.01f;
                    StartCoroutine(CooldownTimer(timeModifier * baseDelay));
                }
                // randomly choose light event:
                else
                {
                    int randomEvent = Random.Range(0, 5);
                    if (!lightsOn) SwitchLights();
                    else
                    {
                        switch (randomEvent)
                        {
                            case 0:
                                SwitchLights();
                                break;

                            case 1:
                                BlinkAllLights();
                                break;

                            case 2:
                                BlinkAllLights();
                                break;

                            case 3:
                                ExplodeRandomLight();
                                break;

                            case 4:
                                ExplodeAllLights();
                                break;
                        }
                    }
                    // wait for next move:
                    StartCoroutine(CooldownTimer(randomEvent + baseDelay));
                }
            }


            // debug: -----------------------------------------------------------------------------------------------------
            if (isActive) Debug.DrawLine(GameManager.instance.Player.transform.position, transform.position, Color.magenta);
        }

        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            // activate light area only when there's at least one non-broken light:
            if (other.gameObject.tag == "Player" && !lightsBroken)
            {
                isActive = true;
                isInRange = true;
                StartCoroutine(CooldownTimer(Random.Range(5f, 7f)));
            }
        }

        // OnTriggerExit is called when the Collider other has stopped touching the trigger
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player") isActive = false;
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
                foreach (LightSource light in lights) light.TurnOnTheLight();
            }
            // turn off the lights:
            else
            {
                foreach (LightSource light in lights) light.TurnOffTheLight();
            }
        }

        /// <summary>
        /// Sets <see cref="isBusy"/> flag to true for specific time.
        /// </summary>
        /// <param name="cooldownTime">Time to wait</param>
        /// <returns></returns>
        private IEnumerator CooldownTimer(float cooldownTime)
        {
            isBusy = true;
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
            if (!lightsBroken)
            {
                foreach (LightSource light in lights) light.ExplodeLight();
                lightsBroken = true;
            }
        }

        /// <summary>
        /// Makes one random child light to explode.
        /// </summary>
        public void ExplodeRandomLight()
        {
            List<LightSource> temp = lights.Where(x => x.IsBroken == false).ToList();
            if (temp.Count > 0) lights[Random.Range(0, temp.Count)].ExplodeLight();
            else lightsBroken = true;   // all lights are already broken
        }

        /// <summary>
        /// Makes all child lights to blink.
        /// </summary>
        public void BlinkAllLights()
        {
            foreach (LightSource light in lights) light.DoBlink();
        }

        /// <summary>
        /// Makes one random child light to blink.
        /// </summary>
        public void BlinkRandomLight()
        {
            if (lightsBroken) return;
            List<LightSource> temp = lights.Where(x => x.IsBroken == false).ToList();
            // choose randomly which light will blink:
            int index = Random.Range(0, temp.Count);
            temp[index].DoBlink();
        }
        #endregion
    }
}
