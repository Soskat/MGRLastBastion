using UnityEngine;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages rendering of game object's children.
    /// </summary>
    public class RenderManager : MonoBehaviour
    {
        #region Private fields
        /// <summary>Horizontal clipping distance.</summary>
        [SerializeField] private float horizontalClippingDistance = 20f;
        /// <summary>Vertical clipping distance.</summary>
        [SerializeField] private float verticalClippingDistance = 6f;
        /// <summary>Is game object in player's range?</summary>
        [SerializeField] private bool isInRange = false;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            if (Mathf.Abs(transform.position.y - LevelManager.instance.Player.transform.position.y) > verticalClippingDistance) SwitchVisibilityTo(false);
            else
            {
                if ((transform.position - LevelManager.instance.Player.transform.position).magnitude < horizontalClippingDistance) SwitchVisibilityTo(true);
                else SwitchVisibilityTo(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Mathf.Abs(transform.position.y - LevelManager.instance.Player.transform.position.y) > verticalClippingDistance) SwitchVisibilityTo(false);
            else
            {
                if ((transform.position - LevelManager.instance.Player.transform.position).magnitude < horizontalClippingDistance)
                {
                    if (!isInRange) SwitchVisibilityTo(true);
                }
                else if (isInRange) SwitchVisibilityTo(false);
            }

            if (LevelManager.instance.RenderManagerOn && isInRange) Debug.DrawLine(LevelManager.instance.Player.transform.position, transform.position, Color.magenta);
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Switches visibility status of game object's children.
        /// </summary>
        /// <param name="visibility">New visibility status</param>
        private void SwitchVisibilityTo(bool visibility)
        {
            isInRange = visibility;
            foreach(Transform child in transform) child.gameObject.SetActive(visibility);
        }
        #endregion
    }
}
