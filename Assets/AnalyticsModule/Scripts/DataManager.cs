using LastBastion.Game;
using LastBastion.Game.SurveySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


namespace LastBastion.Analytics
{
    /// <summary>
    /// Class that manages saving data from game for future analysis purposes.
    /// </summary>
    public class DataManager
    {
        #region Private static fields
        /// <summary>Directory path for files with analysis data.</summary>
        private static string dataDirectory;
        /// <summary>Path for file with analysis data from current game.</summary>
        private static string filePath;
        /// <summary>FileStream handler for file with analysis data from current game.</summary>
        private static FileStream file;
        /// <summary>Current player ID.</summary>
        private static long playerID;
        #endregion


        #region Public static methods
        /// <summary>
        /// Initializes DataManager system.
        /// </summary>
        public static void InitializeSystem()
        {
            dataDirectory = Application.streamingAssetsPath + "/TestsData/";
            if (!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);
        }

        /// <summary>
        /// Begins new analysis record.
        /// </summary>
        /// <param name="gameType">Current game mode</param>
        public static void BeginAnalysis(BiofeedbackMode gameType)
        {
            playerID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssff"));
            filePath = dataDirectory + playerID;
            file = File.Create(filePath);

            // save info about player and game settings:
            AddTestInfo(InfoType.ID, playerID);
            AddTestInfo(InfoType.GameType, gameType);
        }

        /// <summary>
        /// Ends current analysis record.
        /// </summary>
        public static void EndAnalysis()
        {
            file.Close();
        }

        /// <summary>
        /// Saves data about game settings.
        /// </summary>
        /// <param name="infoType">Type of game settings data</param>
        /// <param name="value">Value of game settings data</param>
        public static void AddTestInfo(InfoType infoType, object value)
        {
            string data = "- " + infoType.ToString() + " " + value.ToString();
            SaveToFile(data);
        }

        /// <summary>
        /// Saves calibration data (average HR and GSR values) as string "- Avg_HR_GSR [averageHR] [averageGSR]".
        /// </summary>
        /// <param name="infoType">Type of game settings data</param>
        /// <param name="averageHr">Average HR</param>
        /// <param name="averageGsr">Average GSR</param>
        public static void AddCalibrationData(InfoType infoType, int averageHr, int averageGsr)
        {
            string data = "- " + infoType.ToString() + " " + averageHr.ToString() + " " + averageGsr.ToString();
            SaveToFile(data);
        }

        /// <summary>
        /// Saves level data as string: "- [levelName] [calculationType]".
        /// </summary>
        /// <param name="levelName"></param>
        /// <param name="calculationType"></param>
        public static void AddLevelInfo(LevelName levelName, CalculationType calculationType)
        {
            string data = "- " + levelName + " " + calculationType;
            //string data = "- " + levelName + " " + (int)calculationType;
            SaveToFile(data);
        }

        /// <summary>
        /// Saves level data as string: "- [levelName] [calculationType] [averageHr] [averageGsr]".
        /// </summary>
        /// <param name="levelName">Name of the level</param>
        /// <param name="calculationType">Calculation type for calculating arousal from HR and GSR</param>
        /// <param name="averageHr">Average HR value</param>
        /// <param name="averageGsr">Average GSR value</param>
        public static void AddLevelInfo(LevelName levelName, CalculationType calculationType, int averageHr, int averageGsr)
        {
            string data = "- " + (int)levelName + " " + (int)calculationType + " " + averageHr + " " + averageGsr;
            SaveToFile(data);
        }
        
        /// <summary>
        /// Saves game event.
        /// </summary>
        /// <param name="eventType">Type of the event</param>
        /// <param name="time">Time of the event</param>
        /// <param name="value">Additional event object value</param>
        public static void AddGameEvent(EventType eventType, int time, object value = null)
        {
            string data;
            if (value != null) data = (int)eventType + " " + time + " " + value.ToString();
            else data = (int)eventType + " " + time;
            SaveToFile(data);
        }

        /// <summary>
        /// Saves game event.
        /// </summary>
        /// <param name="eventType">Type of the event</param>
        /// <param name="time">Time of the event</param>
        /// <param name="value">Additional event object value</param>
        public static void AddGameEvent(EventType eventType, TimeSpan time, object value = null)
        {
            string data;
            if (value != null) data = eventType + " " + String.Format("{0:00}:{1:00}", time.Minutes, time.Seconds) + " " + value.ToString();
            else data = eventType + " " + String.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
            //if (value != null) data = (int)eventType + " " + String.Format("{0:00}:{1:00}", time.Minutes, time.Seconds) + " " + value.ToString();
            //else data = (int)eventType + " " + String.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
            SaveToFile(data);
        }

        /// <summary>
        /// Saves survey answers.
        /// </summary>
        /// <param name="survey">List of questions</param>
        public static void AddSurveyAnswers(List<Question> questions)
        {
            // save section headline:
            SaveToFile("- survey_answers");
            // save survey answers:
            foreach (Question question in questions) SaveToFile(question.ID + " " + question.AnswerType + " " + question.Answer);
            //foreach(Question question in questions) SaveToFile(question.ID + " " + (int)question.AnswerType + " " + question.Answer);
        }
        #endregion


        #region Private static methods
        /// <summary>
        /// Saves data to file.
        /// </summary>
        /// <param name="value">Data to save</param>
        private static void SaveToFile(string value)
        {
            byte[] data = new UTF8Encoding(true).GetBytes(value + "\n");
            file.Write(data, 0, data.Length);
        }
        #endregion
    }
}