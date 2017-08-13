using LastBastion.Game.UIControllers;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace LastBastion.Game.Managers
{
    /// <summary>
    /// Component that manages Summary scene behaviours.
    /// </summary>
    public class SummaryManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private Button endSceneButton;
        [SerializeField] private Button backToMainMenuButton;
        [SerializeField] private AchievementPanelController timeAchievement;
        [SerializeField] private AchievementPanelController runesAchievement;
        [SerializeField] private AchievementPanelController doorsAchievement;
        [SerializeField] private AchievementPanelController lightSwitchAchievement;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(timeAchievement);
            Assert.IsNotNull(runesAchievement);
            Assert.IsNotNull(doorsAchievement);
            Assert.IsNotNull(lightSwitchAchievement);
        }

        // Use this for initialization
        void Start()
        {
            // update buttons behaviour:
            endSceneButton.onClick.AddListener(() => { GameManager.instance.LevelHasEnded(); });
            backToMainMenuButton.onClick.AddListener(() => { GameManager.instance.BackToMainMenu(); });

            // update achievements data:
            UpdateAchievementsPanels();

            // unlock cursor state after game level:
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


        #region Public methods
        private void UpdateAchievementsPanels()
        {
            // TEST:
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
                                               GameManager.instance.GameTime.Hours,
                                               GameManager.instance.GameTime.Minutes,
                                               GameManager.instance.GameTime.Seconds);
            timeAchievement.UpdateAchievementData("time achik >>", elapsedTime);
            runesAchievement.UpdateAchievementData("runes achik >>", GameManager.instance.CollectedRunes.ToString());
            doorsAchievement.UpdateAchievementData("doors achik >>", GameManager.instance.OpenedDoors.ToString());
            lightSwitchAchievement.UpdateAchievementData("light switch achik >>", GameManager.instance.LightSwitchUses.ToString());
        }
        #endregion
    }
}