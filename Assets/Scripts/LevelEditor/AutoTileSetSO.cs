using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "AutoTileSetSO", menuName = "AutoTileSetSO", order = 1)]
public class AutoTileSetSO : ScriptableObject
{
    public string autoTileSetName;
    public AutoTileSO[] autoTileSOs;

    [HideInInspector] private TileBitMaskAndRotation[] tileBitMaskAndRotations = new TileBitMaskAndRotation[256];

    /// <summary>
    /// The default tile to use when no other tile is found.
    /// </summary>
    public Tile defaultTile;

    public void GenerateAllTileBitMaskAndRotations() {
        foreach (AutoTileSO autoTileSO in autoTileSOs) {
            List<TileBitMaskAndRotation> tileBitMaskAndRotations = autoTileSO.GetAllTileBitMasksAndRotations();
            foreach (TileBitMaskAndRotation tileBitMaskAndRotation in tileBitMaskAndRotations) {
                this.tileBitMaskAndRotations[tileBitMaskAndRotation.bitMask] = tileBitMaskAndRotation;
            }
        }
    }

    public TileBitMaskAndRotation GetTileBitMaskAndRotation(int bitMask) {
        if (tileBitMaskAndRotations[bitMask] == null) {
            return new TileBitMaskAndRotation {
                bitMask = 0,
                rotation = 0,
                tile = defaultTile
            };
        }
        return tileBitMaskAndRotations[bitMask];
    }
}