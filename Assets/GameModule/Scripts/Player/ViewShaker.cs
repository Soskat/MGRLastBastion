using LastBastion.Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastBastion.Game.Player
{
    public class ViewShaker : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private float initialDelay = 5f;
        [SerializeField] private float duration = 35f;
        [SerializeField] private float magnitude = 5f;
        private bool wasActivated;
        private Camera playerCamera;
        private float initialCameraYPosition;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            wasActivated = false;
            playerCamera = GetComponent<Camera>();
            initialCameraYPosition = playerCamera.transform.localPosition.y;
        }

        // Update is called once per frame
        void Update()
        {
            if (!wasActivated && LevelManager.instance.IsOutroOn)
            {
                StartCoroutine(Shake());
                wasActivated = true;
            }
        }
        #endregion


        #region Private methods
        // based on code from: http://unitytipsandtricks.blogspot.com/2013/05/camera-shake.html
        /// <summary>
        /// Shakes camera view.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Shake()
        {
            yield return new WaitForSeconds(initialDelay);

            float elapsed = 0.0f;

            Vector3 originalCamPos = playerCamera.transform.localPosition;

            while (elapsed < duration)
            {

                elapsed += Time.deltaTime;

                float percentComplete = elapsed / duration;
                float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

                // map value to [-1, 1]
                float x = Random.value * 2.0f - 1.0f;
                float y = Random.value * 2.0f - 1.0f;
                x *= magnitude * damper;
                y *= magnitude * damper;

                playerCamera.transform.localPosition = new Vector3(x, y + initialCameraYPosition, originalCamPos.z);

                yield return null;
            }

            playerCamera.transform.localPosition = originalCamPos;
        }
        #endregion
    }
}
