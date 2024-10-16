using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the level editor menu. Creating, saving and loading levels. Also handles the transition to the level editor scene.
/// </summary>
public class LevelEditorMenuHandler : MonoBehaviour
{

    public static LevelEditorMenuHandler Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private TMP_InputField levelNameInputField;
    [SerializeField] private UIButtonAnimated createLevelButton;
    [SerializeField] private UIButtonAnimated returnToMenuButton;
    [SerializeField] private UIButtonAnimated gameTitleButton;

    [SerializeField] private GameObject levelEditorMenuEnrtyPrefab;

    [SerializeField] private float entrySpacing = 50f;
    [SerializeField] private float entryStartYOffset = 100f;
    [SerializeField] private int maxEntriesPerColumn = 10;

    public LevelStatsData levelStatsDataToEdit { get; private set; }

    /// <summary>
    /// A list of all the level editor menu entries currently loaded in.
    /// </summary>
    List<LevelEditorMenuEntry> levelEditorMenuEntries = new List<LevelEditorMenuEntry>(); 

    private const string LEVEL_EDITOR_SCENE_REF = "LevelEditorScene";
    


    private void Start()
    {
        createLevelButton.OnUIButtonReleased += OnCreateLevelButtonClicked;
        returnToMenuButton.OnUIButtonReleased += OnReturnToMenuButtonClicked;
        levelNameInputField.transform.localScale = new Vector3(0, 0, 0);
    }


    public void ShowMenu() {
        createLevelButton.ChangeVisible(true);
        returnToMenuButton.ChangeVisible(true);
        gameTitleButton.ChangeVisible(false);
        levelNameInputField.transform.localScale = new Vector3(1, 1, 1);
        CreateLevelEditorMenuEntries();
    }

    public void HideMenu() {
        createLevelButton.ChangeVisible(false);
        returnToMenuButton.ChangeVisible(false);
        gameTitleButton.ChangeVisible(true);
        levelNameInputField.transform.localScale = new Vector3(0, 0, 0);
        ClearAndDestroyLevelEditorMenuEntries();
    }


    private void CreateLevelEditorMenuEntries() {
        List<LevelStatsData> levelStatsDatas = LevelHandler.Insance.levelDataManager.GetLevelDataList();
        for (int i = 0; i < levelStatsDatas.Count; i++)
        {
            if (levelStatsDatas[i].isMainLevel) continue; // dont allow editing of main levels
            LevelStatsData levelStatsData = levelStatsDatas[i];
            CreateLevelEditorMenuEntry(i, levelStatsData);
        }
    }

    private void CreateLevelEditorMenuEntry(int i, LevelStatsData levelStatsData)
    {
        GameObject entryObject = Instantiate(levelEditorMenuEnrtyPrefab, transform);
        entryObject.transform.localPosition = new Vector3(0, entryStartYOffset - i * entrySpacing, 0);
        LevelEditorMenuEntry levelEditorMenuEntry = entryObject.GetComponent<LevelEditorMenuEntry>();
        levelEditorMenuEntries.Add(levelEditorMenuEntry);
        levelEditorMenuEntry.SetupFromLevelData(levelStatsData);
    }


    private void ClearAndDestroyLevelEditorMenuEntries() {
        foreach (LevelEditorMenuEntry levelEditorMenuEntry in levelEditorMenuEntries) {
            Destroy(levelEditorMenuEntry.gameObject);
        }
        levelEditorMenuEntries.Clear();
    }

    private void SaveAllLevelData() {
        foreach (LevelEditorMenuEntry levelEditorMenuEntry in levelEditorMenuEntries) {
            levelEditorMenuEntry.SaveLevelData();
        }
    }

    
    private void OnReturnToMenuButtonClicked(object sender, EventArgs e)
    {
        SaveAllLevelData(); // save all level data before returning to menu
        HideMenu();
        InitialMenuButtonsHandler.Instance.ShowInitialButtons();
    }

    private void OnCreateLevelButtonClicked(object sender, EventArgs e)
    {
        string levelName = levelNameInputField.text;
        levelNameInputField.text = ""; // clear the input field

        // create level stats data and save it
        LevelStatsData levelStatsData = new LevelStatsData(levelName);
        DataSerializer.Instance.SaveData(levelStatsData, LevelDataManager.LEVEL_DATA_SUB_PATH_DEFAULT, levelStatsData.levelID+LevelDataManager.LEVEL_STATS_DATA_DEFAULT_SUFFIX);
        
        // create editor level data and save it
        EditorLevelData editorLevelData = new EditorLevelData {
            editorLevelID = levelStatsData.levelID
        };
        Debug.Log("editorLevelData.editorLevelID: " + editorLevelData.editorObjectDatas.Count);
        DataSerializer.Instance.SaveData(editorLevelData, LevelDataManager.EDITOR_LEVEL_DATA_SUB_PATH_DEFAULT, editorLevelData.editorLevelID);

        // create a new level editor menu entry
        CreateLevelEditorMenuEntry(levelEditorMenuEntries.Count, levelStatsData);
    }

    public void OnLevelEditButtonClicked(LevelStatsData levelStatsData) {
        levelStatsDataToEdit = levelStatsData;
        SaveAllLevelData(); 
        HideMenu();
        LevelHandler.Insance.DestroyDontDestroyOnLoadObjects();
        DontDestroyOnLoad(gameObject); // save this object for the level editor scene
        MusicManager.insance.StopAllMusic(0.5f);
        SceneManager.LoadScene(LEVEL_EDITOR_SCENE_REF);
        
    }
}
