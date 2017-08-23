using LastBastion.Game.UIControllers;
using System;
using UnityEngine;
using UnityEngine.Assertions;


namespace LastBastion.Game.Managers
{
    public class AchievementManager : MonoBehaviour
    {
        #region Private fields
        [SerializeField] private AchievementPanelController timeAchievement;
        [SerializeField] private AchievementPanelController runesAchievement;
        [SerializeField] private AchievementPanelController roomsAchievement;
        [SerializeField] private AchievementPanelController lightSwitchAchievement;
        #endregion


        #region MonoBehaviour methods
        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            Assert.IsNotNull(timeAchievement);
            Assert.IsNotNull(runesAchievement);
            Assert.IsNotNull(roomsAchievement);
            Assert.IsNotNull(lightSwitchAchievement);
        }

        // Use this for initialization
        void Start()
        {
            // update achievements data:
            UpdateAchievementsPanels();
        }
        #endregion


        #region Public methods
        /// <summary>
        /// Updates achievement panels with current values.
        /// </summary>
        private void UpdateAchievementsPanels()
        {
            string timeAchievementTitle, runesAchievementTitle, roomsAchievementTitle, lightSwitchAchievementTitle;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
                                               GameManager.instance.GameTime.Hours,
                                               GameManager.instance.GameTime.Minutes,
                                               GameManager.instance.GameTime.Seconds);
            // polish is the choosen language:
            if (GameManager.instance.ChoosenLanguage == GameLanguage.Polish)
            {
                // update time achievement:
                if (GameManager.instance.GameTime.Minutes < 10) timeAchievementTitle = "Szybki & Wściekły >>";
                else timeAchievementTitle = "Jeszcze momencik >>";
                timeAchievement.UpdateAchievementData(timeAchievementTitle, elapsedTime);
                // update runes achievement:
                if (GameManager.instance.CollectedRunes < GameManager.instance.RunesAmount) runesAchievementTitle = "Szczęśliwy traf >>";
                else runesAchievementTitle = "Kolekcjoner >>";
                runesAchievement.UpdateAchievementData(runesAchievementTitle, GameManager.instance.CollectedRunes.ToString());
                // update rooms achievement:
                if (GameManager.instance.SearchedRooms < 29) roomsAchievementTitle = "Tylko przechodziłem >>";
                else roomsAchievementTitle = "Każdy zakamarek >>";
                roomsAchievement.UpdateAchievementData(roomsAchievementTitle, GameManager.instance.SearchedRooms.ToString());
                // update light switch achievement:
                if (GameManager.instance.LightSwitchUses < 5) lightSwitchAchievementTitle = "To nie ja! >>";
                else lightSwitchAchievementTitle = "Beznadziejny klikacz >>";
                lightSwitchAchievement.UpdateAchievementData(lightSwitchAchievementTitle, GameManager.instance.LightSwitchUses.ToString());
            }
            // english is the choosen language:
            else
            {
                // update time achievement:
                if (GameManager.instance.GameTime.Minutes < 10) timeAchievementTitle = "Fast & Furious >>";
                else timeAchievementTitle = "One moment please >>";
                timeAchievement.UpdateAchievementData(timeAchievementTitle, elapsedTime);
                // update runes achievement:
                if (GameManager.instance.CollectedRunes < GameManager.instance.RunesAmount) runesAchievementTitle = "Lucky find >>";
                else runesAchievementTitle = "The Collector >>";
                runesAchievement.UpdateAchievementData(runesAchievementTitle, GameManager.instance.CollectedRunes.ToString());
                // update rooms achievement:
                if (GameManager.instance.SearchedRooms < 29) roomsAchievementTitle = "Just passing by >>";
                else roomsAchievementTitle = "Every nook & cranny >>";
                roomsAchievement.UpdateAchievementData(roomsAchievementTitle, GameManager.instance.SearchedRooms.ToString());
                // update light switch achievement:
                if (GameManager.instance.LightSwitchUses < 5) lightSwitchAchievementTitle = "It wasn't me! >>";
                else lightSwitchAchievementTitle = "Helpless clicker >>";
                lightSwitchAchievement.UpdateAchievementData(lightSwitchAchievementTitle, GameManager.instance.LightSwitchUses.ToString());
            }
        }
        #endregion
    }
}
