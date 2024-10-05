using System;
using System.Collections.Generic;

[Serializable]
public class EditorLevelData {


    public string editorLevelID = "TesteditorLevelID";
    public string songID = "DefaultLevelSong";

    /// <summary>
    /// The amount of tilen in EACH direction from the center of the level on the x axis.
    /// </summary>
    public int levelSizeX = 500;
    /// <summary>
    /// The amount of tilen in EACH direction from the center of the level on the y axis.
    /// </summary>
    public int levelSizeY = 500;


    public List<EditorObjectData> editorObjectDatas = new List<EditorObjectData>() {
        new EditorObjectData {
            SpawnnableObjectID = "SpawnPoint",
            position = new UnityEngine.Vector2(0, 0),
            rotation = 0,
            scale = new UnityEngine.Vector2(1, 1),
            settings = new List<EditorObjectSetting>() {
                new EditorObjectSetting {
                    settingName = "Start Velocity X",
                    valueType = SettingValueType.FLOAT,
                    value = "0",
                    increment = 10,
                    min = 0,
                    max = 40,
                },
                new EditorObjectSetting {
                    settingName = "Start Velocity Y",
                    valueType = SettingValueType.FLOAT,
                    value = "20",
                    increment = 10,
                    min = 0,
                    max = 40,
                },
                new EditorObjectSetting {
                    settingName = "Starting Gamemode",
                    valueType = SettingValueType.INT,
                    value = "1",
                    increment = 1,
                    min = 1,
                    max = 4,
                },
            },
        }
    };

    public List<TileArrayData> tileArrayDatas = new List<TileArrayData>();

}