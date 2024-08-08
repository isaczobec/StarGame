using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.Tilemaps;

/// <summary>
/// A class that represents a panel of tiles in the level editor.
/// </summary>
public class TilePanel : MonoBehaviour {
 [SerializeField] private float buttonOffsetWidth = 2f;
    [SerializeField] private float buttonOffsetHeight = 2f;

    [SerializeField] private GameObject objectButtonPrefab;


    [Header("Visuals")]
    [SerializeField] private Color buttonUnpressedColor = Color.white;
    [SerializeField] private Color buttonPressedColor = Color.green;


    List<TileArray> tileArrays;
    private int currentTileArrayIndex = 0;
    TileArray activeTileArray;

    [SerializeField] private int maxColumns = 6;


    private Dictionary<UIButton, TileArray> buttonTileDict = new Dictionary<UIButton, TileArray>(); 


    private UIButton activeButton;


    public static TilePanel instance {get; private set;}
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Debug.LogError("There is already an instance of TilePanel in the scene.");
        }
    }

    private void Start() {

    }


    public void InitializeButtons(List<TileArray> tileArrays, bool setDefaultActive = true) {
        this.tileArrays = tileArrays;

        for (int i = 0; i < tileArrays.Count; i++) {


                // create the button gameobject and offset it
                GameObject newButton = Instantiate(objectButtonPrefab, transform);
                int column = i % maxColumns;
                int row = i / maxColumns;
                newButton.transform.localPosition = new Vector3(column * buttonOffsetWidth, row * buttonOffsetHeight, 0f);

                
                // add to dict to track buttons and sub to click event
                UIButtonAnimated button = newButton.GetComponent<UIButtonAnimated>();
                buttonTileDict.Add(button, tileArrays[i]);
                button.OnUIButtonClicked += OnTileButtonClicked;

                // set visuals
                button.SetIcon(tileArrays[i].autoTileSet.defaultTile.sprite);


                // add the object to the buildingmanager
                EditorBuildingManager.instance.AddEditorUIButton(button);
            
        }

        // set the first tileArray active
        if (setDefaultActive) {
            SetActiveTileArrayThroughButtonClick(buttonTileDict.Keys.ElementAt(0));
        }

    }

    public void DeSelectTileArray() {
        if (activeButton != null) {
            activeButton.SetColor(buttonUnpressedColor);
        }
        activeButton = null;
        activeTileArray = null;
    }

    public void SetActiveTileArray(TileArray tileArray) {
        activeTileArray = tileArray;
        TileArrayManager.instance.SetCurrentTileArray(tileArray);
    }

    private void OnTileButtonClicked(object sender, EventArgs e)
    {
        SetActiveTileArrayThroughButtonClick((UIButton)sender);
    }

    private void SetActiveTileArrayThroughButtonClick(UIButton clickedButton) {
        if (buttonTileDict.ContainsKey(clickedButton)) {



            // update visuals
            if (activeButton != null) {
                activeButton.SetColor(buttonUnpressedColor);
            }
            activeButton = clickedButton; // set the new active button
            activeButton.SetColor(buttonPressedColor);

            // find and set the active object
            TileArray tileArray = buttonTileDict[clickedButton];
            SetActiveTileArray(tileArray);

            // deselect others
            LevelEditorObjectManager.instance.DeSelectCurrentObjectToPlace();
            EditorBuildingManager.instance.EnableBuildingMode();
        }
    }
}