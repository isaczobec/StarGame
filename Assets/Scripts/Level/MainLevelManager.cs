using System;
using System.Collections.Generic;
using UnityEngine;

public class MainLevelManager : MonoBehaviour {

    [SerializeField] private MainLevelSO[] mainLevelSOs;
    /// <summary>
    /// The current main level that will be saved.
    /// </summary>
    [SerializeField] private MainLevelSO currentMainLevelSO; 


    [SerializeField] private bool isInEditorMode = false;

    [SerializeField] private UIButton saveButton;

    public void Start() {
        if (!isInEditorMode) WriteAllUnwrittenLevelDataToMachine();
        else {
            saveButton.OnUIButtonReleased += SaveCurrentLevelDataButtonClicked;
        }
    }

    private void SaveCurrentLevelDataButtonClicked(object sender, EventArgs e)
    {
        SaveCurrentLevelData();
    }

    public void SaveCurrentLevelData() {
        currentMainLevelSO.SaveCurrentLevelData();
    }

    public void WriteAllUnwrittenLevelDataToMachine() {
        List<MainLevelSO> unwrittenMainLevelSOs = GetUnWrittenMainLevelSOs();
        foreach (MainLevelSO mainLevelSO in unwrittenMainLevelSOs) {
            mainLevelSO.WriteLevelDataToMachine();
        }
    }

    private List<MainLevelSO> GetUnWrittenMainLevelSOs() {
        List<MainLevelSO> unwrittenMainLevelSOs = new List<MainLevelSO>();
        List<LevelStatsData> levelStatsDatas = DataSerializer.Instance.LoadDatasInDirectory<LevelStatsData>(LevelDataManager.LEVEL_DATA_SUB_PATH_DEFAULT, LevelDataManager.LEVEL_STATS_DATA_DEFAULT_SUFFIX);
        foreach (MainLevelSO mainLevelSO in mainLevelSOs) {
            bool isWritten = false;
            foreach (LevelStatsData levelStatsData in levelStatsDatas) {
                if (levelStatsData.levelID == mainLevelSO.levelID) {
                    isWritten = true;
                    break;
                }
            }
            if (!isWritten) unwrittenMainLevelSOs.Add(mainLevelSO);
        }
        return unwrittenMainLevelSOs;
    }

}