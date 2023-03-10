using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace LastBastion.Game.UIControllers
{
    /// <summary>
    /// Component that manages behaviour of AchievementPanel game objects.
    /// </summary>
    public class AchievementPanelController : MonoBehaviour
    {
        #region Private fields
        /// <summary>Name of the achievement.</summary>
        [SerializeField] private Text headlineText;
        /// <summary>Value of the achievement.</summary>
        [SerializeField] private Text valueText;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(headlineText);
            Assert.IsNotNull(valueText);
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Updates achievement panel details.
        /// </summary>
        /// <param name="headline">Headline content</param>
        /// <param name="value">Value content</param>
        public void UpdateAchievementData(string headline, string value)
        {
            headlineText.text = headline;
            valueText.text = value;
        }
        #endregion
    }
}