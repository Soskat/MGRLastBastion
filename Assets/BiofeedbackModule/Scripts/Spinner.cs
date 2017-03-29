using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.BiofeedbackModule.Scripts
{
    class Spinner : MonoBehaviour
    {
        public float Angle = 5.0f;
        public Vector3 SpinAxis = new Vector3(0, 1, 0);

        private void Update()
        {
            gameObject.transform.Rotate(SpinAxis, Angle);
        }
    }
}