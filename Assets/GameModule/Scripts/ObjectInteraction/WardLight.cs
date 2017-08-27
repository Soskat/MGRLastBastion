using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that represents ward light behaviour.
    /// </summary>
    public class WardLight : MonoBehaviour
    {
        #region Private fields
        /// <summary>Door that is assigned to this ward light.</summary>
        [SerializeField] private Door watchedDoor;
        /// <summary>Lapm shade object.</summary>
        [SerializeField] private GameObject lampShade;
        /// <summary>Light source.</summary>
        private Light lightSource;
        /// <summary>Bulb material.</summary>
        private Material bulbMaterial;
        /// <summary>Light shade material.</summary>
        private Material lampShadeMaterial;
        /// <summary>Last state of watched door.</summary>
        private bool lastDoorState;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(watchedDoor);
        }

        // Use this for initialization
        void Start()
        {
            lightSource = GetComponentInChildren<Light>();
            bulbMaterial = GetComponent<Renderer>().material;
            if (lampShade != null) lampShadeMaterial = lampShade.GetComponent<Renderer>().material;
            lastDoorState = watchedDoor.IsLocked;
        }

        // Update is called once per frame
        void Update()
        {
            if (watchedDoor.IsLocked != lastDoorState)
            {
                lastDoorState = watchedDoor.IsLocked;
                SwitchLightState();
            }
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Switches color of the light based on door's state.
        /// </summary>
        private void SwitchLightState()
        {
            if (lastDoorState)
            {
                bulbMaterial.SetColor("_EmissionColor", Color.red);
                if (lampShade != null) lampShadeMaterial.SetColor("_EmissionColor", Color.red);
                if (lightSource != null) lightSource.color = Color.red;
            }
            else
            {
                bulbMaterial.SetColor("_EmissionColor", Color.green);
                if (lampShade != null) lampShadeMaterial.SetColor("_EmissionColor", Color.green);
                if (lightSource != null) lightSource.color = Color.green;
            }
        }
        #endregion
    }
}