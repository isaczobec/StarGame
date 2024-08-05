using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "AutoTile", menuName = "AutoTile")]
public class AutoTileSO : ScriptableObject {

    public Tile tile;
    [Header("put 1 for solid block, 0 for empty block")]
    public LegalTilePlacement[] legalTilePlacements;


    /// <summary>
    /// Get a list of all the possible bit masks and rotations of the tile for this tile.
    /// </summary>
    /// <returns></returns>
    public List<TileBitMaskAndRotation> GetAllTileBitMasksAndRotations() {
        List<TileBitMaskAndRotation> tileBitMaskAndRotations = new List<TileBitMaskAndRotation>();

        foreach (LegalTilePlacement legalTilePlacement in legalTilePlacements) {

            // add the default rotation
            tileBitMaskAndRotations.Add(new TileBitMaskAndRotation {
                bitMask = legalTilePlacement.GetBitMask(),
                rotation = 0,
                tile = tile
            });
    
            // if other rotations are allowed, add them
            if (legalTilePlacement.allow90DegreeRotation) {
                tileBitMaskAndRotations.Add(new TileBitMaskAndRotation {
                    bitMask = legalTilePlacement.Rotate90Degrees().GetBitMask(),
                    rotation = 1,
                    tile = tile
                });
            }
            if (legalTilePlacement.allow180DegreeRotation) {
                tileBitMaskAndRotations.Add(new TileBitMaskAndRotation {
                    bitMask = legalTilePlacement.Rotate90Degrees().Rotate90Degrees().GetBitMask(),
                    rotation = 2,
                    tile = tile
                });
            }
            if (legalTilePlacement.allow270DegreeRotation) {
                tileBitMaskAndRotations.Add(new TileBitMaskAndRotation {
                    bitMask = legalTilePlacement.Rotate90Degrees().Rotate90Degrees().Rotate90Degrees().GetBitMask(),
                    rotation = 3,
                    tile = tile
                });
            }

        }
        return tileBitMaskAndRotations;
    } 

}


public class TileBitMaskAndRotation {

    /// <summary>
    /// Should be between 0 and 255. 0 is in the top left, one in the top middle, then in the top right. and then going to the next row etc. The middle is the current tile and is skipped.
    /// </summary>
    public int bitMask;
    /// <summary>
    /// 0 for 0 degrees, 1 for 90 degrees, 2 for 180 degrees, 3 for 270 degrees.
    /// </summary>
    public int rotation;

    public Tile tile;
}


[Serializable]
public class LegalTilePlacement {

    // what adjacent tiles are solid
    public Vector3Int topRowSolid;
    public Vector3Int middleRowSolid;
    public Vector3Int bottomRowSolid;

    public bool allow90DegreeRotation = false;
    public bool allow180DegreeRotation = false;
    public bool allow270DegreeRotation = false;

    /// <summary>
    /// Returns a new LegalTilePlacement that is rotated 90 degrees.
    /// </summary>
    /// <returns></returns>
    public LegalTilePlacement Rotate90Degrees() {
            LegalTilePlacement newPlacement = new LegalTilePlacement {
                topRowSolid = new Vector3Int(topRowSolid.z, middleRowSolid.z, bottomRowSolid.z),
                middleRowSolid = new Vector3Int(topRowSolid.y, middleRowSolid.y, bottomRowSolid.y),
                bottomRowSolid = new Vector3Int(topRowSolid.x, middleRowSolid.x, bottomRowSolid.x),
            };
            return newPlacement;
    }

    /// <summary>
    /// add up the values of the vectors to get the bit mask.
    /// </summary>
    /// <returns></returns>
    public int GetBitMask() {
        int bitMask = 0;
        if (topRowSolid.x > 0) {
            bitMask += 1;
        }
        if (topRowSolid.y > 0) {
            bitMask += 2;
        }
        if (topRowSolid.z > 0) {
            bitMask += 4;
        }
        if (middleRowSolid.x > 0) {
            bitMask += 8;
        }
        // if (middleRowSolid.y > 0) SKIP THIS ONE, IT IS THE MIDDLE 
        //     bitMask += 16;
        // }
        if (middleRowSolid.z > 0) {
            bitMask += 16;
        }
        if (bottomRowSolid.x > 0) {
            bitMask += 32;
        }
        if (bottomRowSolid.y > 0) {
            bitMask += 64;
        }
        if (bottomRowSolid.z > 0) {
            bitMask += 128;
        }

        return bitMask;
    }
}

