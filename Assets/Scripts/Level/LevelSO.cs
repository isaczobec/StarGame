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

    public LevelData levelData;


    [Header("LevelID")]
    /// <summary>
    /// The ID of the level, used for saving and loading level data
    /// </summary>
    public string levelID;
    [Header("Level Scene")]
    public string sceneToLoadRefString;
}
