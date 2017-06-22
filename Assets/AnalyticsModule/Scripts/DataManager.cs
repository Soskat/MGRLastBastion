using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataManager {

    private static string dataDirectory;
    private static string filePath;
    private static FileStream file;

    private static long playerID;

    // initialize whole system:
    public static void InitializeSystem()
    {
        dataDirectory = Application.streamingAssetsPath + "/TestsData/";
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }
    }

    // begins analysis for new test object:
    public static void BeginAnalysis(GameType gameType)
    {
        playerID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssff"));
        filePath = dataDirectory + playerID;
        file = File.Create(filePath);

        // save info about player and game settings:
        AddTestInfo(InfoType.ID, playerID);
        AddTestInfo(InfoType.GameType, gameType);
    }

    // ends current analysis:
    public static void EndAnalysis()
    {
        file.Close();
    }






    // saves test info:
    public static void AddTestInfo(InfoType infoType, object value)
    {
        string data = "- " + infoType.ToString() + " " + value.ToString();
        SaveToFile(data);
    }

    /// <summary>
    /// Saves level info as string: "- [levelName] [calculationType] [averageHr] [averageGsr]".
    /// </summary>
    /// <param name="levelName">Name of the level</param>
    /// <param name="calculationType">Calculation type</param>
    /// <param name="averageHr">Average HR value</param>
    /// <param name="averageGsr">Average GSR value</param>
    public static void AddLevelInfo(string levelName, CalculationType calculationType, int averageHr, int averageGsr)
    {
        string data = "- " + levelName + " " + calculationType + " " + averageHr + " " + averageGsr;
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
        if (value != null)
        {
            data = eventType + " " + time + " " + value.ToString();
        }
        else
        {
            data = eventType + " " + time;
        }
        SaveToFile(data);
    }
    //public static void AddGameEvent(EventType eventType, int time, params object[] values)
    //{
    //    string data;
    //    if (values.Length > 0)
    //    {
    //        data = eventType + " " + time + " " + values[0].ToString();
    //    }
    //    else
    //    {
    //        data = eventType + " " + time;
    //    }
    //    SaveToFile(data);
    //}


    /// <summary>
    /// Saves data to file.
    /// </summary>
    /// <param name="value">Data to save</param>
    private static void SaveToFile(string value)
    {
        byte[] data = new UTF8Encoding(true).GetBytes(value);
        file.Write(data, 0, data.Length);
    }


    private static void SaveSurveyToFile()
    {

    }



}
