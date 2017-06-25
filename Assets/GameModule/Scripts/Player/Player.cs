using LastBastion.Biofeedback;
using LastBastion.Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Player
{
    public class Player : MonoBehaviour
    {

        #region Private fields
        private Flashlight flashlight;
        private int averageHR;
        private int averageGSR;
        #endregion
        
        [SerializeField] private float hrModifier;
        [SerializeField] private float gsrModifier;
        [SerializeField] private DataState hrState;
        [SerializeField] private DataState gsrState;

        // Use this for initialization
        void Start()
        {
            flashlight = GetComponentInChildren<Flashlight>();

            GameManager.instance.BBModule.BiofeedbackDataChanged += data =>
            {
                UpdatePlayerState(data);
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                flashlight.SwitchLight();
            }
        }


        private void UpdatePlayerState(BiofeedbackData data)
        {
            hrModifier = data.HrModifier;
            hrState = data.HrState;
            gsrModifier = data.GsrModifier;
            gsrState = data.GsrState;
        }
    }
}