using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelSO", menuName = "Level/LevelSO", order = 1)]
public class LevelSO : ScriptableObject
{

    [Header("Level Info")]
    public string levelName;
    public string levelDifficulty;
    public string author;

    public SongSO levelSong;

    public LevelStatsData levelData;


    [Header("LevelID")]
    /// <summary>
    /// The ID of the level, used for saving and loading level stat data etc
    /// </summary>
    public string levelID;
    [Header("For loading")]
    public string sceneToLoadRefString;

    public bool loadLevelIDInstead; // if true, the sceneToLoadRefString will be ignored and the levelID will be used to load the editor level
    
}
