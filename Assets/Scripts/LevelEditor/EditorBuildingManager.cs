using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EditorMode
{
    BuildObjects,
    BuildTiles,
    Delete,
}

public class EditorBuildingManager : MonoBehaviour
{

    private EditorMode editorMode = EditorMode.BuildTiles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelEditorInputManager.instance.GetPlayerIsPlacing()) {
            if (editorMode == EditorMode.BuildObjects) {


            } else if (editorMode == EditorMode.BuildTiles) {
                TileArrayManager.instance.TryPlaceTile(GetMouseWorldPosition());


            } else if (editorMode == EditorMode.Delete) {


            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0;
        return vec;
    }


}
