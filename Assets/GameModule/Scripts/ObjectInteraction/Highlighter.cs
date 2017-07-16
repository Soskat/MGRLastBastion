using LastBastion.Game.Managers;
using UnityEngine;


namespace LastBastion.Game.ObjectInteraction
{
    /// <summary>
    /// Component that manages higlighting interactive game object.
    /// </summary>
    public class Highlighter : MonoBehaviour
    {
        #region Private fields
        private Color basicColor;
        private Color basicEmissionColor;
        private GameObject player;
        private bool isMouseOver = false;
        private bool isInRange = false;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            basicColor = GetComponent<Renderer>().material.color;
            basicEmissionColor = GetComponent<Renderer>().material.GetColor("_EmissionColor");
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (!isMouseOver)
            {
                if ((transform.position - player.transform.position).magnitude <= GameManager.instance.Assets.HintRange)
                {
                    if (!isInRange)
                    {
                        isInRange = true;
                        ManageHintColor();
                    }
                }
                else
                {
                    if (isInRange)
                    {
                        isInRange = false;
                        ManageHintColor();
                    }
                }
            }
        }

        // Called when the mouse enters the GUIElement or Collider
        private void OnMouseEnter()
        {
            isMouseOver = true;
            GetComponent<Renderer>().material.color = GameManager.instance.Assets.HighlightColor;
            GetComponent<Renderer>().material.SetColor("_EmissionColor", GameManager.instance.Assets.HighlightColor);
        }

        // Called when the mouse is not any longer over the GUIElement or Collider
        private void OnMouseExit()
        {
            isMouseOver = false;
            ManageHintColor();
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Sets or removes hint color from game object's material.
        /// </summary>
        private void ManageHintColor()
        {
            if (isInRange)
            {
                GetComponent<Renderer>().material.color = GameManager.instance.Assets.HintColor;
                GetComponent<Renderer>().material.SetColor("_EmissionColor", GameManager.instance.Assets.HintColor);
            }
            else
            {
                GetComponent<Renderer>().material.color = basicColor;
                GetComponent<Renderer>().material.SetColor("_EmissionColor", basicEmissionColor);
            }
        }
        #endregion
    }
}
