using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Player
{
    public class Player : MonoBehaviour
    {

        #region Private fields
        private Flashlight flashlight;
        #endregion


        // Use this for initialization
        void Start()
        {
            flashlight = GetComponentInChildren<Flashlight>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                flashlight.SwitchLight();
            }
        }
    }
}