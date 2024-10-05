using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum EditorMode
{
    PointerMode,
    BuildMode,
    Delete,
}

/// <summary>
/// class responsible for managing the building of objects and tiles in the editor. Also ui building buttons and such.
/// </summary>
public class EditorBuildingManager : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private UIButton pointerButton;
    [SerializeField] private UIButton deleteButton;
    [Header("Visuals")]
    [Header("Colors")]
    [SerializeField] private Color buttonUnpressedColor = Color.white;
    [SerializeField] private Color buttonPressedColor = Color.magenta;
    [Header("Object button settings")]
    [SerializeField] private LevelEditorObjectPanel levelEditorObjectPanel;
    [SerializeField] private TilePanel tilePanel;

    /// <summary>
    /// Can be set by other classes to block the player from placing objects.
    /// </summary>
    private bool externalBlockingPlacing = false; 
    /// <summary>
    /// Blocks or unblocks the player from placing objects.
    /// </summary>
    /// <param name="value"></param>
    public void SetExternalBlockingPlacing(bool value) {
        externalBlockingPlacing = value;
    }


    private List<UIButton> toolButtons = new List<UIButton>();
    private EditorMode editorMode = EditorMode.PointerMode;
    public void SetEditorMode(EditorMode mode) {
        editorMode = mode;

        // deselect all buttons if entering build mode (not represented by one of theese buttons)
        if (mode == EditorMode.BuildMode) {
            foreach (UIButton button in toolButtons) {
                button.SetColor(buttonUnpressedColor);
            }
        } 
    }
    private List<UIButton> editorUIButtons = new List<UIButton>();


    /// <summary>
    /// Adds a button to the editorUIButtons list. The button will be considered when checking if the player is hovering ui.
    /// </summary>
    /// <param name="button"></param>
    public void AddEditorUIButton(UIButton button) {
        editorUIButtons.Add(button);
        button.OnUIButtonHoveredChanged += OnUIButtonHoveredChanged;
    }

    /// <summary>
    /// Removes a button from the editorUIButtons list. The buitton will no longer be considered when checking if the player is hovering ui.
    /// </summary>
    /// <param name="button"></param>
    public void RemoveEditorUIButton(UIButton button) {
        editorUIButtons.Remove(button);
        button.OnUIButtonHoveredChanged -= OnUIButtonHoveredChanged;
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
        toolButtons.Add(pointerButton);
        toolButtons.Add(deleteButton);

        foreach (UIButton button in toolButtons) {
            AddEditorUIButton(button);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        // sub to events
        pointerButton.OnUIButtonClicked += OnPointerButtonClicked;
        deleteButton.OnUIButtonClicked += OnDeleteButtonClicked;

        LevelEditorInputManager.instance.OnPlacePressed += OnPlacePressed;

        // initialize object buttons
        levelEditorObjectPanel.InitializeButtons(LevelEditorObjectManager.instance.GetEditorObjectCategories());

        OnPointerButtonClicked(null, null); // set pointer button as default
    }


    private void OnPointerButtonClicked(object sender, EventArgs e){editorMode = EditorMode.PointerMode;SetToolButtonSelected(pointerButton);}
    private void OnDeleteButtonClicked(object sender, EventArgs e){editorMode = EditorMode.Delete;SetToolButtonSelected(deleteButton);}

    // Update is called once per frame
    void Update()
    {
        HandleConstantPlacing();
    }

    private void HandleConstantPlacing()
    {

        // if hovering ui, return
        if (isHoveringUI || externalBlockingPlacing) return;

        // if is placing, try to place or delete object
        if (LevelEditorInputManager.instance.GetPlayerIsPlacing())
        {
            if (editorMode == EditorMode.BuildMode)
            {
                bool succeded = TileArrayManager.instance.TryPlaceTile(GetMouseWorldPosition());
                if (succeded) {LevelEditorObjectManager.instance.DeSelectAllObjects();}

            }
            else if (editorMode == EditorMode.Delete)
            {
                TileArrayManager.instance.TryDeleteTile(GetMouseWorldPosition());
                LevelEditorObjectManager.instance.DeleteHoveredObjects();
                LevelEditorObjectManager.instance.DeSelectAllObjects();

            }
        }
    }

    private void OnPlacePressed(object sender, EventArgs e)
    {

        // if hovering ui, return
        if (isHoveringUI || externalBlockingPlacing) return;

        if (editorMode == EditorMode.BuildMode) {
            LevelEditorObjectManager.instance.TryPlaceEditorObject(GetMouseWorldPosition(), setSelectedByDefault: true, deselectOtherObjects: !LevelEditorInputManager.instance.GetShiftButtonPressed());
        } 

        // handle selecting objects

        if (editorMode == EditorMode.PointerMode) {
            if (!LevelEditorInputManager.instance.GetShiftButtonPressed() || LevelEditorObjectManager.instance.GetAmountOfHoveredObjects() == 0) {LevelEditorObjectManager.instance.DeSelectAllObjects();}
            if (LevelEditorObjectManager.instance.GetAmountOfHoveredObjects() > 0) LevelEditorObjectManager.instance.AddHoveredObjectsToSelected();
        }
    }

    private void SetToolButtonSelected(UIButton button) {
        foreach (UIButton b in toolButtons) {
            if (b == button) {
                b.SetColor(buttonPressedColor);
            } else {
                b.SetColor(buttonUnpressedColor);
            }
        }

        // deselect other buttons
        LevelEditorObjectManager.instance.DeSelectCurrentObjectToPlace();
        TileArrayManager.instance.DeSelectTileArray();
    }



    private Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0;
        return vec;
    }

    public void EnableBuildingMode() {
        SetEditorMode(EditorMode.BuildMode);
    }

    


}

