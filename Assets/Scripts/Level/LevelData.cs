

using System;

/// <summary>
/// A class for saving level data like name and ID and data that has to do with the player having played the level, ie highscore, time, etc. 
/// </summary>
[System.Serializable]
public class LevelStatsData {
    public string levelID;
    public string levelName;
    public int secondsPlayed = 0;
    public int timesDied = 0;
    public bool completed = false;
    public string difficulty = "Normal";
    public string author = "John Doe";
    public string songID = "DefaultLevelSong";

    public LevelStatsData(string levelName) {
        this.levelName = levelName;
        this.levelID = Guid.NewGuid().ToString();
    }

}