

using System;

/// <summary>
/// A class for saving level data that has to do with the player having played the level, ie highscore, time, etc. NOT the level name or scene.
/// </summary>
[System.Serializable]
public class LevelData {

    public int secondsPlayed;
    public int timesDied;
    public bool completed;

}