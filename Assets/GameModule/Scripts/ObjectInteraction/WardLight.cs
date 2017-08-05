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
        [SerializeField] private Door watchedDoor;
        [SerializeField] private GameObject lampShade;
        private Light wardLight;
        private Material bulbMaterial;
        private Material lampShadeMaterial;
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
            wardLight = GetComponentInChildren<Light>();
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
        /// Switches light state -> color.
        /// </summary>
        private void SwitchLightState()
        {
            if (lastDoorState)
            {
                bulbMaterial.SetColor("_EmissionColor", Color.red);
                if (lampShade != null) lampShadeMaterial.SetColor("_EmissionColor", Color.red);
                if (wardLight != null) wardLight.color = Color.red;
            }
            else
            {
                bulbMaterial.SetColor("_EmissionColor", Color.green);
                if (lampShade != null) lampShadeMaterial.SetColor("_EmissionColor", Color.green);
                if (wardLight != null) wardLight.color = Color.green;
            }
        }
        #endregion
    }
}
