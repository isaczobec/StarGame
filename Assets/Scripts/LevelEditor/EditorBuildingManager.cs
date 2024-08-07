using System;
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
    [Header("UI Buttons")]
    [SerializeField] private UIButton buildObjectsButton;
    [SerializeField] private UIButton buildTilesButton;
    [SerializeField] private UIButton deleteButton;
    [Header("Visuals")]
    [Header("Colors")]
    [SerializeField] private Color buttonUnpressedColor = Color.white;
    [SerializeField] private Color buttonPressedColor = Color.magenta;


    private List<UIButton> topUIButtons = new List<UIButton>();

    private EditorMode editorMode = EditorMode.BuildTiles;

    private void Awake() {
        topUIButtons.Add(buildObjectsButton);
        topUIButtons.Add(buildTilesButton);
        topUIButtons.Add(deleteButton);
    }

    // Start is called before the first frame update
    void Start()
    {
        buildObjectsButton.OnUIButtonClicked += OnBuildObjectsButtonClicked;
        buildTilesButton.OnUIButtonClicked += OnBuildTilesButtonClicked;
        deleteButton.OnUIButtonClicked += OnDeleteButtonClicked;

        LevelEditorInputManager.instance.OnPlacePressed += OnPlacePressed;
        
    }


    private void OnBuildObjectsButtonClicked(object sender, EventArgs e)    {editorMode = EditorMode.BuildObjects;SetTopButtonSelected(buildObjectsButton);}
    private void OnBuildTilesButtonClicked(object sender, EventArgs e){editorMode = EditorMode.BuildTiles;SetTopButtonSelected(buildTilesButton);}
    private void OnDeleteButtonClicked(object sender, EventArgs e){editorMode = EditorMode.Delete;SetTopButtonSelected(deleteButton);}

    // Update is called once per frame
    void Update()
    {
        // if is placing, try to place or delete object
        if (LevelEditorInputManager.instance.GetPlayerIsPlacing()) {
            if (editorMode == EditorMode.BuildObjects) {
                // LevelEditorObjectManager.instance.TryPlaceEditorObject(GetMouseWorldPosition());

            } else if (editorMode == EditorMode.BuildTiles) {
                TileArrayManager.instance.TryPlaceTile(GetMouseWorldPosition());


            } else if (editorMode == EditorMode.Delete) {
                TileArrayManager.instance.TryDeleteTile(GetMouseWorldPosition());

            }
        }
    }

    private void OnPlacePressed(object sender, EventArgs e)
    {
        if (editorMode == EditorMode.BuildObjects) {
            LevelEditorObjectManager.instance.TryPlaceEditorObject(GetMouseWorldPosition());
        } 
    }

    private void SetTopButtonSelected(UIButton button) {
        foreach (UIButton b in topUIButtons) {
            if (b == button) {
                b.SetColor(buttonPressedColor);
            } else {
                b.SetColor(buttonUnpressedColor);
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
