using LastBastion.Analytics;
using LastBastion.Game.ObjectInteraction;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages logic of switching lights in area based on player's biofeedback.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class LightManager : MonoBehaviour
    {
        #region Private fields
        /// <summary>Is light manager active?</summary>
        [SerializeField] private bool isActive = false;
        /// <summary>Is light switched on?</summary>
        [SerializeField] private bool lightsOn = false;
        /// <summary>Are lights broken?</summary>
        [SerializeField] private bool lightsBroken = false;
        /// <summary>Is light manager busy?</summary>
        [SerializeField] private bool isBusy = false;
        /// <summary>Base delay for cooldown.</summary>
        [SerializeField] private float baseDelay = 10f;
        /// <summary>List of light sources in area.</summary>
        [SerializeField] private List<LightSource> lights;
        /// <summary>Has something happened during current frame?</summary>
        private bool somethingHappened;
        #endregion


        #region Public fields & properties
        /// <summary>Are the lights turned on?</summary>
        public bool LightsOn { get { return lightsOn; } }
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            lights.AddRange(GetComponentsInChildren<LightSource>());
            somethingHappened = false;
        }

        // Update is called once per frame
        void Update()
        {
            // if level outro is playing, skip all calculations:
            if (LevelManager.instance.IsOutroOn)
            {
                // shut down all lights:
                if (lightsOn) ExplodeAllLights();
                return;
            }

            if (isActive && !lightsBroken && !isBusy)
            {
                // debug mode:
                if (GameManager.instance.DebugMode)
                {
                    if (Input.GetKeyDown(KeyCode.L)) SwitchLights();
                    if (Input.GetKeyDown(KeyCode.K) && lightsOn) ExplodeAllLights();
                }

                isBusy = true;

                // biofeedback on:
                if (GameManager.instance.BiofeedbackMode == BiofeedbackMode.BiofeedbackON && GameManager.instance.BBModule.IsEnabled)
                {
                    somethingHappened = true;
                    switch (GameManager.instance.BBModule.ArousalState)
                    {
                        case Biofeedback.DataState.High:
                            if (!lightsOn) SwitchLights();
                            else
                            {
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
                            }
                            break;

                        case Biofeedback.DataState.Medium:
                            ExplodeRandomLight();
                            break;

                        case Biofeedback.DataState.Low:
                            ExplodeAllLights();
                            break;

                        default:
                            somethingHappened = false;
                            break;
                    }
                    // wait for next move:
                    float timeModifier = (GameManager.instance.BBModule.ArousalModifier > 0f) ? GameManager.instance.BBModule.ArousalModifier : 0.01f;
                    StartCoroutine(CooldownTimer(timeModifier * baseDelay));
                }
                // randomly choose light event:
                else
                {
                    somethingHappened = true;
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

                // save info about event:
                if (GameManager.instance.AnalyticsEnabled && somethingHappened) LevelManager.instance.AddGameEvent(Analytics.EventType.Light);
            }


            // debug mode:
            if (GameManager.instance.DebugMode && isActive)
            {
                Debug.DrawLine(LevelManager.instance.Player.transform.position, transform.position, Color.yellow);
            }
        }

        // OnTriggerEnter is called when the Collider other enters the trigger
        private void OnTriggerEnter(Collider other)
        {
            // activate light area only when there's at least one non-broken light:
            if (other.gameObject.tag == "Player" && !lightsBroken)
            {
                isActive = true;
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
        /// Sets <see cref="isBusy"/> flag to true for specific period of time.
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
        /// Switches all child lights' ON/OFF state.
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
                lightsOn = false;
            }
        }

        /// <summary>
        /// Makes one random child light to explode.
        /// </summary>
        public void ExplodeRandomLight()
        {
            List<LightSource> temp = lights.Where(x => x.IsBroken == false).ToList();
            if (temp.Count > 0) lights[Random.Range(0, temp.Count)].ExplodeLight();
            else
            {
                // all lights are already broken
                lightsBroken = true;
                lightsOn = false;
            }
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
