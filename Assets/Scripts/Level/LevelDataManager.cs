using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// A class that handles saving and loading of level data for a levelSO as well as changing its variables during runtime.
/// </summary>
public class LevelDataManager {


    private string levelDataSubPath = "";
    private string defaultFileExtension = ".json";

    public const string LEVEL_DATA_SUB_PATH_DEFAULT = "LevelData";
    public const string EDITOR_LEVEL_DATA_SUB_PATH_DEFAULT = "LevelEditorData";
    public const string DEFAULT_FILE_EXTENSION_DEFAULT = ".json";

    public const string LEVEL_STATS_DATA_DEFAULT_SUFFIX = "STATS";

    /// <summary>
    /// The current level data that is being tracked.
    /// </summary>
    private LevelStatsData currentLevelData;
    private float timeSinceStartupWhenLevelStarted = 0;

    public LevelDataManager(string levelDataSubPath = LEVEL_DATA_SUB_PATH_DEFAULT, string defaultFileExtension = DEFAULT_FILE_EXTENSION_DEFAULT) {
        this.levelDataSubPath = levelDataSubPath;
        this.defaultFileExtension = defaultFileExtension;
    }


    public void Setup() {
        // Subscribe to player death event
        Player.Instance.OnPlayerDeath += IncrementPlayerDeaths;
    }

    /// <summary>
    /// Starts tracking the players stats to the levelData.
    /// </summary>
    /// <param name="levelData"></param>
    public void StartTrackingLevelData(LevelStatsData levelStatsData, float timeSinceStartup) {
        Debug.Log("level : " + levelStatsData.levelName);
        Debug.Log("levelSO.levelData : " + levelStatsData);
        currentLevelData = levelStatsData;
        timeSinceStartupWhenLevelStarted = timeSinceStartup;
    }

    /// <summary>
    /// Stops tracking the players stats to the levelData.
    /// </summary>
    public void StopTrackingLevelData() {
        currentLevelData = null;
    }

    private void IncrementPlayerDeaths(object sender, PlayerDeathEventArgs e)
    {
        currentLevelData.timesDied++;
    }

    public void SetLevelCompleted() {
        currentLevelData.completed = true;
    }


    /// <summary>
    /// Saves the levelData of a levelSO to a file. The file will be named after the levelID of the levelSO.
    /// </summary>
    /// <param name="levelSO"></param>
    public void SaveLevelData(LevelSO levelSO, float timeSinceStartup) {

        // Update level data
        currentLevelData.secondsPlayed += (int)(timeSinceStartup - timeSinceStartupWhenLevelStarted);
        timeSinceStartupWhenLevelStarted = timeSinceStartup; // reset time

        // Save level data
        DataSerializer.Instance.SaveData(currentLevelData, levelDataSubPath, currentLevelData.levelID + LEVEL_STATS_DATA_DEFAULT_SUFFIX);

    }


    /// <summary>
    /// Tries to load and set the LevelData field of a LevelSO. If the file doesn't exist and createIfNotExists is true, it will create a new one and save it.
    /// </summary>
    /// <param name="levelSO"></param>
    /// <param name="createIfNotExists"></param>
    public void LoadLevelData(LevelSO levelSO, bool createIfNotExists = true) {

        // try and load the data
        LoadedData<LevelStatsData> loadedLevelData = DataSerializer.Instance.LoadData<LevelStatsData>(levelDataSubPath, levelSO.levelID + LEVEL_STATS_DATA_DEFAULT_SUFFIX);

        if (!loadedLevelData.didExist) {
            // did not exist, create new level data if it doesn't exist and save it to a file
            if (createIfNotExists) {
                levelSO.levelData = new LevelStatsData(levelSO.levelName);
                DataSerializer.Instance.SaveData(levelSO.levelData, levelDataSubPath, levelSO.levelID);
            }
        } else {
            levelSO.levelData = loadedLevelData.data; // succesful, set the levelData to the levelSO
        }

    }

    /// <summary>
    /// gets a list of all LevelStatsData from files in the levelDataSubPath directory.
    /// </summary>
    public List<LevelStatsData> GetLevelDataList(bool compareToLevelEditorData = true) {

        List<LevelStatsData> levelStatsDatas =  DataSerializer.Instance.LoadDatasInDirectory<LevelStatsData>(levelDataSubPath, LEVEL_STATS_DATA_DEFAULT_SUFFIX, defaultFileExtension);
        if (compareToLevelEditorData) {

            FileInfo[] editorLevelDataFiles = DataSerializer.Instance.GetFilesInDirectory(EDITOR_LEVEL_DATA_SUB_PATH_DEFAULT, "", defaultFileExtension);
            
            foreach (LevelStatsData statsData in levelStatsDatas) {
                bool found = false;
                foreach (FileInfo fileInfo in editorLevelDataFiles) {
                    if (fileInfo.Name.Contains(statsData.levelID)) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    levelStatsDatas.Remove(statsData);
                }
            }
        }
        return levelStatsDatas;
    }

}