using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum EditorMode
{
    BuildObjects,
    BuildTiles,
    Delete,
}

/// <summary>
/// class responsible for managing the building of objects and tiles in the editor. Also ui building buttons and such.
/// </summary>
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
    [Header("Object button settings")]
    [SerializeField] private LevelEditorObjectPanel levelEditorObjectPanel;



    private List<UIButton> toolButtons = new List<UIButton>();

    private EditorMode editorMode = EditorMode.BuildTiles;


    private List<UIButton> editorUIButtons = new List<UIButton>();


    public void AddEditorUIButton(UIButton button) {
        editorUIButtons.Add(button);
        button.OnUIButtonHoveredChanged += OnUIButtonHoveredChanged;
    }

    private void OnUIButtonHoveredChanged(object sender, bool e)
    {
        isHoveringUI = e;
    }

    private bool isHoveringUI = false;
    public static EditorBuildingManager instance {get; private set;}

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("There is already an instance of EditorBuildingManager in the scene.");
        }
        toolButtons.Add(buildObjectsButton);
        toolButtons.Add(buildTilesButton);
        toolButtons.Add(deleteButton);

        foreach (UIButton button in toolButtons) {
            AddEditorUIButton(button);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        // sub to events
        buildObjectsButton.OnUIButtonClicked += OnBuildObjectsButtonClicked;
        buildTilesButton.OnUIButtonClicked += OnBuildTilesButtonClicked;
        deleteButton.OnUIButtonClicked += OnDeleteButtonClicked;

        LevelEditorInputManager.instance.OnPlacePressed += OnPlacePressed;

        // initialize object buttons
        levelEditorObjectPanel.InitializeButtons(LevelEditorObjectManager.instance.GetEditorObjectCategories());
    }


    private void OnBuildObjectsButtonClicked(object sender, EventArgs e)    {editorMode = EditorMode.BuildObjects;SetTopButtonSelected(buildObjectsButton);}
    private void OnBuildTilesButtonClicked(object sender, EventArgs e){editorMode = EditorMode.BuildTiles;SetTopButtonSelected(buildTilesButton);}
    private void OnDeleteButtonClicked(object sender, EventArgs e){editorMode = EditorMode.Delete;SetTopButtonSelected(deleteButton);}

    // Update is called once per frame
    void Update()
    {
        HandleConstantPlacing();
    }

    private void HandleConstantPlacing()
    {

        // if hovering ui, return
        if (isHoveringUI) return;

        // if is placing, try to place or delete object
        if (LevelEditorInputManager.instance.GetPlayerIsPlacing())
        {
            if (editorMode == EditorMode.BuildObjects)
            {
                // LevelEditorObjectManager.instance.TryPlaceEditorObject(GetMouseWorldPosition());

            }
            else if (editorMode == EditorMode.BuildTiles)
            {
                TileArrayManager.instance.TryPlaceTile(GetMouseWorldPosition());


            }
            else if (editorMode == EditorMode.Delete)
            {
                TileArrayManager.instance.TryDeleteTile(GetMouseWorldPosition());

            }
        }
    }

    private void OnPlacePressed(object sender, EventArgs e)
    {

        // if hovering ui, return
        if (isHoveringUI) return;

        if (editorMode == EditorMode.BuildObjects) {
            LevelEditorObjectManager.instance.TryPlaceEditorObject(GetMouseWorldPosition());
        } 
    }

    private void SetTopButtonSelected(UIButton button) {
        foreach (UIButton b in toolButtons) {
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

