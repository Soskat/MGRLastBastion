using LastBastion.Game.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace LastBastion.Game.UIControllers
{
    /// <summary>
    /// Component that translates Text content of UI element based on choosen game language.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LabelTranslator : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private string textInEnglish;
        [SerializeField] private string textInPolish;
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            UpdateLabelLanguage();
            GameManager.instance.UpdatedLanguage += UpdateLabelLanguage;
        }
        #endregion


        #region Private methods
        /// <summary>
        /// Updates label content with text in choosen game language.
        /// </summary>
        private void UpdateLabelLanguage()
        {
            switch (GameManager.instance.ChoosenLanguage)
            {
                case GameLanguage.Polish:
                    if (textInPolish == "") goto default;
                    GetComponent<Text>().text = textInPolish;
                    break;

                case GameLanguage.English:
                default:
                    GetComponent<Text>().text = textInEnglish;
                    break;
            }
        }
        #endregion
    }
}
