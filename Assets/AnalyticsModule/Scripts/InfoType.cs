namespace LastBastion.Analytics
{
    /// <summary>
    /// Represents types of information that describes current game.
    /// </summary>
    public enum InfoType : short
    {
        ID,             // player's ID
        GameType,       // type of the game
        Avg_HR_GSR,     // callibrated average values of HR and GSR
        LevelInfo,      // information about the level
    }
}