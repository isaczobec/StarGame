using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for saving and loading the level data into the editor.
/// </summary>
public class LevelEditorDataManager : MonoBehaviour
{

    private const string defaultLevelDataSubPath = "LevelEditorData";


    public static LevelEditorDataManager instance {get; private set;}

    public EditorLevelData editorLevelData {get; private set;} = new EditorLevelData();

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadData("TesteditorLevelID");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit() {
        SaveData();
    }

    /// <summary>
    /// Updates the editorLevelData class and serializes it to a file.
    /// </summary>
    public void SaveData() {
        
        editorLevelData.tileArrayDatas = TileArrayManager.instance.GetTileArrayDatas();

        // Save the data to a file
        DataSerializer.Instance.SaveData(editorLevelData, defaultLevelDataSubPath, editorLevelData.editorLevelID);
    }

    /// <summary>
    /// Loads the editorLevelData from a file into the editor.
    /// </summary>
    /// <param name="levelID"></param>
    public void LoadData(string levelID) {
        LoadedData<EditorLevelData> loadedData = DataSerializer.Instance.LoadData<EditorLevelData>(defaultLevelDataSubPath, levelID);

        if (loadedData.didExist) {
            editorLevelData = loadedData.data;

            // Load the data into the tileArrayManager
            TileArrayManager.instance.LoadTileArrayDatas(editorLevelData.tileArrayDatas);
        }
    }
}
