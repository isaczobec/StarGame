using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileArrayManager : MonoBehaviour
{
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile testTile;

    public static TileArrayManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("There is already an instance of TileArrayManager in the scene.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryPlaceTile(Vector3 pos) {
        Vector3Int tilePos = tilemap.WorldToCell(pos);
        tilemap.SetTile(tilePos, testTile);
    } 
}
