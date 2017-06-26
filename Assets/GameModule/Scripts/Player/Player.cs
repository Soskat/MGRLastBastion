using LastBastion.Analytics;
using LastBastion.Biofeedback;
using LastBastion.Game.Managers;
using System;
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
        //[SerializeField] private float hrModifier;  //--------------------------------- to remove later ?
        //[SerializeField] private DataState hrState; //--------------------------------- to remove later ?
        //[SerializeField] private float gsrModifier; //--------------------------------- to remove later ?
        //[SerializeField] private DataState gsrState;//--------------------------------- to remove later ?
        [SerializeField] private float arousalModifier;
        [SerializeField] private DataState arousalState;
        [SerializeField] private bool isFlashlightEquipped = true;
        //private AudioSource audioSource;
        private int averageHR;
        private int averageGSR;
        #endregion


        #region Public fields & properties
        /// <summary>Current arousal modifier.</summary>
        public float ArousalModifier { get { return arousalModifier; } }
        /// <summary>Current arousal state based on <see cref="arousalModifier"/>.</summary>
        public DataState ArousalState { get { return arousalState; } }
        #endregion


        #region Public actions
        /// <summary>Informs that player want to switch light in the flashlight</summary>
        public Action SwitchLight;
        /// <summary>Informs that light in the flashlight should be shutted down</summary>
        public Action ShutDownLight;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            //audioSource = GetComponent<AudioSource>();
            GameManager.instance.BBModule.BiofeedbackDataChanged += data => UpdatePlayerState(data);
        }

        // Update is called once per frame
        void Update()
        {
            if (isFlashlightEquipped && Input.GetKeyDown(KeyCode.R))
            {
                SwitchLight();
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
            //hrModifier = data.HrModifier;   //--------------------------------- to remove later ?
            //hrState = data.HrState;         //--------------------------------- to remove later ?
            //gsrModifier = data.GsrModifier; //--------------------------------- to remove later ?
            //gsrState = data.GsrState;       //--------------------------------- to remove later ?
            arousalModifier = data.ArousalModifier;
            arousalState = data.ArousalState;
        }

        
        #endregion
    }
}