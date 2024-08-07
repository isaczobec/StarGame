using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileArrayManager : MonoBehaviour
{
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile testTile;

    [SerializeField] private List<AutoTileSetSO> autoTileSet;
    private List<TileArray> tileArrays = new List<TileArray>();
    private TileArray currentTileArray;

    public static TileArrayManager instance;

    // matricies

    private Dictionary<int,Matrix4x4> rotationDict = new Dictionary<int,Matrix4x4>() {
        {0, Matrix4x4.identity},
        {1, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90))},
        {2, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180))},
        {3, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270))},
    };

    public static Vector2Int[] directions = new Vector2Int[] {
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(-1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
    }; 

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("There is already an instance of TileArrayManager in the scene.");
        }

        foreach (AutoTileSetSO autoTileSet in autoTileSet) {
            autoTileSet.GenerateAllTileBitMaskAndRotations();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
        // // Create the tile arrays
        // foreach (AutoTileSetSO autoTileSet in autoTileSet) {
        //     tileArrays.Add(new TileArray(autoTileSet, 
        //     LevelEditorDataManager.instance.editorLevelData.levelSizeX * 2, 
        //     LevelEditorDataManager.instance.editorLevelData.levelSizeY * 2));
        // }

        // currentTileArray = tileArrays[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// loads in tilearray data to new tillearrays and spawns blocks in the world.
    /// </summary>
    /// <param name="datas"></param>
    public void LoadTileArrayDatas(List<TileArrayData> datas, bool setCurrentTileArray = true) {
        tileArrays.Clear(); // clear so new data can be set
        for (int i = 0; i < datas.Count; i++) {
            tileArrays.Add(new TileArray(null, LevelEditorDataManager.instance.editorLevelData.levelSizeX * 2, LevelEditorDataManager.instance.editorLevelData.levelSizeY * 2));
            tileArrays[i].LoadTileArrayData(datas[i]);
        }

        // set the blocks
        foreach (TileArray tileArray in tileArrays) {
            for (int i = tileArray.LowerLeftBound.x; i <= tileArray.UpperRightBound.x; i++) {
                for (int j = tileArray.LowerLeftBound.y; j <= tileArray.UpperRightBound.y; j++) {
                    UpdateAndPlaceTile(tileArray, i, j, true);
                }
            }
        }

        if (setCurrentTileArray) {
            currentTileArray = tileArrays[0];
        }
    }



    public void TryPlaceTile(Vector3 pos) {
        Vector3Int tilePos = tilemap.WorldToCell(pos);

        // only proceed if the tile is within the bounds of the level
        if (!tilemap.HasTile(tilePos)) {

            // Set solid
            currentTileArray.SetSolid(tilePos.x, tilePos.y, true);

            // place the tile
            UpdateAndPlaceTile(currentTileArray, tilePos.x, tilePos.y);

            // update the surrounding tiles
            for (int i = 0; i < directions.Length; i++) {
                Vector2Int direction = directions[i];
                UpdateAndPlaceTile(currentTileArray, tilePos.x + direction.x, tilePos.y + direction.y, true);
            }

        }
        
    }

    public void UpdateAndPlaceTile(TileArray tileArray, int xWorldPos, int yWorldPos, bool onlyUpdate = false) {

        // get if it is solid
        bool solid = tileArray.solidArray[xWorldPos + LevelEditorDataManager.instance.editorLevelData.levelSizeX, yWorldPos + LevelEditorDataManager.instance.editorLevelData.levelSizeY];
        if (onlyUpdate && !solid) return;

        // get the tile and rotation
            TileBitMaskAndRotation tileBitMaskAndRotation = tileArray.GetTileBitMaskAndRotation(xWorldPos, yWorldPos);
            TileChangeData tileChangeData = new TileChangeData {
                tile = tileBitMaskAndRotation.tile,
                position = new Vector3Int(xWorldPos, yWorldPos, 0),
                transform = rotationDict[tileBitMaskAndRotation.rotation] // rotation matrix from a predefined dictionary
            };

            // set the tile
            tilemap.SetTile(tileChangeData, false);

    }

    /// <summary>
    /// Returns a list of TileArrayDatas from the tileArrays.
    /// </summary>
    /// <returns></returns>
    public List<TileArrayData> GetTileArrayDatas() {
        List<TileArrayData> tileArrayDatas = new List<TileArrayData>();
        foreach (TileArray tileArray in tileArrays) {
            tileArrayDatas.Add(tileArray.GenerateTileArrayData());
        }
        return tileArrayDatas;
    }


    /// <summary>
    /// returns an autotileset from an ID. Null if not found.
    /// </summary>
    /// <param name="autoTileSetID"></param>
    /// <returns></returns>
    public AutoTileSetSO GetAutoTileSetSOFromID(string autoTileSetID) {
        foreach (AutoTileSetSO autoTileSet in autoTileSet) {
            if (autoTileSet.autoTileSetID == autoTileSetID) {
                return autoTileSet;
            }
        }
        return null;
    }
}










public class TileArray {

    /// <summary>
    /// An array determining if a tile is solid or not, true if solid, false if not.
    /// </summary>
    public bool[,] solidArray;
    public AutoTileSetSO autoTileSet;


    /// <summary>
    /// The WORLD lower bound of the solid array.
    /// </summary>
    private Vector2Int lowerLeftBound;
    public Vector2Int LowerLeftBound { get { return lowerLeftBound; } }

    /// <summary>
    /// The WORLD upper bound of the solid array.
    /// </summary>
    private Vector2Int upperRightBound;
    public Vector2Int UpperRightBound { get { return upperRightBound; } }

    public TileArray(AutoTileSetSO autoTileSet, int xWidth, int yHeight) {
        this.autoTileSet = autoTileSet;
        solidArray = new bool[xWidth, yHeight];
    }


    private int GetBitMaskAtCoordinates(int worldX, int worldY) {

        // calculate tile array index
        int tileArrayXIndex = worldX + LevelEditorDataManager.instance.editorLevelData.levelSizeX;
        int tileArrayYIndex = worldY + LevelEditorDataManager.instance.editorLevelData.levelSizeY;

        // get the bit mask
        int bitMask = 0;
        for (int i = 0; i < TileArrayManager.directions.Length; i++) {
            Vector2Int direction = TileArrayManager.directions[i];
            int x = tileArrayXIndex + direction.x;
            int y = tileArrayYIndex + direction.y;

            if (x >= 0 && x < solidArray.GetLength(0) && y >= 0 && y < solidArray.GetLength(1)) {
                if (solidArray[x, y]) {
                    bitMask += (int)Mathf.Pow(2, i);
                }
            }
        }

        return bitMask;
    }

    public TileBitMaskAndRotation GetTileBitMaskAndRotation(int worldX, int worldY) {
        int bitMask = GetBitMaskAtCoordinates(worldX, worldY);
        return autoTileSet.GetTileBitMaskAndRotation(bitMask);
    }

    public void SetSolid(int worldX , int worldY, bool solid) {
        Vector2Int pos = new Vector2Int(worldX + LevelEditorDataManager.instance.editorLevelData.levelSizeX, worldY + LevelEditorDataManager.instance.editorLevelData.levelSizeY);
        solidArray[pos.x, pos.y] = solid;

        // update bounds
        if (solid) {
            if (worldX < lowerLeftBound.x) lowerLeftBound.x = worldX;
            if (worldY < lowerLeftBound.y) lowerLeftBound.y = worldY;
            if (worldX > upperRightBound.x) upperRightBound.x = worldX;
            if (worldY > upperRightBound.y) upperRightBound.y = worldY;
        }
    }

    /// <summary>
    /// Creates a serializable TileArrayData object from the solid array.
    /// </summary>
    /// <returns></returns>
    public TileArrayData GenerateTileArrayData() {

        Vector2Int arrayLowerLeftBound = new Vector2Int(lowerLeftBound.x + LevelEditorDataManager.instance.editorLevelData.levelSizeX, lowerLeftBound.y + LevelEditorDataManager.instance.editorLevelData.levelSizeY);
        Vector2Int arrayUpperRightBound = new Vector2Int(upperRightBound.x + LevelEditorDataManager.instance.editorLevelData.levelSizeX, upperRightBound.y + LevelEditorDataManager.instance.editorLevelData.levelSizeY);

        // crop the solid array
        bool[,] croppedSolidArray = new bool[arrayUpperRightBound.x - arrayLowerLeftBound.x + 1, arrayUpperRightBound.y - arrayLowerLeftBound.y + 1];
        for (int i = arrayLowerLeftBound.x; i <= arrayUpperRightBound.x; i++) {
            for (int j = arrayLowerLeftBound.y; j <= arrayUpperRightBound.y; j++) {
                croppedSolidArray[i - arrayLowerLeftBound.x, j - arrayLowerLeftBound.y] = solidArray[i, j];
            }
        }

        // flatten the array
        bool[] flatSolidArray = new bool[croppedSolidArray.GetLength(0) * croppedSolidArray.GetLength(1)];
        for (int i = 0; i < croppedSolidArray.GetLength(0); i++) {
            for (int j = 0; j < croppedSolidArray.GetLength(1); j++) {
                flatSolidArray[i * croppedSolidArray.GetLength(1) + j] = croppedSolidArray[i, j];
            }
        }

        // create tile array data
        TileArrayData tileArrayData = new TileArrayData {
            croppedSolidArray = flatSolidArray,
            solidArrayDimensions = new Vector2(croppedSolidArray.GetLength(0), croppedSolidArray.GetLength(1)),
            AutoTileSetID = autoTileSet.autoTileSetID,
            lowerLeftBound = lowerLeftBound,
            upperRightBound = upperRightBound
        };
        return tileArrayData;
    }

    public void LoadTileArrayData(TileArrayData data) {
        // parse the solid array
        bool[,] readSolidArray = new bool[(int)data.solidArrayDimensions.x, (int)data.solidArrayDimensions.y];
        for (int i = 0; i < data.solidArrayDimensions.x; i++) {
            for (int j = 0; j < data.solidArrayDimensions.y; j++) {
                readSolidArray[i, j] = data.croppedSolidArray[i * (int)data.solidArrayDimensions.y + j];
            }
        }

        // set this objects solid array
        for (int i = 0; i < readSolidArray.GetLength(0); i++) {
            for (int j = 0; j < readSolidArray.GetLength(1); j++) {
                Vector2Int arrayOffset = new Vector2Int(data.lowerLeftBound.x + LevelEditorDataManager.instance.editorLevelData.levelSizeX, data.lowerLeftBound.y + LevelEditorDataManager.instance.editorLevelData.levelSizeY);
                solidArray[i + arrayOffset.x, j + arrayOffset.y] = readSolidArray[i, j];
            }
        }

        // get the auto tile set
        autoTileSet = TileArrayManager.instance.GetAutoTileSetSOFromID(data.AutoTileSetID);

        // set bounds
        lowerLeftBound = data.lowerLeftBound;
        upperRightBound = data.upperRightBound;
    }


}


[Serializable]
public class TileArrayData {
    public bool[] croppedSolidArray;
    public Vector2 solidArrayDimensions;
    public string AutoTileSetID;
    public Vector2Int lowerLeftBound;
    public Vector2Int upperRightBound;
}