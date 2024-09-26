

using System;

/// <summary>
/// A class for saving level data like name and ID and data that has to do with the player having played the level, ie highscore, time, etc. 
/// </summary>
[System.Serializable]
public class LevelStatsData {
    public string levelID;
    public string levelName;
    public int secondsPlayed;
    public int timesDied;
    public bool completed;

}