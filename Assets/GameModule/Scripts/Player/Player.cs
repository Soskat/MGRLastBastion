using LastBastion.Biofeedback;
using LastBastion.Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Player
{
    /// <summary>
    /// Component that manages Player behaviour.
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private float hrModifier;
        [SerializeField] private float gsrModifier;
        [SerializeField] private DataState hrState;
        [SerializeField] private DataState gsrState;
        private Flashlight flashlight;
        private int averageHR;
        private int averageGSR;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            flashlight = GetComponentInChildren<Flashlight>();

            if (GameManager.instance.BBModule.IsEnabled)
            {
                GameManager.instance.BBModule.BiofeedbackDataChanged += data => { UpdatePlayerState(data); };
            }
            else
            {
                GameManager.instance.BBSimulator.BiofeedbackDataChanged += data => { UpdatePlayerState(data); };
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                flashlight.SwitchLight();
            }
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Updates Player's biofeedback state.
        /// </summary>
        /// <param name="data"></param>
        private void UpdatePlayerState(BiofeedbackData data)
        {
            Debug.Log("Biofeedback update");
            hrModifier = data.HrModifier;
            hrState = data.HrState;
            gsrModifier = data.GsrModifier;
            gsrState = data.GsrState;
        }
        #endregion
    }
}