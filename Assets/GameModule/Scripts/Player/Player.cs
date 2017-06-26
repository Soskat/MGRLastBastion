using LastBastion.Analytics;
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
        [SerializeField] private float hrModifier;  //--------------------------------- to remove later ?
        [SerializeField] private DataState hrState; //--------------------------------- to remove later ?
        [SerializeField] private float gsrModifier; //--------------------------------- to remove later ?
        [SerializeField] private DataState gsrState;//--------------------------------- to remove later ?
        [SerializeField] private float arousalModifier;
        [SerializeField] private DataState arousalState;
        private Flashlight flashlight;
        private int averageHR;
        private int averageGSR;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            flashlight = GetComponentInChildren<Flashlight>();
            GameManager.instance.BBModule.BiofeedbackDataChanged += data => UpdatePlayerState(data);
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
            hrModifier = data.HrModifier;   //--------------------------------- to remove later ?
            hrState = data.HrState;         //--------------------------------- to remove later ?
            gsrModifier = data.GsrModifier; //--------------------------------- to remove later ?
            gsrState = data.GsrState;       //--------------------------------- to remove later ?
            arousalModifier = data.ArousalModifier;
            arousalState = data.ArousalState;
        }
        #endregion
    }
}