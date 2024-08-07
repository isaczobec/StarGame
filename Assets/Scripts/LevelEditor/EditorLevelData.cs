using System;
using System.Collections.Generic;

[Serializable]
public class EditorLevelData {


    public string editorLevelID = "TesteditorLevelID";

    /// <summary>
    /// The amount of tilen in EACH direction from the center of the level on the x axis.
    /// </summary>
    public int levelSizeX = 500;
    /// <summary>
    /// The amount of tilen in EACH direction from the center of the level on the y axis.
    /// </summary>
    public int levelSizeY = 500;

    public List<TileArrayData> tileArrayDatas = new List<TileArrayData>();

}