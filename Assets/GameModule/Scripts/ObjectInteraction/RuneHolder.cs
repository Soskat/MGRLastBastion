using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    public class RuneHolder : MonoBehaviour
    {
        #region Private fields

        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            foreach (Transform child in transform) child.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion
    }
}
