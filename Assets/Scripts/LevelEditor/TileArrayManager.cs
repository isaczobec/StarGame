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
        
        // Create the tile arrays
        foreach (AutoTileSetSO autoTileSet in autoTileSet) {
            tileArrays.Add(new TileArray(autoTileSet, 
            LevelEditorDataManager.instance.editorLevelData.levelSizeX * 2, 
            LevelEditorDataManager.instance.editorLevelData.levelSizeY * 2));
        }

        currentTileArray = tileArrays[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void TryPlaceTile(Vector3 pos) {
        Vector3Int tilePos = tilemap.WorldToCell(pos);

        // only proceed if the tile is within the bounds of the level
        if (!tilemap.HasTile(tilePos)) {

            // get the tile and rotation
            TileBitMaskAndRotation tileBitMaskAndRotation = currentTileArray.GetTileBitMaskAndRotation(tilePos.x, tilePos.y);
            TileChangeData tileChangeData = new TileChangeData {
                tile = tileBitMaskAndRotation.tile,
                position = tilePos,
                transform = rotationDict[tileBitMaskAndRotation.rotation] // rotation matrix from a predefined dictionary
            };

            // set the tile
            tilemap.SetTile(tileChangeData, false);

            // Set solid
            currentTileArray.SetSolid(tilePos.x, tilePos.y, true);

        }
        
    }
}


public class TileArray {

    /// <summary>
    /// An array determining if a tile is solid or not, true if solid, false if not.
    /// </summary>
    public bool[,] solidArray;
    public AutoTileSetSO autoTileSet;

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
        solidArray[worldX + LevelEditorDataManager.instance.editorLevelData.levelSizeX, worldY + LevelEditorDataManager.instance.editorLevelData.levelSizeY] = solid;
    }

}