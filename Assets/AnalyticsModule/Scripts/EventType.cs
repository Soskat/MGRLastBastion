namespace LastBastion.Analytics
{
    /// <summary>
    /// Represents types of the game events that are stored via <see cref="DataManager"/>.
    /// </summary>
    public enum EventType
    {
        GameStart,      // start of the game
        GameEnd,        // end of the game
        HrData,         // current HR value
        GsrData,        // current GSR value
        ArousalData,    // current arousal modifier value
        Sound,          // sound event has occurerd
        Light,          // light event has occurerd
        Decals,         // decals event has occurerd
        TV,             // TV event has occurerd
        Flashlight,     // flashlight event has occurerd
        Heartbeat,      // heartbeat sound event has occurerd
        Shaking         // shaking event has occurerd
    }
}