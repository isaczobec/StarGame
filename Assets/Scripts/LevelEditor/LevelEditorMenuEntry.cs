using System;
using TMPro;
using UnityEngine;

public class LevelEditorMenuEntry : MonoBehaviour {

    [SerializeField] private UIButtonAnimated editLevelButton;
    [SerializeField] private TMP_InputField levelNameInputField;
    [SerializeField] private TMP_InputField levelAuthorInputField;
    [SerializeField] private TMP_InputField levelDifficultyInputField;

    public LevelStatsData levelStatsData { get; private set; }

    public void SetupFromLevelData(LevelStatsData levelData) {
        levelStatsData = levelData;
        levelNameInputField.text = levelData.levelName;
        levelAuthorInputField.text = levelData.author;
        levelDifficultyInputField.text = levelData.difficulty;
        editLevelButton.OnUIButtonReleased += OnEditLevelButtonClicked;
    }


    public void SaveLevelData() {
        levelStatsData.levelName = levelNameInputField.text;
        levelStatsData.author = levelAuthorInputField.text;
        levelStatsData.difficulty = levelDifficultyInputField.text;
        DataSerializer.Instance.SaveData(levelStatsData, LevelDataManager.LEVEL_DATA_SUB_PATH_DEFAULT, levelStatsData.levelID + LevelDataManager.LEVEL_STATS_DATA_DEFAULT_SUFFIX);
    }

    private void OnEditLevelButtonClicked(object sender, EventArgs e)
    {
        LevelEditorMenuHandler.Instance.OnLevelEditButtonClicked(levelStatsData);
    }



}