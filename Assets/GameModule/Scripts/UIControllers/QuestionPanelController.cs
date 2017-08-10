using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace LastBastion.Game.UIControllers
{
    /// <summary>
    /// Component that manages behaviour of QuestionPanel game object.
    /// </summary>
    public class QuestionPanelController : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Text questionContent;
        [SerializeField] private RectTransform dropdownPosition;
        private GameObject dropdownMenu;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(questionContent);
            Assert.IsNotNull(dropdownPosition);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Public methods
        /// <summary>
        /// Updates content and dropdown menu of the question panel.
        /// </summary>
        /// <param name="content">Content of the question</param>
        /// <param name="dropdown">Dropdown menu</param>
        public void UpdatePanel(string content, GameObject dropdown)
        {
            questionContent.text = content;
            dropdownMenu = Instantiate(dropdown, transform);
            dropdownMenu.GetComponent<RectTransform>().localPosition = dropdownPosition.localPosition;
            dropdownMenu.GetComponent<RectTransform>().sizeDelta = dropdownPosition.sizeDelta;
        }

        /// <summary>
        /// Returns number of selected option from dropdown menu.
        /// </summary>
        /// <returns>Selected option number</returns>
        public int SaveAnswer()
        {
            // value - 1 because first option is always empty:
            return dropdownMenu.GetComponent<Dropdown>().value - 1;
        }
        #endregion
    }
}
