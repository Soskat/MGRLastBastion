using LastBastion.Game.SurveySystem;
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
        private Question question;
        #endregion


        #region Public fields & properties
        /// <summary>The question assigned to the question panel.</summary>
        public Question Question { get { return question; } }
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
        /// Updates contents the question panel.
        /// </summary>
        /// <param name="_question">The question</param>
        /// <param name="dropdown">Dropdown menu prefab</param>
        public void UpdatePanel(Question _question, GameObject dropdown)
        {
            question = _question;
            questionContent.text = question.Content;
            dropdownMenu = Instantiate(dropdown, transform);
            dropdownMenu.GetComponent<RectTransform>().localPosition = dropdownPosition.localPosition;
            dropdownMenu.GetComponent<RectTransform>().sizeDelta = dropdownPosition.sizeDelta;
        }

        /// <summary>
        /// Saves number of selected option from dropdown menu.
        /// </summary>
        public void SaveAnswer()
        {
            // value - 1 because first option is always empty:
            question.Answer = (dropdownMenu.GetComponent<Dropdown>().value - 1).ToString();
        }
        #endregion
    }
}
