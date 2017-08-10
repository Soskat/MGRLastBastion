using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LastBastion.Game.UIControllers
{
    /// <summary>
    /// Component that manages behaviour of QuestionPage game object.
    /// </summary>
    public class QuestionsPageController : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;
        [SerializeField] private List<float> yPositions;
        [SerializeField] private List<QuestionPanelController> questions;
        private int questionSpaceUnits = 7;                                 // determines how many questions can be displayed on page
        #endregion


        #region MonoBehaviour methods
        // Use this for initialization
        void Start()
        {
            nextButton.onClick.AddListener(() => { GetComponentInParent<QuestionnairePanelController>().NextPage(); });
            backButton.onClick.AddListener(() => { GetComponentInParent<QuestionnairePanelController>().PreviousPage(); });
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Public methods
        public int CreateQuestionsPage(int currentQuestion)
        {

            return 0;
        }
        #endregion
    }
}
