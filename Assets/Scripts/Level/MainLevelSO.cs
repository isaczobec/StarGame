using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainLevelSO", menuName = "MainLevelSO", order = 51)]
public class MainLevelSO : ScriptableObject
{
    /// <summary>
    /// The level data for the level.
    /// </summary>
    public EditorLevelData editorLevelData;
    public string levelID;
    public string levelName;
    public string difficulty;


    public void SaveCurrentLevelData() {
        // save the level editor data to a file and set the levelID and levelName to the level editor data.
        LevelEditorDataManager.instance.SaveData();
        editorLevelData = LevelEditorDataManager.instance.editorLevelData;
        editorLevelData.editorLevelID = levelID;
    }


    public void WriteLevelDataToMachine() {
        // save the level editor data to a file and set the levelID and levelName to the level editor data.
        DataSerializer.Instance.SaveData(editorLevelData, LevelDataManager.EDITOR_LEVEL_DATA_SUB_PATH_DEFAULT, levelID);

        // create level stats data and save it
        LevelStatsData levelStatsData = new LevelStatsData(levelName);
        levelStatsData.levelID = levelID;
        levelStatsData.isMainLevel = true;
        levelStatsData.author = "Main Level";
        levelStatsData.difficulty = difficulty;
        DataSerializer.Instance.SaveData(levelStatsData, LevelDataManager.LEVEL_DATA_SUB_PATH_DEFAULT, levelStatsData.levelID + LevelDataManager.LEVEL_STATS_DATA_DEFAULT_SUFFIX);
    }
}
