using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Responsible for loading editor data into a playable level.
/// Assumes a TileArrayManager and LevelEditorDataManager Instance is present in the scene.
/// </summary>
public class EditorLevelDataLoader : MonoBehaviour
{
    private const string defaultLevelDataSubPath = "LevelEditorData";

    /// <summary>
    /// A list of all spawnable level objects. Used to spawn objects in the level.
    /// </summary>
    [SerializeField] private List<SpawnnableLevelObjectSO> spawnnableLevelObjectSOs;

    public static EditorLevelDataLoader instance {get; private set;}

    private void Awake() {
        instance = this;
    }

    public void LoadToPlayableLevel(string levelID) {


        // try to load the editor level data
        EditorLevelData editorLevelData = LevelEditorDataManager.instance.LoadEditorLevelData(levelID);

        // if it is not null, load the level
        if (editorLevelData != null) {

            // Load the data into the tileArrayManager
            TileArrayManager.instance.LoadTileArrayDatas(editorLevelData.tileArrayDatas);

            // spawn the objects
            LoadSpawnableLevelObjects(editorLevelData);
        }

    }

    private void LoadSpawnableLevelObjects(EditorLevelData editorLevelData) {
        foreach (EditorObjectData editorObjectData in editorLevelData.editorObjectDatas) {

            foreach (SpawnnableLevelObjectSO spawnnableLevelObjectSO in spawnnableLevelObjectSOs) {
                if (spawnnableLevelObjectSO.SpawnnableObjectID == editorObjectData.SpawnnableObjectID) {
                    GameObject newObject = Instantiate(spawnnableLevelObjectSO.prefab, editorObjectData.position, Quaternion.identity);
                    ISpawnFromEditorObjectData iSpawnFromEditorObjectData = newObject.GetComponent<ISpawnFromEditorObjectData>();
                    if (iSpawnFromEditorObjectData != null) {
                        iSpawnFromEditorObjectData.CopyEditorObjectData(editorObjectData);
                    }
                }
            }

        }
    }

}
