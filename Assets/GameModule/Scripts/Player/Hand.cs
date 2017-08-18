using LastBastion.Biofeedback;
using LastBastion.Game.Managers;
using System.Collections;
using UnityEngine;


namespace LastBastion.Game.Player
{
    /// <summary>
    /// Component that represents Player's hand.
    /// </summary>
    public class Hand : MonoBehaviour
    {
        #region Private fields
        private DataState lastState;
        private bool shakeOn;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        protected void Start()
        {
            lastState = DataState.None;
            shakeOn = false;
            // biofeedback off:
            if (!GameManager.instance.BBModule.IsEnabled) StartCoroutine(Shake());
        }

        // Update is called once per frame
        protected void Update()
        {
            // biofeedback on:
            if (GameManager.instance.BBModule.IsEnabled)
            {
                if (GameManager.instance.BBModule.ArousalState == DataState.High)
                {
                    shakeOn = true;
                    if (lastState != DataState.High)
                    {
                        // save info about start of the shaking event:
                        if (GameManager.instance.AnalyticsEnabled) LevelManager.instance.AddGameEvent(Analytics.EventType.Shaking);
                    }
                }
                else shakeOn = false;
                // update last state variable:
                lastState = GameManager.instance.BBModule.ArousalState;
            }

            // simulate hand shaking:
            if (shakeOn)
            {
                float x = Random.Range(0.0f, GameManager.instance.BBModule.ArousalModifier);
                float y = Random.Range(0.0f, GameManager.instance.BBModule.ArousalModifier);
                float z = Random.Range(0.0f, GameManager.instance.BBModule.ArousalModifier);
                // update transform rotation:
                transform.localRotation = Quaternion.Euler(x, y, z);
            }
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Activates and deactivates shaking hand simulation.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Shake()
        {
            yield return new WaitForSeconds(Random.Range(60, 120));
            shakeOn = true;
            // save info about start of the shaking event:
            if (GameManager.instance.AnalyticsEnabled) LevelManager.instance.AddGameEvent(Analytics.EventType.Shaking);
            yield return new WaitForSeconds(Random.Range(30, 90));
            shakeOn = false;
            StartCoroutine(Shake());
        }
        #endregion
    }
}
